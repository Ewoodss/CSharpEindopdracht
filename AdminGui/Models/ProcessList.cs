using Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Framework.Models;

namespace AdminGui.Models
{
    public class ProcessList : ObservableObject
    {
        private ObservableCollection<Process> processes;
        private readonly object _syncRoot = new object();
        private readonly ConcurrentQueue<NotifyCollectionChangedEventArgs> _uiItemQueue = new ConcurrentQueue<NotifyCollectionChangedEventArgs>();

        public ProcessList()
        {
            this.processes = new ObservableCollection<Process>();
            this.Dispatcher = Application.Current.Dispatcher;
        }

        public Dispatcher Dispatcher { get; set; }

        public ObservableCollection<Process> Processes
        {
            get { return this.processes; }
            private set
            {
                this.processes = value;
                NotifyPropertyChanged();
            }
        }

        public void Add(Process process)
        {
            NotifyCollectionChangedEventArgs args;
            lock (_syncRoot)
            {
                if (this.processes.Contains(process))
                    return;

                args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, process);

                this.AddTask(args);
            }
        }

        public void Remove(Process process)
        {
            NotifyCollectionChangedEventArgs args;
            lock (_syncRoot)
            {
                if (!this.processes.Contains(process))
                    return;

                args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, process);

                this.AddTask(args);
            }
        }

        private void AddTask(NotifyCollectionChangedEventArgs args)
        {
            _uiItemQueue.Enqueue(args);
            Dispatcher.BeginInvoke(new Action(this.ProcessQueue));
        }

		private void ProcessQueue()
		{
			// This Method should always be invoked only by the UI thread only.
			if (!this.Dispatcher.CheckAccess())
			{
				throw new Exception("Can't be called from any thread than the dispatcher one");
			}

			NotifyCollectionChangedEventArgs args;
			while (this._uiItemQueue.TryDequeue(out args))
			{
				switch (args.Action)
				{
                    case NotifyCollectionChangedAction.Add:
                        if (args.NewStartingIndex >= 0)
                        {
                            int offset = 0;
                            foreach (Process item in args.NewItems)
                            {
                                this.Processes.Insert(args.NewStartingIndex + offset, item);
                                offset++;
                            }
                        }
                        else
                        {
                            foreach (Process item in args.OldItems)
                            {
                                this.Processes.Add(item);
                            }
                        }
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        if (args.NewStartingIndex >= 0)
                        {
                            this.Processes.RemoveAt(args.NewStartingIndex);
                        }
                        else
                        {
                            foreach (Process item in args.OldItems)
                            {
                                this.Processes.Remove(item);
                            }
                        }
                        break;
                    default:
						throw new Exception("Unsupported NotifyCollectionChangedEventArgs.Action");
				}
			}
		}

        public void Clear()
        {
            foreach (Process process in this.processes)
            {
                this.Remove(process);
            }
        }
    }
}
