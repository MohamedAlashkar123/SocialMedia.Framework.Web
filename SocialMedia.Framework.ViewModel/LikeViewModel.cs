using SocialMedia.Framework.Core;
using SocialMedia.Framework.Core.Login;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Framework.ViewModel
{
    public class LikeViewModel
    {
        public int Id { get; set; }

        public virtual Post Post { get; set; }

        public virtual User User { get; set; }
    }
}
