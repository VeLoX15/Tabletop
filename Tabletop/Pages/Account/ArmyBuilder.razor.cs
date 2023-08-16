using Blazor.Pagination;
using DbController;
using DbController.MySql;
using Microsoft.AspNetCore.Components;
using Tabletop.Core.Extensions;
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

        public List<User> Friends { get; set; } = new();
        public List<Player> Players { get; set; } = new();
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
            if (Input is not null)
            {
                if (SelectedUnit is not null)
                {
                    Input.Units.Add(SelectedUnit);
                }

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
    }
}