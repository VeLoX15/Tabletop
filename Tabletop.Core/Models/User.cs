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
        [CompareField("image")]
        public byte[] Image { get; set; } = new byte[0];
        [CompareField("password")]
        public string Password { get; set; } = string.Empty;
        [CompareField("salt")]
        public string Salt { get; set; } = string.Empty;
        [CompareField("last_login")]
        public DateTime LastLogin { get; set; }



        public List<Permission> Permissions { get; set; } = new();
        public int Id => UserId;

        public Dictionary<string, object?> GetParameters()
        {
            return new Dictionary<string, object?>
            {
                { "USER_ID", UserId },
                { "USERNAME", Username },
                { "DISPLAY_NAME", DisplayName },
                { "IMAGE", Image },
                { "PASSWORD", Password },
                { "SALT", Salt },
                { "LAST_LOGIN", LastLogin }
            };
        }
    }
}
