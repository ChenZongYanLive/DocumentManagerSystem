using DocumentLibrary.Application.DBHelper.FileData;
using DocumentLibrary.MySql.Models;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentLibrary.Application.Test
{
    public class TestDocumentDb
    {
        [SetUp]
        public void Setup()
        {
            File.Delete("Document.json");
        }
        

        [Test]
        public async Task 測試儲存文件資訊且回傳的文件id為1()
        {
            var document = new Document { 
                DocumentName = "日記本",
                CreateBy = "宗彥"
            };

            var documentHelper = new DocumentDbHelper();
            
            var cancellationToken = new CancellationToken();

            await documentHelper.AddAsync(document,cancellationToken);

            Assert.AreEqual(1, document.Id);
        }

        [Test]
        public void 測試儲存文件資訊時如果未填寫文件名稱必須回傳錯誤訊息()
        {
            var document = new Document();

            var documentHelper = new DocumentDbHelper();
            
            var cancellationToken = new CancellationToken();
            
            var ex = Assert.ThrowsAsync<NullReferenceException>(() => documentHelper.AddAsync(document,cancellationToken));

            Assert.That(ex?.Message, Is.EqualTo("未填文件名稱"));
        }

        [Test]
        public void 測試儲存文件資訊時如果未填寫建立者必須回傳錯誤訊息()
        {
            var document = new Document {
                DocumentName = "日記本"
            };

            var documentHelper = new DocumentDbHelper();
            
            var cancellationToken = new CancellationToken();
            
            var ex = Assert.ThrowsAsync<NullReferenceException>(() => documentHelper.AddAsync(document,cancellationToken));

            Assert.That(ex?.Message, Is.EqualTo("未填建立者"));
        }
    }
}