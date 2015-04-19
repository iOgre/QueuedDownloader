using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TestDownloader;

namespace DownloaderExampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var instance = DownloadManager.Create;
            instance.AddToDownload("xxx")
                .OnTaskCompleted += DownloadCompleted;
            instance.AddToDownload("yyy")
                 .OnTaskCompleted += DownloadCompleted;
            instance.AddToDownload("rrrrr", true);

            instance.AddToDownload("rrrrr")
                 .OnTaskCompleted += DownloadCompleted;
            instance.AddToDownload("rrrrr");

            instance.StartDownload();

            instance.AddToDownload("xxxr")
                .OnTaskCompleted += DownloadCompleted;
        
            Console.ReadLine();
        }

        private static void DownloadCompleted(object sender, TaskCompleteEventArgs<byte[]> e)
        {
            
            Console.WriteLine("Finished: {0}", e.TaskName);
            foreach (var dataItem in e.Result)
            {
                Console.Write(" " + dataItem);
            }
            Console.WriteLine("");
            Console.WriteLine("--------");

        }
    }
}
