using DocumentLibrary.Application.DBHelper;
using DocumentLibrary.MySql.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DocumentLibrary.Application.Interface;
using DocumentLibrary.Data.Dto;

namespace DocumentLibrary.Application.Helper
{
    /// <summary>
    /// 處理1~3階文件的商務邏輯
    /// </summary>
    public class StageDocumentHelper
    {
        private readonly IDocumentDbHelper _documentDbHelper;
        private readonly IDocumentVersionHistoryDbHelper _documentVersionHistory;
        private readonly IFileHelper _fileHelper;
        private readonly IMapper _mapper;

        public StageDocumentHelper(IDocumentDbHelper documentDbHelper,IDocumentVersionHistoryDbHelper documentVersionHistory, IFileHelper fileHelper, IMapper mapper)
        {
            _documentDbHelper = documentDbHelper;
            _documentVersionHistory = documentVersionHistory;
            _fileHelper = fileHelper;
            _mapper = mapper;
        }

        /// <summary>
        /// 新增文件
        /// </summary>
        /// <param name="document"></param>
        /// <param name="cancellationToken"></param>
        public async Task AddDocumentAsync(Document document, CancellationToken cancellationToken)
        {
            await _documentDbHelper.AddAsync(document, cancellationToken);
        }
        
        /// <summary>
        /// 修改文件
        /// </summary>
        /// <param name="document"></param>
        /// <param name="cancellationToken"></param>
        public async Task SetDocumentAsync(Document document, CancellationToken cancellationToken)
        {
            var updateDic = new Dictionary<string, object>
            {
                {_documentDbHelper.FieldDocumentName, document.DocumentName},
                {_documentDbHelper.FieldDocumentNumber, document.DocumentNumber},
                {_documentDbHelper.FieldDocumentOwner, document.DocumentOwner},
                {_documentDbHelper.FieldDocumentType, document.DocumentType},
                {_documentDbHelper.FieldEffectiveDate, document.EffectiveDate},
                
                {_documentDbHelper.FieldUpdateBy, document.UpdateBy},
                {_documentDbHelper.FieldUpdateTime, document.UpdateTime},
            };
            await _documentDbHelper.SetAsync(document.Id, updateDic, cancellationToken);
        }
        
        /// <summary>
        /// 修改檔案
        /// </summary>
        /// <param name="document"></param>
        /// <param name="cancellationToken"></param>
        public async Task SetDocumentFileAsync(Document document, CancellationToken cancellationToken)
        {
            var updateDic = new Dictionary<string, object>
            {
                {_documentDbHelper.FieldFileName, document.FileName},
                {_documentDbHelper.FieldFileType, document.FileType},
                {_documentDbHelper.FieldFileContent, document.FileContent},
            };
            await _documentDbHelper.SetAsync(document.Id, updateDic, cancellationToken);
        }

        /// <summary>
        /// 發布文件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        public async Task PublishDocumentAsync(int id, CancellationToken cancellationToken)
        {
            var updateDic = new Dictionary<string, object>
            {
                {_documentDbHelper.FieldIsPublish, true},
                {_documentDbHelper.FieldPublishDate, DateTime.Now},
                {_documentDbHelper.FieldUpdateBy, "Admin"},
                {_documentDbHelper.FieldUpdateTime, DateTime.Now},
            };
            await _documentDbHelper.SetAsync(id, updateDic, cancellationToken);
        }
        
        /// <summary>
        /// 取消發布文件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        public async Task UnPublishDocumentAsync(int id, CancellationToken cancellationToken)
        {
            var updateDic = new Dictionary<string, object>
            {
                {_documentDbHelper.FieldIsPublish, false},
                {_documentDbHelper.FieldPublishDate, DateTime.Now},
                {_documentDbHelper.FieldUpdateBy, "Admin"},
                {_documentDbHelper.FieldUpdateTime, DateTime.Now},
            };
            await _documentDbHelper.SetAsync(id, updateDic, cancellationToken);
        }

        /// <summary>
        /// 文件更版
        /// </summary>
        /// <param name="changeDocumentVersionDto"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="NullReferenceException"></exception>
        public async Task ChangeDocumentVersionAsync(ChangeDocumentVersionDto changeDocumentVersionDto, CancellationToken cancellationToken)
        {
            var oldDocument = await _documentDbHelper.GetDocumentById(changeDocumentVersionDto.Id, cancellationToken);

            if (oldDocument != null)
            {
                #region == 將舊版本移至DocumentVersionHistory ==

                var documentVersionHistory = _mapper.Map<DocumentVersionHistory>(oldDocument);
                documentVersionHistory.UpdateTime = DateTime.Now;
                documentVersionHistory.UpdateBy = "Admin";
                documentVersionHistory.IsPublish = false;
                documentVersionHistory.IsDelete = false;

                await _documentVersionHistory.AddAsync(documentVersionHistory, cancellationToken);

                #endregion

                #region == 處理檔案 & 更新文件資訊==

                var file = changeDocumentVersionDto.File;

                if (_fileHelper.VerifyFile(file))
                {
                    var fileName = file.Name;
                    var fileType = _fileHelper.GetFileType(file);
                    var base64ByFile = await _fileHelper.ConvertFileToBase64Async(file, cancellationToken);
                    var compressionBase64ToByte = await _fileHelper.ZipAsync(base64ByFile, cancellationToken);
                
                    var document = _mapper.Map<Document>(changeDocumentVersionDto);
                    document.FileName = fileName;
                    document.FileType = fileType;
                    document.FileContent = compressionBase64ToByte;
                    document.IsPublish = false;
                    document.UpdateBy = "Admin";
                    document.UpdateTime = DateTime.Now;

                    var updateDic = new Dictionary<string, object>
                    {
                        {_documentDbHelper.FieldDocumentName, document.DocumentName},
                        {_documentDbHelper.FieldDocumentNumber, document.DocumentNumber},
                        {_documentDbHelper.FieldDocumentOwner, document.DocumentOwner},
                        {_documentDbHelper.FieldDocumentType, document.DocumentType},
                        {_documentDbHelper.FieldDocumentVersion, document.DocumentType},
                        {_documentDbHelper.FieldEffectiveDate, document.EffectiveDate},
                        {_documentDbHelper.FieldIsPublish, document.IsPublish},
                
                        {_documentDbHelper.FieldUpdateBy, document.UpdateBy},
                        {_documentDbHelper.FieldUpdateTime, document.UpdateTime},
                    };
                    await _documentDbHelper.SetAsync(document.Id, updateDic, cancellationToken);
                }
                
                #endregion
            }
            else
            {
                throw new NullReferenceException($"無法更版，因找不到該文件，文件ID為:{changeDocumentVersionDto.Id}");
            }
        }

        /// <summary>
        /// 作廢文件
        /// 作廢日期等於生效日
        /// TODO 生效日不等於有效日
        /// </summary>
        public async Task InvalidDocumentAsync(InvalidDocumentDto invalidDocumentDto, CancellationToken cancellationToken)
        {
            var oldDocument = await _documentDbHelper.GetDocumentById(invalidDocumentDto.Id, cancellationToken);

            if (oldDocument != null)
            {
                #region == 將舊版本移至DocumentVersionHistory ==
                
                var documentVersionHistory = _mapper.Map<DocumentVersionHistory>(oldDocument);
                documentVersionHistory.IsPublish = false;
                documentVersionHistory.IsDelete = false;
                documentVersionHistory.UpdateBy = "Admin";
                documentVersionHistory.UpdateTime = DateTime.Now;
                
                await _documentVersionHistory.AddAsync(documentVersionHistory, cancellationToken);
                
                #endregion

                #region == 更新文件資訊 ==

                var updateDic = new Dictionary<string, object>
                {
                    {_documentDbHelper.FieldIsInvalid, true},
                    {_documentDbHelper.FieldIsPublish, false},
                    {_documentDbHelper.FieldEffectiveDate, oldDocument.EffectiveDate},
                    {_documentDbHelper.FieldIsPublish, false},
                
                    {_documentDbHelper.FieldUpdateBy, "Admin"},
                    {_documentDbHelper.FieldUpdateTime, DateTime.Now},
                };
                await _documentDbHelper.SetAsync(oldDocument.Id, updateDic, cancellationToken);

                #endregion
            }
        }

        /// <summary>
        /// 取消作廢文件
        /// </summary>
        /// <param name="cancelVoidDocumentDto"></param>
        /// <param name="cancellationToken"></param>
        public async Task CancelVoidDocumentAsync(CancelVoidDocumentDto cancelVoidDocumentDto, CancellationToken cancellationToken)
        {
            var document = _mapper.Map<Document>(cancelVoidDocumentDto);
            
            var documentVersionHistory = await _documentVersionHistory
                .GetDocumentVersionHistoryByFilter(x =>
                    x.DocumentId == document.Id &&
                    x.DocumentVersion == document.DocumentVersion &&
                    !x.IsDelete, cancellationToken);
            
            if (documentVersionHistory != null)
            {
                document.EffectiveDate = documentVersionHistory.EffectiveDate;
                
                var updateDocumentVersionHistoryDic = new Dictionary<string, object>
                {
                    {_documentVersionHistory.FieldIsDelete, true},
                    {_documentVersionHistory.FieldUpdateBy, "Admin"},
                    {_documentVersionHistory.FieldUpdateTime, DateTime.Now}
                };
                await _documentVersionHistory.SetAsync(documentVersionHistory.Id, updateDocumentVersionHistoryDic, cancellationToken);
            }
            
            var updateDocumentDic = new Dictionary<string, object>
            {
                {_documentDbHelper.FieldEffectiveDate, document.EffectiveDate},
                {_documentDbHelper.FieldIsInvalid, false},
                {_documentDbHelper.FieldUpdateBy, "Admin"},
                {_documentDbHelper.FieldUpdateTime, DateTime.Now},
            };
            await _documentDbHelper.SetAsync(document.Id, updateDocumentDic, cancellationToken);
        }

        /// <summary>
        /// 刪除文件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        public async Task DeleteDocumentAsync(int id, CancellationToken cancellationToken)
        {
            var updateDic = new Dictionary<string, object>
            {
                {_documentDbHelper.FieldIsDelete, true}
            };
            await _documentDbHelper.SetAsync(id, updateDic, cancellationToken);
        }
    }
}