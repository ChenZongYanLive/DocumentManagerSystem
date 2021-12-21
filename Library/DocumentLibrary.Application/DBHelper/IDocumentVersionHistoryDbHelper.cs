using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DocumentLibrary.MySql.Models;

namespace DocumentLibrary.Application.DBHelper
{
    public interface IDocumentVersionHistoryDbHelper
    {
        Task AddAsync(DocumentVersionHistory document, CancellationToken cancellationToken);
        Task<DocumentVersionHistory> GetDocumentVersionHistoryByFilter(Expression<Func<DocumentVersionHistory, bool>> filter, CancellationToken cancellationToken);
        string FieldUpdateBy { get; }
        string FieldUpdateTime { get; }
        string FieldIsDelete { get; }
        Task SetAsync(int id, Dictionary<string, object> updateDic, CancellationToken cancellationToken);
    }
}