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
        public Unit? SelectedUnit { get; set; }
        public Player? SelectedPlayer { get; set; }
        public List<Player> Friends { get; set; } = new(); //Friends of Game Host
        public List<Player> SelectedPlayers { get; set; } = new(); //Players of Game
        public List<Template> Templates { get; set; } = new();

        protected override async Task OnParametersSetAsync()
        {
            _loggedInUser = await authService.GetUserAsync();

            using IDbController dbController = new MySqlController(AppdataService.ConnectionString);
            Game = await gameService.GetAsync(GameId, dbController);
            if (_loggedInUser != null && Game is not null)
            {
                _loggedInUser.Units = await unitService.GetUserUnitsAsync(_loggedInUser.UserId, dbController);
                Templates = await templateService.GetTemplateOnForceAsync(_loggedInUser.UserId, Game.Force, dbController);
            }

            await FriendReloading();
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
            }

            Player = null;
        }

        private Task AddUserAsync()
        {
            if (Game is not null)
            {
                if (SelectedPlayer is not null)
                {
                    SelectedPlayer.GameId = Game.GameId;
                    SelectedPlayer.UserId = SelectedPlayer.User.UserId;
                    SelectedPlayer.Team = SelectedTeam;

                    SelectedPlayers.Add(SelectedPlayer);
                }
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

        private Task AddUnitAsync()
        {
            if (Player is not null)
            {
                if (SelectedUnit is not null)
                {
                    Player.StartUnits.Add(SelectedUnit);
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

        private int CalculateTotalForce(List<Unit> list)
        {
            int totalForce = 0;
            if (list is not null)
            {
                foreach (var unit in list)
                {
                    unit.PrimaryWeapon = AppdataService.Weapons.FirstOrDefault(x => x.WeaponId == unit.PrimaryWeaponId);
                    unit.SecondaryWeapon = AppdataService.Weapons.FirstOrDefault(x => x.WeaponId == unit.SecondaryWeaponId);
                    totalForce += Calculation.Force(unit) * unit.Quantity;
                }
            }

            return totalForce;
        }

        private int CalculateForce(Unit unit)
        {
            unit.PrimaryWeapon = AppdataService.Weapons.FirstOrDefault(x => x.WeaponId == unit.PrimaryWeaponId);
            unit.SecondaryWeapon = AppdataService.Weapons.FirstOrDefault(x => x.WeaponId == unit.SecondaryWeaponId);
            int force = Calculation.Force(unit);

            return force;
        }

        private bool ReadyToReveal()
        {
            if(Game is not null)
            {
                int count = 0;
                foreach(var player in Game.Players)
                {
                    if(player.StartUnits.Any())
                    {
                        count++;
                    }
                }

                if(Game.Players.Count == count)
                {
                    return true;
                }
            }
            return false;
        }
    }
}