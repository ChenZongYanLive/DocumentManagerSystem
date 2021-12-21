using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DocumentLibrary.Application.Interface
{
    public interface IFileHelper
    {
        /// <summary>
        /// 檢查檔案是否符合規範
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        bool VerifyFile(IFormFile file);

        /// <summary>
        /// 取得檔案類型
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        string GetFileType(IFormFile file);

        /// <summary>
        /// 儲存檔案到實體位置
        /// </summary>
        /// <param name="file"></param>
        /// <param name="pathBuilt"></param>
        /// <returns></returns>
        Task<bool> SaveFileToDisc(IFormFile file, string pathBuilt);

        /// <summary>
        /// 將檔案轉成Base64字串
        /// </summary>
        /// <param name="file"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> ConvertFileToBase64Async(IFormFile file, CancellationToken cancellationToken);

        Task<byte[]> ZipAsync(string str, CancellationToken cancellationToken);
        Task<string> UnzipAsync(byte[] bytes, CancellationToken cancellationToken);
        string ConvertByteToBase64(byte[] bytes);
        byte[] ConvertBase64ToByte(string base64);
    }
}