using System.Threading;

namespace TestDownloader
{
    public class FileDownloader
    {
        public byte[] Download(string url)
        {
            //    Имитируем бурную деятельность
            Thread.Sleep(2000);
            return new byte[] {1, 2, 3, 4};
        }

        
    }
}