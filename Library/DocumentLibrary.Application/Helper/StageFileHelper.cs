using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DocumentLibrary.Application.Interface;
using Microsoft.AspNetCore.Http;

namespace DocumentLibrary.Application.Helper
{

    public class StageFileHelper : IFileHelper
    {
        private readonly Dictionary<string, string> _fileTypeDictionary = new Dictionary<string, string>
        {
            {".xlsx","xlsx"},
            {".xls","xls"},
            {".pdf","pdf"}
        };

        public  StageFileHelper()
        {
            
        }

        /// <summary>
        /// 檢查檔案是否符合規範
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool VerifyFile(IFormFile file)
        {
            return _fileTypeDictionary.ContainsKey(GetFileType(file));
        }

        /// <summary>
        /// 取得副檔名
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string GetFileType(IFormFile file)
        {
            return Path.GetExtension(file.FileName);
        }


        /// <summary>
        /// 寫入檔案
        /// </summary>
        /// <param name="file"></param>
        /// <param name="pathBuilt"></param>
        /// <returns></returns>
        public async Task<bool> SaveFileToDisc(IFormFile file, string pathBuilt)
        {
            var isSaveSuccess = false;
            try
            {
                var extension = GetFileType(file);
                var fileName = DateTime.Now.Ticks + extension;

                if (!Directory.Exists(pathBuilt))
                {
                    Directory.CreateDirectory(pathBuilt);
                }

                var fullPath = Path.Combine(pathBuilt,fileName);

                await using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                isSaveSuccess = true;
            }
            catch (Exception)
            {
                //log error
            }

            return isSaveSuccess;
        }


        /// <summary>
        /// 將檔案轉成Base64字串
        /// </summary>
        /// <param name="file"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> ConvertFileToBase64Async(IFormFile file, CancellationToken cancellationToken)
        {
            await using var ms = new MemoryStream();
            await file.CopyToAsync(ms, cancellationToken);
            var fileBytes = ms.ToArray();
            var s = Convert.ToBase64String(fileBytes);
            return s;
        }
        
        public async Task<byte[]> ZipAsync(string str, CancellationToken cancellationToken) {
            var bytes = Encoding.UTF8.GetBytes(str);

            await using var msi = new MemoryStream(bytes);
            await using var mso = new MemoryStream();
            await using (var gs = new BrotliStream(mso, CompressionMode.Compress)) {
                await CopyTo(msi, gs, cancellationToken);
            }

            return mso.ToArray();
        }

        public async Task<string> UnzipAsync(byte[] bytes, CancellationToken cancellationToken)
        {
            await using var msi = new MemoryStream(bytes);
            await using var mso = new MemoryStream();
            await using (var gs = new BrotliStream(msi, CompressionMode.Decompress)) {
                await CopyTo(gs, mso, cancellationToken);
            }

            return Encoding.UTF8.GetString(mso.ToArray());
        }
        private static async Task CopyTo(Stream src, Stream dest, CancellationToken cancellationToken) {
            var bytes = new byte[4096];

            int cnt;

            while ((cnt = await src.ReadAsync(bytes, 0, bytes.Length, cancellationToken)) != 0) {
                await dest.WriteAsync(bytes, 0, cnt, cancellationToken);
            }
        }

        public string ConvertByteToBase64(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        public byte[] ConvertBase64ToByte(string base64)
        {
            return Convert.FromBase64String(base64);
        }
    }
}