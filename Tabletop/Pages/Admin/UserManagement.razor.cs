using Blazor.Pagination;
using DbController;
using DbController.MySql;
using Tabletop.Core.Extensions;
using Tabletop.Core.Filters;
using Tabletop.Core.Models;
using Tabletop.Core.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Tabletop.Pages.Public;
using Microsoft.AspNetCore.Mvc;

namespace Tabletop.Pages.Admin
{
    public partial class UserManagement : IHasPagination
    {
        private int _page = 1;
        private User? _loggedInUser;
        public UserFilter Filter { get; set; } = new()
        {
            Limit = AppdataService.PageLimit
        };
        public Permission? SelectedPermission { get; set; }
        public List<Fraction> Fractions { get; set; } = new();
        public int Page { get => _page; set => _page = value < 1 ? 1 : value; }
        public int TotalItems { get; set; }
        protected override async Task OnParametersSetAsync()
        {
            Fractions = AppdataService.Fractions;
            _loggedInUser = await authService.GetUserAsync();
            await LoadAsync();
        }
        protected override async Task SaveAsync()
        {
            if (Input is null)
            {
                return;
            }

            PasswordHasher<User> hasher = new();
            string passwordHashed = hasher.HashPassword(Input, Input.Password + Input.Salt);
            Input.Password = passwordHashed;

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
            Input = new User
            {
                Salt = StringExtensions.RandomString(10)
            };

            return Task.CompletedTask;
        }

        private Task AddPermissionAsync()
        {
            if (Input is not null)
            {
                if (SelectedPermission is not null)
                {
                    Input.Permissions.Add(SelectedPermission);
                }

                SelectedPermission = null;
            }

            return Task.CompletedTask;
        }

        private Task PermissionSelectionChangedAsync(ChangeEventArgs e)
        {
            int permissionId = Convert.ToInt32(e.Value);
            SelectedPermission = AppdataService.Permissions.FirstOrDefault(x => x.PermissionId == permissionId);
            return Task.CompletedTask;
        }
    }
}