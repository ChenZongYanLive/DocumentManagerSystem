using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DocumentLibrary.MySql;
using DocumentLibrary.MySql.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentLibrary.Application.DBHelper.MySql
{
    public class DocumentVersionHistoryDbHelper : IDocumentVersionHistoryDbHelper
    {
        private readonly DocumentDbContext _documentDbContext;

        private DocumentVersionHistoryDbHelper(DocumentDbContext documentDbContext)
        {
            _documentDbContext = documentDbContext;
        }
        
        public async Task AddAsync(DocumentVersionHistory document, CancellationToken cancellationToken)
        {
            try
            {
                await _documentDbContext.Documentversionhistory.AddAsync(document, cancellationToken);
                await _documentDbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task SetAsync(int id, Dictionary<string, object> updateDic, CancellationToken cancellationToken)
        {
            try
            {
                var document = await _documentDbContext.Documentversionhistory.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

                if (document != null)
                {
                    foreach (var (key, value) in updateDic)
                    {
                        switch (key)
                        {
                            case _fieldIsDelete:
                                document.IsDelete = (bool) value;
                                break;
                            case _fieldUpdateBy:
                                document.UpdateBy = (string) value;
                                break;
                            case _fieldUpdateTime:
                                document.UpdateTime = (DateTime) value;
                                break;
                        }
                    }

                    await _documentDbContext.SaveChangesAsync(cancellationToken);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private const string _fieldUpdateTime = "UpdateTime";

        private const string _fieldUpdateBy = "UpdateBy";

        private const string _fieldIsDelete = "IsDelete";
        
        public string FieldUpdateBy => _fieldUpdateBy;
        public string FieldUpdateTime => _fieldUpdateTime;
        public string FieldIsDelete => _fieldIsDelete;

        public async Task<DocumentVersionHistory> GetDocumentVersionHistoryByFilter(Expression<Func<DocumentVersionHistory, bool>> filter, CancellationToken cancellationToken)
        {
            try
            {
                return await _documentDbContext.Documentversionhistory.FirstOrDefaultAsync(filter, cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }        
        
    }
}