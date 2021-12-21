﻿using System;
using Microsoft.AspNetCore.Http;

namespace DocumentLibrary.Data.Dto
{
    public class AddDocumentFileDto
    {
        public string DocumentName { get; set; }
        public int DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentOwner { get; set; }
        public DateTime EffectiveDate { get; set; }
        
        public IFormFile File { get; set; }

        public string CreateBy { get; set; }
        public DateTime CreateTime { get; set; }
        
        public string UpdateBy { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}