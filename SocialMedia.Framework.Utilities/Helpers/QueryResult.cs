using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Framework.Utilities.Helpers
{
    public class QueryResult<T>
    {
        public int TotalItems { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
