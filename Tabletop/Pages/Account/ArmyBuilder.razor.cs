using Blazor.Pagination;
using DbController;
using DbController.MySql;
using Microsoft.AspNetCore.Components;
using Tabletop.Core.Calculators;
using Tabletop.Core.Filters;
using Tabletop.Core.Models;
using Tabletop.Core.Services;

namespace Tabletop.Pages.Account
{
    public partial class ArmyBuilder : IHasPagination
    {
        private int _page = 1;
        private User? _loggedInUser;
        public TemplateFilter Filter { get; set; } = new();

        public Unit? SelectedUnit { get; set; }
        public List<Unit> Units { get; set; } = new();
        public List<Weapon> Weapons { get; set; } = new();
        public int Page { get => _page; set => _page = value < 1 ? 1 : value; }
        public int TotalItems { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            _loggedInUser = await authService.GetUserAsync();

            if (_loggedInUser != null)
            {
                using IDbController dbController = new MySqlController(AppdataService.ConnectionString);
                _loggedInUser.Units = await unitService.GetUserUnitsAsync(_loggedInUser.UserId, dbController);

                Units = AppdataService.Units;
                Weapons = AppdataService.Weapons;

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
            using IDbController dbController = new MySqlController(AppdataService.ConnectionString);
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
            SelectedUnit = Units.FirstOrDefault(x => x.UnitId == unitId);
            return Task.CompletedTask;
        }

        private async Task CalculateTotalForceAsync()
        {
            int totalForce = 0;
            if (Input is not null)
            {
                foreach (var unit in Input.Units)
                {
                    unit.PrimaryWeapon = Weapons.FirstOrDefault(x => x.WeaponId == unit.PrimaryWeaponId);
                    unit.SecondaryWeapon = Weapons.FirstOrDefault(x => x.WeaponId == unit.SecondaryWeaponId);

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
                    unit.PrimaryWeapon = Weapons.FirstOrDefault(x => x.WeaponId == unit.PrimaryWeaponId);
                    unit.SecondaryWeapon = Weapons.FirstOrDefault(x => x.WeaponId == unit.SecondaryWeaponId);

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
            }

            return Task.CompletedTask;
        }

        private async Task ClearUnitAsync(Unit unit)
        {
            unit.Quantity = 0;
            Input?.Units.Remove(unit);
            await CalculateTotalForceAsync();
        }

        private async Task IncrementAsync(Unit unit)
        {
            if (await CheckTroopSize(unit) && await CheckAllowedUnitsOfClass(unit) && Input?.Force > Input?.UsedForce)
            {
                if (unit.Quantity < _loggedInUser?.Units?.FirstOrDefault(x => x.UnitId == unit.UnitId)?.Quantity)
                {
                    unit.Quantity++;
                }

                int quantity = Input?.Units?.FirstOrDefault(x => x.UnitId == unit.UnitId)?.Quantity ?? 0;
                unit.Quantity = quantity;

                await CalculateTotalForceAsync();
                await CalculateForceAsync();
            }
        }

        private async Task DecrementAsync(Unit unit)
        {
            if (await CheckTroopSize(unit))
            {
                if (unit.Quantity > 0)
                {
                    unit.Quantity--;
                }

                int quantity = Input?.Units?.FirstOrDefault(x => x.UnitId == unit.UnitId)?.Quantity ?? 0;
                unit.Quantity = quantity;

                await CalculateTotalForceAsync();
                await CalculateForceAsync();
            }
        }

        private Task<bool> CheckTroopSize(Unit unit)
        {
            if (Input is not null)
            {
                foreach (var item in Input.Units.Where(x => x.ClassId == unit.ClassId))
                {
                    if (item.Quantity % item.TroopQuantity != 0 && item != unit)
                    {
                        return Task.FromResult(false);
                    }
                }

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        private Task<bool> CheckAllowedUnitsOfClass(Unit unit)
        {
            if (Input is not null)
            {
                if (unit.ClassId == 1)
                {
                    return Task.FromResult(true);
                }

                int maxOfClass = (Input.Force / 200);
                int numberOfTroops;

                foreach (var item in Input.Units.Where(x => x.ClassId == unit.ClassId))
                {
                    if (item.Quantity > 0 || item == unit)
                    {
                        if (maxOfClass > 0)
                        {
                            if (item == unit)
                            {
                                numberOfTroops = (int)Math.Ceiling(((double)item.Quantity + 1) / (double)item.TroopQuantity);
                            }
                            else
                            {
                                numberOfTroops = (int)Math.Ceiling((double)item.Quantity / (double)item.TroopQuantity);
                            }

                            maxOfClass -= numberOfTroops;
                        }
                        else
                        {
                            return Task.FromResult(false);
                        }
                    }
                }

                if(maxOfClass >= 0)
                {
                    return Task.FromResult(true);
                }
            }

            return Task.FromResult(false);
        }
    }
}