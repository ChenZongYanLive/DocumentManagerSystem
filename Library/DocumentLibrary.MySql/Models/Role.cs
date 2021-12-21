using Microsoft.AspNetCore.Identity;
using System;

namespace DocumentLibrary.MySql.Models
{
    public class Role : IdentityRole<Guid>
    {
        public ushort Level { get; set; }
        public bool IsDelete { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
