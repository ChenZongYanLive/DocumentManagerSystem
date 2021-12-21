using System.Collections.Generic;
using System.Threading;
using DocumentLibrary.MySql.Models;
using System.Threading.Tasks;

namespace DocumentLibrary.Application.DBHelper
{
    public interface IDocumentDbHelper
    {
        string FieldDocumentName { get; }
        string FieldDocumentType { get; }
        string FieldDocumentNumber { get; }
        string FieldDocumentOwner { get; }
        string FieldEffectiveDate { get; }
        
        string FieldFileName { get; }
        string FieldFileType { get; }
        string FieldFileContent { get; }
        
        string FieldIsPublish { get; }
        string FieldPublishDate { get; }
        string FieldCreateBy { get; }
        string FieldCreateTime { get; }
        string FieldUpdateBy { get; }
        string FieldUpdateTime { get; }
        string FieldDocumentVersion { get;}
        string FieldIsInvalid { get; }
        string FieldIsDelete { get; }
        Task AddAsync(Document document, CancellationToken cancellationToken);
        void Add(Document document);
        Task SetAsync(int id, Dictionary<string, object> updateDic, CancellationToken cancellationToken);
        Task<Document> GetDocumentById(int id, CancellationToken cancellationToken);
    }
}