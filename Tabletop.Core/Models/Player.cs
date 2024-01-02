using DbController;

namespace Tabletop.Core.Models
{
    public class Player : IDbModel
    {
        [CompareField("player_id")]
        public int PlayerId { get; set; }
        [CompareField("user_id")]
        public int UserId { get; set; }
        [CompareField("game_id")]
        public int GameId { get; set; }
        [CompareField("fraction_id")]
        public int FractionId { get; set; }
        [CompareField("team")]
        public int Team { get; set; }
        [CompareField("used_force")]
        public int UsedForce { get; set; }

        public int AllowedForce {  get; set; }
        public User User { get; set; } = new();
        public List<Unit> StartUnits { get; set; } = new();
        public int TotalUnits { get; set; }

        public int Id => PlayerId;

        public Dictionary<string, object?> GetParameters()
        {
            return new Dictionary<string, object?>
            {
                { "PLAYER_ID", PlayerId },
                { "USER_ID", UserId },
                { "GAME_ID", GameId },
                { "FRACTION_ID", FractionId },
                { "TEAM", Team },
                { "USED_FORCE", UsedForce }
            };
        }
    }
}