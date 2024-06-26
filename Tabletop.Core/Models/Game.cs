﻿using DbController;

namespace Tabletop.Core.Models
{
    public class Game : IDbModel
    {
        [CompareField("game_id")]
        public int GameId { get; set; }
        [CompareField("gamemode_id")]
        public int GamemodeId { get; set; }
        [CompareField("user_id")]
        public int UserId { get; set; }
        [CompareField("name")]
        public string Name { get; set; } = string.Empty;
        [CompareField("rounds")]
        public int? Rounds { get; set; }
        [CompareField("force")]
        public int Force { get; set; }
        [CompareField("number_of_teams")]
        public int NumberOfTeams { get; set; }
        [CompareField("number_of_players")]
        public int NumberOfPlayers { get; set; }
        [CompareField("date")]
        public DateTime Date { get; set; } = DateTime.Now;

        public User Host { get; set; } = new();
        public List<Player> Players { get; set; } = new();

        public int Id => GameId;

        public Dictionary<string, object?> GetParameters()
        {
            return new Dictionary<string, object?>
            {
                { "GAME_ID", GameId },
                { "GAMEMODE_ID", GamemodeId },
                { "USER_ID", UserId },
                { "NAME", Name },
                { "ROUNDS", Rounds },
                { "FORCE", Force },
                { "NUMBER_OF_TEAMS", NumberOfTeams },
                { "NUMBER_OF_PLAYERS", NumberOfPlayers },
                { "DATE", Date }
            };
        }
    }
}