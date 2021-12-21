using System;

namespace DocumentLibrary.MySql.Models
{
    public partial class DocumentVersionHistory
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public bool IsDelete { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Remark { get; set; }
        public int DocumentType { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime PublishDate { get; set; }
        public byte[] FileContent { get; set; }
        public string DocumentNumber { get; set; }
        public bool IsPublish { get; set; }
        public string DocumentStartNumber { get; set; }
        public string DocumentEndNumber { get; set; }
        public string DocumentYearMonth { get; set; }
        public int DocumentCount { get; set; }
        public int DocumentPageCount { get; set; }
        public string DocumentOwner { get; set; }
        public string DocumentVersion { get; set; }
        public bool IsInvalid { get; set; }
    }
}
