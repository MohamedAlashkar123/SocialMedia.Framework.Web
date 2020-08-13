using SocialMedia.Framework.Core.Login;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Framework.ViewModel
{
    public class FriendshipViewModel
    {
        public int Id { get; set; }

        public DateTime? timestamp { get; set; }

        public virtual User User1 { get; set; }

        public virtual User User2 { get; set; }
    }
}
