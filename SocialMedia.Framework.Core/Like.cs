using SocialMedia.Framework.Core.Login;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Framework.Core
{
    public partial class Like
    {
        public int Id { get; set; }

        public virtual Post Post { get; set; }

        public virtual User User { get; set; }
    }
}
