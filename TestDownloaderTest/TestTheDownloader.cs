using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TestDownloader;

namespace TestDownloaderTest
{
    [TestFixture]
    public class TestTheDownloader
    {
        [SetUp]
        public void BeforeEachTask()
        {
        }

        [Test]
        public void ExceptionIfNoTasksAdded()
        {
            Assert.Throws<ArgumentNullException>(() => DownloadManager.Create.StartDownload());
        }

        [Test]
        public void AddingTaskWithSameArgumentReturnsTheSame()
        {
            var downloader = DownloadManager.Create;
            var ext1 = downloader.AddToDownload("one");
            var ext2 = downloader.AddToDownload("one");
            Assert.AreSame(ext1,ext2);
        }

        [Test]
        public void AddingTaskWithDifferentArgumentReturnsDifferent()
        {
            var downloader = DownloadManager.Create;
            var ext1 = downloader.AddToDownload("one");
            var ext2 = downloader.AddToDownload("two");
            Assert.AreNotSame(ext1, ext2);
        }

        
    }
}
