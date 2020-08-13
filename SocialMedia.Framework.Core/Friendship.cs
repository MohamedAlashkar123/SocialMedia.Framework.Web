using SocialMedia.Framework.Core.Login;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Framework.Core
{
    public partial class Friendship
    {
        public int Id { get; set; }

        public DateTime? timestamp { get; set; }

        public virtual User User1 { get; set; }

        public virtual User User2 { get; set; }
    }
}
