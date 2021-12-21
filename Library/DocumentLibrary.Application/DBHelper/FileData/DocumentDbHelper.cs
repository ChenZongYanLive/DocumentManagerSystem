using DocumentLibrary.MySql.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentLibrary.Application.DBHelper.FileData
{
    public class DocumentDbHelper: IDocumentDbHelper
    {        
        public DocumentDbHelper()
        {            
        }

        public string FieldDocumentName { get; }
        public string FieldDocumentType { get; }
        public string FieldDocumentNumber { get; }
        public string FieldDocumentOwner { get; }
        public string FieldEffectiveDate { get; }
        public string FieldFileName { get; }
        public string FieldFileType { get; }
        public string FieldFileContent { get; }
        public string FieldIsPublish { get; }
        public string FieldPublishDate { get; }
        public string FieldCreateBy { get; }
        public string FieldCreateTime { get; }
        public string FieldUpdateBy { get; }
        public string FieldUpdateTime { get; }
        public string FieldDocumentVersion { get; }
        public string FieldIsInvalid { get; }
        public string FieldIsDelete { get; set; }

        public async Task AddAsync(Document document, CancellationToken cancellationToken)
        {            
            try
            {
                if(string.IsNullOrEmpty(document.DocumentName))
                {
                    throw new NullReferenceException("未填文件名稱");
                }

                if (string.IsNullOrEmpty(document.CreateBy))
                {
                    throw new NullReferenceException("未填建立者");
                }

                document.Id+=1;              

                const string fileName = "Document.json";

                await using var createStream = File.Create(fileName);
                await JsonSerializer.SerializeAsync(createStream, document, cancellationToken: cancellationToken);
                await createStream.DisposeAsync();
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        
        public void Add(Document document)
        {
            throw new NotImplementedException();
        }

        public Task SetAsync(int id, Dictionary<string, object> updateDic, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Document> GetDocumentById(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}