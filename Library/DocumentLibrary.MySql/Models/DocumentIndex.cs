using System;

namespace DocumentLibrary.MySql.Models
{
    public partial class DocumentIndex
    {
        public int Id { get; set; }
        public string DocumentName { get; set; }
        public string DocumentNumber { get; set; }
        public bool IsDelete { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool IsInvalid { get; set; }
        public int DocumentId { get; set; }
        public string DocumentOwner { get; set; }
    }
}
