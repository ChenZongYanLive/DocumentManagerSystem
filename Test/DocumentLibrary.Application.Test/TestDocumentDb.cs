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
        public async Task �����x�s����T�B�^�Ǫ����id��1()
        {
            var document = new Document { 
                DocumentName = "��O��",
                CreateBy = "�v��"
            };

            var documentHelper = new DocumentDbHelper();
            
            var cancellationToken = new CancellationToken();

            await documentHelper.AddAsync(document,cancellationToken);

            Assert.AreEqual(1, document.Id);
        }

        [Test]
        public void �����x�s����T�ɦp�G����g���W�٥����^�ǿ��~�T��()
        {
            var document = new Document();

            var documentHelper = new DocumentDbHelper();
            
            var cancellationToken = new CancellationToken();
            
            var ex = Assert.ThrowsAsync<NullReferenceException>(() => documentHelper.AddAsync(document,cancellationToken));

            Assert.That(ex?.Message, Is.EqualTo("������W��"));
        }

        [Test]
        public void �����x�s����T�ɦp�G����g�إߪ̥����^�ǿ��~�T��()
        {
            var document = new Document {
                DocumentName = "��O��"
            };

            var documentHelper = new DocumentDbHelper();
            
            var cancellationToken = new CancellationToken();
            
            var ex = Assert.ThrowsAsync<NullReferenceException>(() => documentHelper.AddAsync(document,cancellationToken));

            Assert.That(ex?.Message, Is.EqualTo("����إߪ�"));
        }
    }
}