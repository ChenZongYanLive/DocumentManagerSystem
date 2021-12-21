using System;

namespace DocumentLibrary.MySql.Models
{
    public partial class AccountRule
    {
        public uint Id { get; set; }
        public string Account { get; set; }
        public string Rule { get; set; }
        public bool IsDelete { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
