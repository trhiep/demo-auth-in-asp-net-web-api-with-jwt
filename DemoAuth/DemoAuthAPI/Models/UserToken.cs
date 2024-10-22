using System;
using System.Collections.Generic;

namespace DemoAuthAPI.Models
{
    public partial class UserToken
    {
        public int UserTokenId { get; set; }
        public int UserId { get; set; }
        public string RefreshToken { get; set; } = null!;
        public DateTime ExpiredAt { get; set; }

        public virtual UserInfo User { get; set; } = null!;
    }
}
