using DbController;

namespace Tabletop.Core.Models
{
    public class Round
    {
        [CompareField("round")]
        public int CurrentRound { get; set; }
        [CompareField("game_id")]
        public int GameId { get; set; }
        [CompareField("player_id")]
        public int PlayerId { get; set; }

        List<Casualty> Casualties { get; set; } = [];
        List<Zone> Zones { get; set; } = [];
 
        public Dictionary<string, object?> GetParameters()
        {
            return new Dictionary<string, object?>
            {
                { "ROUND", CurrentRound },
                { "GAME_ID", GameId },
                { "PLAYER_ID", PlayerId }
            };
        }
    }
}