using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DocumentLibrary.Application.Helper;
using DocumentLibrary.Application.Interface;
using DocumentLibrary.Data.Dto;
using DocumentLibrary.MySql.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagerSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly StageDocumentHelper _stageDocumentHelper;
        private readonly IFileHelper _fileHelper;
        private readonly IMapper _mapper;

        public DocumentController(StageDocumentHelper stageDocumentDbHelper, IFileHelper fileHelper, IMapper mapper)
        {
            _stageDocumentHelper = stageDocumentDbHelper;
            _fileHelper = fileHelper;
            _mapper = mapper;
        }

        [HttpGet("Data")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetData()
        {
            //_documentDbHelper.GetData();

            return Ok();
        }   
        
        /// <summary>
        /// 新增文件
        /// 1. Document 文件名稱
        /// 2. DocumentType 文件階層
        /// 3. DocumentNumber 文件編號 
        /// 4. DocumentOwner 權責人員
        /// 5. EffectiveDate 上版日期
        /// </summary>
        /// <param name="addDocumentFileDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("AddDocument")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddDocument([FromForm] AddDocumentFileDto addDocumentFileDto,CancellationToken cancellationToken)
        {
            var file = addDocumentFileDto.File;
            
            if (_fileHelper.VerifyFile(file))
            {
                //var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files");
                //await _fileHelper.SaveFileToDisc(file, pathBuilt);
                var fileName = file.Name;
                var fileType = _fileHelper.GetFileType(file);
                var base64ByFile = await _fileHelper.ConvertFileToBase64Async(file, cancellationToken);
                var compressionBase64ToByte = await _fileHelper.ZipAsync(base64ByFile, cancellationToken);
                
                var document = _mapper.Map<Document>(addDocumentFileDto);
                document.FileName = fileName;
                document.FileType = fileType;
                document.FileContent = compressionBase64ToByte;
                document.CreateBy = "Admin";
                document.CreateTime = DateTime.Now;

                await _stageDocumentHelper.AddDocumentAsync(document, cancellationToken);
            }
            else
            {
                return BadRequest(new { message = "Invalid file extension" });
            }

            return Ok();
        }

        /// <summary>
        /// 修改文件
        /// 1. Document 文件名稱
        /// 2. DocumentType 文件階層
        /// 3. DocumentNumber 文件編號 
        /// 4. DocumentOwner 權責人員
        /// 5. EffectiveDate 上版日期
        /// </summary>
        /// <param name="setDocumentFileDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("SetDocument")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SetDocument([FromForm] SetDocumentFileDto setDocumentFileDto,CancellationToken cancellationToken)
        {
            var document = _mapper.Map<Document>(setDocumentFileDto);

            var file = setDocumentFileDto.File;

            if (file != null && !string.IsNullOrEmpty(file.Name))
            {
                if (_fileHelper.VerifyFile(file))
                {
                    var fileName = file.Name;
                    var fileType = _fileHelper.GetFileType(file);
                    var base64ByFile = await _fileHelper.ConvertFileToBase64Async(file, cancellationToken);
                    var compressionBase64ToByte = await _fileHelper.ZipAsync(base64ByFile, cancellationToken);
                    
                    document.FileName = fileName;
                    document.FileType = fileType;
                    document.FileContent = compressionBase64ToByte;
                    
                    await _stageDocumentHelper.SetDocumentFileAsync(document, cancellationToken);
                }
            }
            
            document.UpdateBy = "Admin";
            document.UpdateTime = DateTime.Now;
            
            await _stageDocumentHelper.SetDocumentAsync(document, cancellationToken);


            return Ok();
        }        

        /// <summary>
        /// 發布文件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("PublishDocument/{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PublishDocument(int id,CancellationToken cancellationToken)
        {
            await _stageDocumentHelper.PublishDocumentAsync(id, cancellationToken);
            
            return Ok();
        }
        
        /// <summary>
        /// 取消發布文件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("UnPublishDocument/{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UnPublishDocument(int id,CancellationToken cancellationToken)
        {
            await _stageDocumentHelper.UnPublishDocumentAsync(id, cancellationToken);
            
            return Ok();
        }

        /// <summary>
        /// 文件更版
        /// </summary>
        /// <param name="changeDocumentVersionDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("ChangeDocumentVersion")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeDocumentVersion([FromForm] ChangeDocumentVersionDto changeDocumentVersionDto, CancellationToken cancellationToken)
        {
            await _stageDocumentHelper.ChangeDocumentVersionAsync(changeDocumentVersionDto, cancellationToken);
            
            return Ok();
        }

        /// <summary>
        /// 作廢文件
        /// </summary>
        /// <param name="invalidDocumentDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("Invalid")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Invalid([FromForm] InvalidDocumentDto invalidDocumentDto, CancellationToken cancellationToken)
        {
            await _stageDocumentHelper.InvalidDocumentAsync(invalidDocumentDto, cancellationToken);

            return Ok();
        }

        /// <summary>
        /// 取消作廢文件
        /// </summary>
        /// <param name="cancelVoidDocumentDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("CancelVoid")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CancelVoid([FromForm] CancelVoidDocumentDto cancelVoidDocumentDto, CancellationToken cancellationToken)
        {
            await _stageDocumentHelper.CancelVoidDocumentAsync(cancelVoidDocumentDto, cancellationToken);

            return Ok();
        }

        [HttpGet("DeleteDocument/{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteDocument(int id, CancellationToken cancellationToken)
        {
            await _stageDocumentHelper.DeleteDocumentAsync(id, cancellationToken);

            return Ok();
        }
    }
}
