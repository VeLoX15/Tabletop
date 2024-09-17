using Blazor.Pagination;
using DbController;
using DbController.SqlServer;
using Microsoft.AspNetCore.Components;
using Tabletop.Core;
using Tabletop.Core.Calculators;
using Tabletop.Core.Filters;
using Tabletop.Core.Models;
using Tabletop.Core.Services;

namespace Tabletop.Pages.Tools
{
    public partial class ArmyBuilder : IHasPagination
    {
        private int _page = 1;
        private User? _loggedInUser;
        public TemplateFilter Filter { get; set; } = new();

        public Unit? SelectedUnit { get; set; }
        public int Page { get => _page; set => _page = value < 1 ? 1 : value; }
        public int TotalItems { get; set; }
        public bool OpenFilter { get; set; }
        public bool ShowArmy { get; set; }


        protected override async Task OnParametersSetAsync()
        {
            _loggedInUser = await authService.GetUserAsync();

            if (_loggedInUser != null)
            {
                using IDbController dbController = new SqlController(AppdataService.ConnectionString);
                _loggedInUser.Units = await UnitService.GetUserUnitsAsync(_loggedInUser.UserId, dbController);

                OpenFilter = false;
                ShowArmy = false;

                Filter = new()
                {
                    Limit = AppdataService.PageLimit,
                    UserId = _loggedInUser.UserId
                };
            }

            await LoadAsync();
        }

        protected override async Task SaveAsync()
        {
            if (Input is null)
            {
                return;
            }

            Input.Units.RemoveAll(unit => unit.Quantity == 0);

            await base.SaveAsync();
            await LoadAsync();
        }

        public async Task LoadAsync(bool navigateToPage1 = false)
        {
            Filter.PageNumber = navigateToPage1 ? 1 : Page;
            using IDbController dbController = new SqlController(AppdataService.ConnectionString);
            TotalItems = await Service.GetTotalAsync(Filter, dbController);
            Data = await Service.GetAsync(Filter, dbController);
        }

        protected override async Task DeleteAsync()
        {
            await base.DeleteAsync();
            await LoadAsync();
        }

        protected override async Task EditAsync(Template input)
        {
            await base.EditAsync(input);
            await CalculateTotalForceAsync();
            await CalculateTotalCountAsync();
            await CalculateForceAsync();
        }

        protected override Task NewAsync()
        {
            if (_loggedInUser != null)
            {
                Input = new Template
                {
                    UserId = _loggedInUser.UserId
                };
            }

            return Task.CompletedTask;
        }

        private async Task AddUnitAsync()
        {
            if (SelectedUnit is not null)
            {
                Input?.Units.Add(SelectedUnit);

                SelectedUnit = null;
                await CalculateForceAsync();
            }
        }

        private Task UnitSelectionChangedAsync(ChangeEventArgs e)
        {
            int unitId = Convert.ToInt32(e.Value);
            SelectedUnit = AppdataService.Units.FirstOrDefault(x => x.UnitId == unitId)?.DeepCopyByExpressionTree();
            return Task.CompletedTask;
        }

        private async Task CalculateTotalForceAsync()
        {
            int totalForce = 0;
            if (Input is not null)
            {
                foreach (var unit in Input.Units)
                {
                    int unitForce = await Calculation.ForceAsync(unit);
                    totalForce += unitForce * unit.Quantity;
                }
                Input.UsedForce = totalForce;
            }
        }

        private async Task CalculateForceAsync()
        {
            if (Input is not null)
            {
                foreach (var unit in Input.Units)
                {
                    int force = await Calculation.ForceAsync(unit);

                    unit.Force = force;
                    unit.ForceOfQuantity = force * unit.Quantity;
                }
            }
        }

        private Task ClearUnitsAsync()
        {
            if (Input is not null)
            {
                Input.Units.Clear();
                Input.UsedForce = 0;
                Input.TotalUnits = 0;
            }

            return Task.CompletedTask;
        }

        private async Task ClearUnitAsync(Unit unit)
        {
            unit.Quantity = 0;
            if (Input != null)
            {
                Input.TotalUnits = 0;
                Input.Units.Remove(unit);
            }

            await CalculateTotalCountAsync();
            await CalculateTotalForceAsync();
        }


        private async Task IncrementAsync(Unit unit)
        {
            if (await CheckTroopSize(unit) && await CheckAllowedUnitsOfClass(unit) && Input?.Force > Input?.UsedForce)
            {
                var loggedInUnit = _loggedInUser?.Units?.FirstOrDefault(x => x.UnitId == unit.UnitId);
                if (unit.Quantity < loggedInUnit?.Quantity)
                {
                    if (unit.Quantity % unit.TroopQuantity < unit.TroopQuantity / 2)
                    {
                        int incrementAmount = unit.TroopQuantity / 2;
                        if (unit.Quantity + incrementAmount <= loggedInUnit.Quantity)
                        {
                            unit.Quantity += incrementAmount;
                        }
                    }
                    else
                    {
                        unit.Quantity++;
                    }

                    await CalculateTotalCountAsync();
                    await CalculateTotalForceAsync();
                    await CalculateForceAsync();
                }
            }
        }

        private async Task DecrementAsync(Unit unit)
        {
            if (await CheckTroopSize(unit) && unit.Quantity > 0)
            {
                if (unit.Quantity % unit.TroopQuantity == unit.TroopQuantity / 2)
                {
                    unit.Quantity -= unit.TroopQuantity % 2 == 0 ? unit.TroopQuantity / 2 : 1;
                }
                else
                {
                    unit.Quantity--;
                }

                await CalculateTotalCountAsync();
                await CalculateTotalForceAsync();
                await CalculateForceAsync();
            }
        }

        private Task<int> CalculateTotalCountAsync()
        {
            if (Input != null)
            {
                Input.TotalUnits = Input.Units.Sum(unit => unit.Quantity);
            }
            return Task.FromResult(0);
        }

        private Task<bool> CheckTroopSize(Unit unit)
        {
            if (Input == null)
            {
                return Task.FromResult(false);
            }

            bool isValid = Input.Units
                .Where(x => x.ClassId == unit.ClassId && x != unit)
                .All(x => x.Quantity % x.TroopQuantity == 0);

            return Task.FromResult(isValid);
        }


        private Task<bool> CheckAllowedUnitsOfClass(Unit unit)
        {
            if (Input != null)
            {
                if (unit.ClassId == 1)
                {
                    return Task.FromResult(true);
                }

                int maxOfClass = Input.Force / 200;
                int numberOfTroops = 0;

                foreach (var item in Input.Units.Where(x => x.ClassId == unit.ClassId))
                {
                    int itemTroops = (int)Math.Ceiling((double)item.Quantity / item.TroopQuantity);

                    if (item == unit)
                    {
                        itemTroops = (int)Math.Ceiling(((double)item.Quantity + 1) / item.TroopQuantity);
                    }

                    numberOfTroops += itemTroops;

                    if (numberOfTroops > maxOfClass)
                    {
                        return Task.FromResult(false);
                    }
                }

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

    }
}