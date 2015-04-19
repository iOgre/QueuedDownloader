using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace TestDownloader
{
    public class TaskCompleteEventArgs<TResult>: EventArgs
    {
        public TaskStatus Status { get; set; }
        public TResult Result { get; set; }
        public string TaskName { get; set; }
        public bool RemoveOnCompleted { get; set; }
    }
    public class TaskExtender<T>
    {
        
        public string TaskName { get; set; }
        /// <summary>
        /// Event to execute on task completion
        /// </summary>
        public event EventHandler<TaskCompleteEventArgs<T>> OnTaskCompleted;
        
        /// <summary>
        /// Task assigned to Extender
        /// </summary>
        public Task<T> Task
        {
            get { return _task; }
        }

        public int TaskId
        {
            get { return _task.Id; }
        }
        /// <summary>
        /// Status of the extender's task
        /// </summary>
        public TaskStatus Status
        {
            get { return _task.Status; }
        }

        private Task<T> _task;
       /// <summary>
       /// Creates new instance of TaskExtender
       /// </summary>
       /// <param name="function">function to create task</param>
       /// <param name="name">name of extender</param>
        public TaskExtender(Func<T> function, string name) {

            _task = new Task<T>(function);
            TaskName = name;
            AssignAwaiter(false);

        }

        /// <summary>
        ///  Creates new instance of TaskExtender
        /// </summary>
        /// <param name="function">function to create task</param>
        /// <param name="name">name of extender</param>
        /// <param name="removeOnCompeted">remove the task from queue on completion, to allow readding</param>
        public TaskExtender(Func<T> function, string name, bool removeOnCompeted)
        {

            _task = new Task<T>(function);
            TaskName = name;
            AssignAwaiter(removeOnCompeted);

        }

        private void AssignAwaiter(bool removeOnCompleted)
        {
            
            _task.GetAwaiter().OnCompleted(() =>
            {
                
                if (OnTaskCompleted != null)
                {
                    OnTaskCompleted.Invoke(this, new TaskCompleteEventArgs<T>
                    {
                        Result = this.Task.Result,
                        Status = this.Status,
                        TaskName = this.TaskName,
                        RemoveOnCompleted = removeOnCompleted
                    });
                }
            });
        }
        private TaskExtender(Task<T> task, string name)
        {
            _task = task;
            TaskName = name;
            AssignAwaiter(false);
        }

        private TaskExtender(Task<T> task, string name, bool removeOnCompeted)
        {
            _task = task;
            TaskName = name;
            AssignAwaiter(removeOnCompeted);
        }

        /// <summary>
        /// Allows to set Func as continuation to current TaskExtender;
        /// </summary>
        /// <param name="func">func to create the task</param>
        /// <param name="taskName">task name</param>
        /// <param name="removeOnCompleted">remove extender from queue on completion</param>
        /// <returns></returns>
        public TaskExtender<T> ContinueWith<T>(Func<object, T> func, string taskName, bool removeOnCompleted)
        {
            Task<T> newTask = this.Task.ContinueWith(func);
            var newExtender = new TaskExtender<T>(newTask, taskName, removeOnCompleted);
            newExtender.TaskName = taskName;
            return newExtender;

        }

        /// <summary>
        /// Starts the TaskExtender
        /// </summary>
        /// <returns>an awaiter over TaskExtender's task</returns>
        public TaskAwaiter<T> Start()
        {
            _task.Start();
            return _task.GetAwaiter();

        }

        public void ClearEvents()
        {
            this.OnTaskCompleted = null;
        }
    }
}