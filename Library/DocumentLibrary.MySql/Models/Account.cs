using Microsoft.AspNetCore.Identity;
using System;

namespace DocumentLibrary.MySql.Models
{
    public class Account : IdentityUser<Guid>
    {
        public string Title { get; set; }
        public bool IsDelete { get; set; }
        public long ActiveTime { get; set; }
        public long ExpiredTime { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
