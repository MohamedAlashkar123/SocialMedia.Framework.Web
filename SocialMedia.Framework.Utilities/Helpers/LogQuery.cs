using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Framework.Utilities.Helpers
{
    public interface IQueryObject
    {
        string SortBy { get; set; }
        bool IsSortAscending { get; set; }
        int Page { get; set; }
        byte PageSize { get; set; }
    }
    public class LogQuery : IQueryObject
    {
        public string Level { get; set; }
        public string User { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SortBy { get; set; }
        public bool IsSortAscending { get; set; }
        public int Page { get; set; }
        public byte PageSize { get; set; }
    }


}
