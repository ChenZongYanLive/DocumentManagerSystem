using System;

namespace DocumentLibrary.MySql.Models
{
    public partial class DocumentHistory
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateTime { get; set; }
        public string Remark { get; set; }
        public int DocumentType { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime PublishDate { get; set; }
        public string FileContent { get; set; }
    }
}
