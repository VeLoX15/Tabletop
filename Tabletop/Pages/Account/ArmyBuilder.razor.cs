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
        public int Page { get => _page; set => _page = value < 1 ? 1 : value; }
        public int TotalItems { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            _loggedInUser = await authService.GetUserAsync();

            if (_loggedInUser != null)
            {
                using IDbController dbController = new MySqlController(AppdataService.ConnectionString);
                _loggedInUser.Units = await unitService.GetUserUnitsAsync(_loggedInUser.UserId, dbController);

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

        private Task AddUnitAsync()
        {
            if (Input is not null && SelectedUnit is not null)
            {
                Input.Units.Add(SelectedUnit);

                SelectedUnit = null;
            }

            return Task.CompletedTask;
        }

        private Task UnitSelectionChangedAsync(ChangeEventArgs e)
        {
            int unitId = Convert.ToInt32(e.Value);
            SelectedUnit = AppdataService.Units.FirstOrDefault(x => x.UnitId == unitId);
            return Task.CompletedTask;
        }

        private async Task CalculateTotalForceAsync()
        {
            int totalForce = 0;
            if (Input is not null)
            {
                foreach (var unit in Input.Units)
                {
                    unit.PrimaryWeapon = AppdataService.Weapons.FirstOrDefault(x => x.WeaponId == unit.PrimaryWeaponId);
                    unit.SecondaryWeapon = AppdataService.Weapons.FirstOrDefault(x => x.WeaponId == unit.SecondaryWeaponId);

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
                    unit.PrimaryWeapon = AppdataService.Weapons.FirstOrDefault(x => x.WeaponId == unit.PrimaryWeaponId);
                    unit.SecondaryWeapon = AppdataService.Weapons.FirstOrDefault(x => x.WeaponId == unit.SecondaryWeaponId);

                    int force = await Calculation.ForceAsync(unit);

                    unit.Force = force;
                    unit.ForceOfQuantity = force * unit.Quantity;
                }
            }
        }

        private async Task Increment(Unit unit)
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

        private async Task Decrement(Unit unit)
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
}