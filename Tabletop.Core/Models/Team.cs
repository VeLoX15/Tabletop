using DbController;

namespace Tabletop.Core.Models
{
    public class Team
    {
        [CompareField("team_id")]
        public int TeamId { get; set; }
        [CompareField("game_id")]
        public int GameId { get; set; }
        [CompareField("name")]
        public string Name { get; set; } = string.Empty;
        [CompareField("points")]
        public int Points { get; set; }

        public Dictionary<string, object?> GetParameters()
        {
            return new Dictionary<string, object?>
            {
                { "TEAM_ID", TeamId },
                { "GAME_ID", GameId },
                { "NAME", Name },
                { "POINTS", Points}
            };
        }
    }
}