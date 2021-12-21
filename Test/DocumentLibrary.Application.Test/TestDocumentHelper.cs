using System.Threading;
using System.Threading.Tasks;
using DocumentLibrary.Application.DBHelper;
using DocumentLibrary.Application.Helper;
using DocumentLibrary.MySql.Models;
using NSubstitute;
using NUnit.Framework;

namespace DocumentLibrary.Application.Test
{

    public class TestDocumentHelper
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task 測試儲存文件並且回傳成功狀態()
        {
            var document = new Document { };

            var documentDbHelper = Substitute.For<IDocumentDbHelper>();
            
            var cancellationToken = new CancellationToken();
            
            await documentDbHelper.AddAsync(document, cancellationToken);

            var documentHelper = new StageDocumentHelper(documentDbHelper);

            await documentHelper.AddDocumentAsync(document,cancellationToken);

            //Assert.IsTrue(result);
        }
    }
}