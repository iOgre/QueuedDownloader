using System;
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
        public void AddingTaskWithDifferentArgumentReturnsDifferent()
        {
            DownloadManager downloader = DownloadManager.Create;
            TaskExtender<byte[]> ext1 = downloader.AddToDownload("one");
            TaskExtender<byte[]> ext2 = downloader.AddToDownload("two");
            Assert.AreNotSame(ext1, ext2);
        }

        [Test]
        public void AddingTaskWithSameArgumentReturnsTheSame()
        {
            DownloadManager downloader = DownloadManager.Create;
            TaskExtender<byte[]> ext1 = downloader.AddToDownload("one");
            TaskExtender<byte[]> ext2 = downloader.AddToDownload("one");
            Assert.AreSame(ext1, ext2);
        }

        [Test]
        public void ExceptionIfNoTasksAdded()
        {
            Assert.Throws<ArgumentNullException>(() => DownloadManager.Create.StartDownload());
        }
    }
}