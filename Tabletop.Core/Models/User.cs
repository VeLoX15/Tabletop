using DbController;

namespace Tabletop.Core.Models
{
    public sealed class User : IDbModel
    {
        [CompareField("user_id")]
        public int UserId { get; set; }
        [CompareField("username")]
        public string Username { get; set; } = string.Empty;
        [CompareField("display_name")]
        public string DisplayName { get; set; } = string.Empty;
        [CompareField("description")]
        public string Description { get; set; } = string.Empty;
        [CompareField("main_fraction_id")]
        public int MainFractionId { get; set; }
        [CompareField("password")]
        public string Password { get; set; } = string.Empty;
        [CompareField("salt")]
        public string Salt { get; set; } = string.Empty;
        [CompareField("last_login")]
        public DateTime LastLogin { get; set; }
        [CompareField("image")]
        public byte[]? Image { get; set; }

        public string ConvertedImage { get; set; } = string.Empty;


        public List<Permission> Permissions { get; set; } = new();
        public List<Fraction> Fractions { get; set; } = new();
        public List<Unit> Units { get; set; } = new();
        public int Id => UserId;

        public Dictionary<string, object?> GetParameters()
        {
            return new Dictionary<string, object?>
            {
                { "USER_ID", UserId },
                { "USERNAME", Username },
                { "DISPLAY_NAME", DisplayName },
                { "DESCRIPTION", Description },
                { "MAIN_FRACTION_ID", MainFractionId },
                { "PASSWORD", Password },
                { "SALT", Salt },
                { "LAST_LOGIN", LastLogin },
                { "IMAGE", Image }
            };
        }
    }
}
