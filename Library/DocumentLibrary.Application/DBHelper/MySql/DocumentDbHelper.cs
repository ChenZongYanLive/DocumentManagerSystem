using System;
using System.Collections.Generic;
using DocumentLibrary.MySql;
using System.Threading;
using System.Threading.Tasks;
using DocumentLibrary.MySql.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentLibrary.Application.DBHelper.MySql
{
    public class DocumentDbHelper: IDocumentDbHelper
    {
        private readonly DocumentDbContext _documentDbContext;

        private const string _fieldDocumentName = "DocumentName";
        private const string _fieldDocumentType = "DocumentType";
        private const string _fieldDocumentNumber = "DocumentNumber";
        private const string _fieldDocumentOwner = "DocumentOwner";
        private const string _fieldDocumentVersion = "DocumentVersion";
        
        private const string _fieldEffectiveDate = "EffectiveDate";
        
        private const string _fieldFileName = "FileName";
        private const string _fieldFileType = "FileType";
        private const string _fieldFileContent = "FileContent";
        
        
        private const string _fieldIsInvalid = "IsInvalid";
        
        private const string _fieldIsDelete = "IsDelete";
        
        private const string _fieldIsPublish = "IsPublish";
        private const string _fieldPublishDate = "PublishDate";
        
        private const string _fieldCreateBy = "CreateBy";
        private const string _fieldCreateTime = "CreateTime";
        
        private const string _fieldUpdateBy = "UpdateBy";
        private const string _fieldUpdateTime = "UpdateTime";

        public DocumentDbHelper(DocumentDbContext documentDbContext)
        {
            this._documentDbContext = documentDbContext;
        }

        public string FieldDocumentName  => _fieldDocumentName;
        public string FieldDocumentType => _fieldDocumentType;
        public string FieldDocumentNumber  => _fieldDocumentNumber;
        public string FieldDocumentOwner  => _fieldDocumentOwner;
        public string FieldEffectiveDate  => _fieldEffectiveDate;
        public string FieldFileName => _fieldFileName;
        public string FieldFileType  => _fieldFileType;
        public string FieldFileContent  => _fieldFileContent;
        public string FieldIsPublish => _fieldIsPublish;
        public string FieldPublishDate => _fieldPublishDate;
        
        public string FieldCreateBy => _fieldCreateBy;
        
        public string FieldCreateTime => _fieldCreateTime;
        public string FieldUpdateBy => _fieldUpdateBy;
        public string FieldUpdateTime => _fieldUpdateTime;
        public string FieldDocumentVersion => _fieldDocumentVersion;
        public string FieldIsInvalid => _fieldIsInvalid;
        public string FieldIsDelete => _fieldIsDelete;

        public async Task AddAsync(Document document, CancellationToken cancellationToken)
        {
            try
            {
                await _documentDbContext.Document.AddAsync(document, cancellationToken);
                await _documentDbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void Add(Document document)
        {
            try
            {
                _documentDbContext.Document.Add(document);
                _documentDbContext.SaveChanges();
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
                var document = await _documentDbContext.Document.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

                if (document != null)
                {
                    foreach (var (key, value) in updateDic)
                    {
                        switch (key)
                        {
                            case _fieldDocumentName:
                                document.DocumentName = (string) value;
                                break;
                            case _fieldDocumentNumber:
                                document.DocumentNumber = (string) value;
                                break;
                            case _fieldDocumentType:
                                document.DocumentType = (int) value;
                                break;
                            case _fieldDocumentOwner:
                                document.DocumentOwner = (string) value;
                                break;
                            case _fieldDocumentVersion:
                                document.DocumentVersion = (string) value;
                                break;
                            case _fieldEffectiveDate:
                                document.EffectiveDate = (DateTime) value;
                                break;
                            case _fieldFileName:
                                document.FileName = (string) value;
                                break;
                            case _fieldFileType:
                                document.FileType = (string) value;
                                break;
                            case _fieldFileContent:
                                document.FileContent = (byte[]) value;
                                break;
                            case _fieldIsPublish:
                                document.IsPublish = (bool) value;
                                break;
                            case _fieldPublishDate:
                                document.PublishDate = (DateTime) value;
                                break;
                            case _fieldCreateBy:
                                document.CreateBy = (string) value;
                                break;
                            case _fieldCreateTime:
                                document.CreateTime = (DateTime) value;
                                break;
                            case _fieldUpdateBy:
                                document.UpdateBy = (string) value;
                                break;
                            case _fieldUpdateTime:
                                document.UpdateTime = (DateTime) value;
                                break; 
                            case _fieldIsInvalid:
                                document.IsInvalid = (bool) value;
                                break;
                            case _fieldIsDelete:
                                document.IsDelete = (bool) value;
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

        public async Task<Document> GetDocumentById(int id, CancellationToken cancellationToken)
        {
            try
            {
                return await _documentDbContext.Document.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
