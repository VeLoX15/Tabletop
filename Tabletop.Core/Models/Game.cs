using DbController;

namespace Tabletop.Core.Models
{
    public class Game : IDbModel
    {
        [CompareField("game_id")]
        public int GameId { get; set; }
        [CompareField("gamemode_id")]
        public int GamemodeId { get; set; }
        [CompareField("name")]
        public string Name { get; set; } = string.Empty;
        [CompareField("rounds")]
        public int Rounds { get; set; }
        [CompareField("force")]
        public int Force { get; set; }
        [CompareField("date")]
        public DateTime Date { get; set; }

        public List<Player> Players { get; set; } = new();


        public int Id => GameId;

        public Dictionary<string, object?> GetParameters()
        {
            return new Dictionary<string, object?>
            {
                { "GAME_ID", GameId },
                { "GAMEMODE_ID", GamemodeId },
                { "NAME", Name },
                { "ROUNDS", Rounds },
                { "FORCE", Force },
                { "DATE", Date }
            };
        }
    }
}