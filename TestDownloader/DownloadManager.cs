using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace TestDownloader
{
    public class DownloadManager
    {
        
        private FileDownloader _fileDownloader;
        private TaskExtender<byte[]> _lastInQueue;
        private ConcurrentQueue<TaskExtender<byte[]>> _tasks = new ConcurrentQueue<TaskExtender<byte[]>>();

        private DownloadManager()
        {
            _fileDownloader = new FileDownloader();
        }

        /// <summary>
        ///     Creates single instance on DownloadManager
        /// </summary>
        public static DownloadManager Create
        {
            get { return new DownloadManager(); }
        }


        /// <summary>
        ///     Adds url into download list
        /// </summary>
        /// <param name="url">url of file to download</param>
        /// <returns>Task Extender containing information about requested download task</returns>
        public TaskExtender<byte[]> AddToDownload(string url)
        {
            return AddToDownload(url, false);
        }

        public TaskExtender<byte[]> AddToDownload(string url, bool removeOnComplete)
        {
            //if url is not in queue, than create new task

            if (_tasks.Any(t => t.TaskName == url))
            {
                return _tasks.First(t => t.TaskName == url);
            }
            TaskExtender<byte[]> newTask;
            if (_lastInQueue != null)
            {
                //if task is not first in queue, than create this task as continuation for previous one
                newTask = _lastInQueue.ContinueWith(nxt => { return _fileDownloader.Download(url); }, url, removeOnComplete
                    );
            }
            else
            {
                //this is first task in queue, just add it to the queue
                newTask = new TaskExtender<byte[]>(() => { return _fileDownloader.Download(url); }, url, removeOnComplete);
            }
            newTask.OnTaskCompleted += InnerCompletion;
            _tasks.Enqueue(newTask);
            _lastInQueue = newTask;
            return _lastInQueue;
        }

        private void InnerCompletion(object sender, TaskCompleteEventArgs<byte[]> e)
        {
            if (e.RemoveOnCompleted)
            {
                TaskExtender<byte[]> topLevel;
                _tasks.TryPeek(out topLevel);
                if (topLevel == sender)
                {
                    _tasks.TryDequeue(out topLevel);
                }
            }
        }

        /// <summary>
        ///     Starts download sequence, from the top task in queue
        ///     if no tasks were added, throw
        /// </summary>
        public void StartDownload()
        {
            TaskExtender<byte[]> topLevel;
            _tasks.TryPeek(out topLevel);
            if (topLevel == null)
            {
                throw new ArgumentNullException("No tasks to start");
            }
            if (topLevel.Status == TaskStatus.Created)
            {
                topLevel.Start();
            }
        }

     }
}