using DbController.MySql;
using DbController;
using Microsoft.AspNetCore.Components;
using Tabletop.Core.Models;
using Tabletop.Core.Services;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Forms;
using Tabletop.Core.Calculators;

namespace Tabletop.Pages.Account
{
    public partial class GamePage
    {
        [Parameter]
        public int GameId { get; set; }
        public Game? Game { get; set; }

#nullable disable
        [Inject] public IJSRuntime JSRuntime { get; set; }
#nullable enable

        private User? _loggedInUser;

        public Player? Player { get; set; } //Player Model for Army Builder Modal

        protected EditForm? _formGame;
        protected EditForm? _formArmy;

        public int SelectedTeam { get; set; }
        public int SelectedTemplate { get; set; }

        public bool PlayersReady { get; set; } = false;
        public Unit? SelectedUnit { get; set; }
        public Player? SelectedPlayer { get; set; }
        public List<Player> Friends { get; set; } = new(); //Friends of Game Host
        public List<Player> SelectedPlayers { get; set; } = new(); //Players of the Game
        public List<Template> Templates { get; set; } = new();
        public List<Unit> Units { get; set; } = new();
        public List<Weapon> Weapons { get; set; } = new();
        public List<Ability> Abilities { get; set; } = new();

        protected override async Task OnParametersSetAsync()
        {
            _loggedInUser = await authService.GetUserAsync();

            using IDbController dbController = new MySqlController(AppdataService.ConnectionString);
            Game = await gameService.GetAsync(GameId, dbController);

            Units = AppdataService.Units;
            Weapons = AppdataService.Weapons;
            Abilities = AppdataService.Abilities;

            if (_loggedInUser is not null && Game is not null)
            {
                _loggedInUser.Units = await unitService.GetUserUnitsAsync(_loggedInUser.UserId, dbController);
                Templates = await templateService.GetTemplateOnForceAsync(_loggedInUser.UserId, Game.Force, dbController);
            }

            if (Game is not null)
            {
                foreach (Player player in Game.Players)
                {
                    player.AllowedForce = Game.Force;
                    await CalculateArmyDataAsync(player);
                }
            }

            await FriendReloading();
            await CheckPlayerIsReady();
        }

        protected async Task FriendReloading()
        {
            if (Game is not null)
            {
                using IDbController dbController = new MySqlController(AppdataService.ConnectionString);

                List<User> friends = await userService.GetUserFriendsAsync(Game.UserId, dbController);

                foreach (var user in friends)
                {
                    Friends.Add(new Player()
                    {
                        User = user
                    });
                }
            }
        }

        protected async Task SavePlayersAsync()
        {
            if (_formGame is null || _formGame.EditContext is null || Game is null)
            {
                return;
            }

            if (_formGame.EditContext.Validate())
            {
                using IDbController dbController = new MySqlController(AppdataService.ConnectionString);
                await dbController.StartTransactionAsync();

                try
                {
                    if (_loggedInUser != null)
                    {
                        SelectedPlayers.Add(new Player()
                        {
                            GameId = Game.GameId,
                            UserId = _loggedInUser.UserId,
                            Team = 1,
                            User = _loggedInUser
                        });
                    }
                    Game.Players = SelectedPlayers;
                    await gameService.UpdateAsync(Game, dbController);

                    await dbController.CommitChangesAsync();
                    AppdataService.UpdateRecord(Game);
                }
                catch (Exception)
                {
                    await dbController.RollbackChangesAsync();
                    throw;
                }

                await JSRuntime.ShowToastAsync(ToastType.success, $"Save item");
            }
        }

        protected async Task SaveArmyAsync()
        {
            if (_formArmy is null || _formArmy.EditContext is null || Player is null || Game is null)
            {
                return;
            }

            if (_formArmy.EditContext.Validate())
            {
                using IDbController dbController = new MySqlController(AppdataService.ConnectionString);
                await dbController.StartTransactionAsync();
                try
                {
                    var playerUnitsToUpdate = Game.Players.FirstOrDefault(x => x.PlayerId == Player.PlayerId)?.StartUnits;
                    playerUnitsToUpdate = Player.StartUnits;

                    await playerService.UpdateAsync(Player, dbController);
                    await dbController.CommitChangesAsync();
                    AppdataService.UpdateRecord(Player);
                }
                catch (Exception)
                {
                    await dbController.RollbackChangesAsync();
                    throw;
                }


                await JSRuntime.ShowToastAsync(ToastType.success, $"Save item");

                var playerToUpdate = Game?.Players.FirstOrDefault(x => x.PlayerId == Player.PlayerId);
                if (playerToUpdate != null)
                {
                    playerToUpdate.StartUnits = Player.StartUnits;
                    playerToUpdate.FractionId = Player.FractionId;
                }

                Player = null;
            }
        }

        private Task CheckPlayerIsReady()
        {
            if (Game is not null)
            {
                int count = 0;

                foreach (var player in Game.Players)
                {
                    if (player.StartUnits.Any())
                    {
                        count++;
                    }
                }

                if (Game.Players.Count == count)
                {
                    PlayersReady = true;
                }
            }

            return Task.CompletedTask;
        }

        private Task AddUserAsync()
        {
            if (Game is not null && SelectedPlayer is not null)
            {

                SelectedPlayer.GameId = Game.GameId;
                SelectedPlayer.UserId = SelectedPlayer.User.UserId;
                SelectedPlayer.Team = SelectedTeam;

                SelectedPlayers.Add(SelectedPlayer);
                SelectedPlayer = null;
            }

            return Task.CompletedTask;
        }

        private Task UserSelectionChangedAsync(ChangeEventArgs e)
        {
            int userId = Convert.ToInt32(e.Value);
            SelectedPlayer = Friends.FirstOrDefault(x => x.User.UserId == userId);
            return Task.CompletedTask;
        }

        private async Task AddUnitAsync()
        {
            if (Player is not null && SelectedUnit is not null)
            {
                Player.StartUnits.Add(SelectedUnit);

                SelectedUnit = null;
                await CalculateForceAsync(Player);
            }
        }

        private Task UnitSelectionChangedAsync(ChangeEventArgs e)
        {
            int unitId = Convert.ToInt32(e.Value);
            SelectedUnit = Units.FirstOrDefault(x => x.UnitId == unitId);
            return Task.CompletedTask;
        }

        private async Task CalculateTotalForceAsync(Player player)
        {
            if (player is not null)
            {
                int totalForce = 0;

                foreach (var unit in player.StartUnits)
                {
                    unit.PrimaryWeapon = Weapons.FirstOrDefault(x => x.WeaponId == unit.PrimaryWeaponId);
                    unit.SecondaryWeapon = Weapons.FirstOrDefault(x => x.WeaponId == unit.SecondaryWeaponId);
                    unit.Ability = Abilities.FirstOrDefault(x => x.AbilityId == unit.AbilityId);

                    int unitForce = await Calculation.ForceAsync(unit);
                    totalForce += unitForce * unit.Quantity;
                }
                player.UsedForce = totalForce;
            }
        }

        private async Task CalculateForceAsync(Player player)
        {
            if (player is not null)
            {
                foreach (var unit in player.StartUnits)
                {
                    unit.PrimaryWeapon = Weapons.FirstOrDefault(x => x.WeaponId == unit.PrimaryWeaponId);
                    unit.SecondaryWeapon = Weapons.FirstOrDefault(x => x.WeaponId == unit.SecondaryWeaponId);
                    unit.Ability = Abilities.FirstOrDefault(x => x.AbilityId == unit.AbilityId);

                    int force = await Calculation.ForceAsync(unit);

                    unit.Force = force;
                    unit.ForceOfQuantity = force * unit.Quantity;
                }
            }
        }

        private Task<int> CalculateTotalCountAsync()
        {
            if (Player is not null)
            {
                Player.TotalUnits = 0;
                foreach (Unit unit in Player.StartUnits)
                {
                    Player.TotalUnits += unit.Quantity;
                }
            }
            return Task.FromResult(0);
        }

        private async Task CalculateArmyDataAsync(Player player)
        {
            await CalculateForceAsync(player);
            await CalculateTotalForceAsync(player);
            await CountTotalUnitsAsync(player);
        }

        private Task CountTotalUnitsAsync(Player player)
        {
            int count = 0;

            foreach (Unit unit in player.StartUnits)
            {
                count += unit.Quantity;
            }

            player.TotalUnits = count;
            return Task.CompletedTask;
        }

        private Task ClearUnitsAsync()
        {
            if (Player is not null)
            {
                Player.StartUnits.Clear();
                Player.UsedForce = 0;
                Player.TotalUnits = 0;
            }

            return Task.CompletedTask;
        }

        private async Task ClearUnitAsync(Unit unit)
        {
            if (Player is not null)
            {
                unit.Quantity = 0;
                Player.StartUnits.Remove(unit);

                await CalculateTotalCountAsync();
                await CalculateTotalForceAsync(Player);
            }
        }

        private async Task IncrementAsync(Unit unit)
        {
            if (await CheckTroopSize(unit) && await CheckAllowedUnitsOfClass(unit) && Game?.Force > Player?.UsedForce && Player is not null)
            {
                if (unit.Quantity < _loggedInUser?.Units?.FirstOrDefault(x => x.UnitId == unit.UnitId)?.Quantity)
                {
                    unit.Quantity++;
                }

                int quantity = Player.StartUnits?.FirstOrDefault(x => x.UnitId == unit.UnitId)?.Quantity ?? 0;
                unit.Quantity = quantity;

                await CalculateTotalCountAsync();
                await CalculateTotalForceAsync(Player);
                await CalculateForceAsync(Player);
            }
        }

        private async Task DecrementAsync(Unit unit)
        {
            if (await CheckTroopSize(unit) && Player is not null)
            {
                if (unit.Quantity > 0)
                {
                    unit.Quantity--;
                }

                int quantity = Player.StartUnits?.FirstOrDefault(x => x.UnitId == unit.UnitId)?.Quantity ?? 0;
                unit.Quantity = quantity;

                await CalculateTotalCountAsync();
                await CalculateTotalForceAsync(Player);
                await CalculateForceAsync(Player);
            }
        }

        private Task<bool> CheckTroopSize(Unit unit)
        {
            if (Player is not null)
            {
                foreach (var item in Player.StartUnits.Where(x => x.ClassId == unit.ClassId))
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
            if (Player is not null && Game is not null)
            {
                if (unit.ClassId == 1)
                {
                    return Task.FromResult(true);
                }

                int maxOfClass = (Game.Force / 200);
                int numberOfTroops;

                foreach (var item in Player.StartUnits.Where(x => x.ClassId == unit.ClassId))
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

                if (maxOfClass >= 0)
                {
                    return Task.FromResult(true);
                }
            }

            return Task.FromResult(false);
        }

        private Task NavigateBack()
        {
            navigationManager.NavigateTo($"/Account/Games");

            return Task.CompletedTask;
        }
    }
}