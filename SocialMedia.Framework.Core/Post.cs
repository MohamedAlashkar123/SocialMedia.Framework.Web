using SocialMedia.Framework.Core.Login;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SocialMedia.Framework.Core
{
    public partial class Post
    {
        public int Id { get; set; }

        [StringLength(600)]
        public string content { get; set; }

        public DateTime? timestamp { get; set; }

        public virtual ICollection<Deslike> Deslikes { get; set; }

        public virtual ICollection<Like> Likes { get; set; }

        public virtual User User { get; set; }
    }
}
