using System;
using System.Collections.Generic;

namespace DemoAuthAPI.Models
{
    public partial class UserInfo
    {
        public UserInfo()
        {
            UserTokens = new HashSet<UserToken>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? DisplayName { get; set; } = null!;
        public string? Role { get; set; } = null!;

        public virtual ICollection<UserToken> UserTokens { get; set; }
    }
}
