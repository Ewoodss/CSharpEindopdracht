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
    public class SoftwareList : ObservableObject
    {
        private ObservableCollection<Software> software;
        private readonly object _syncRoot = new object();
        private readonly ConcurrentQueue<NotifyCollectionChangedEventArgs> _uiItemQueue = new ConcurrentQueue<NotifyCollectionChangedEventArgs>();

        public SoftwareList()
        {
            this.software = new ObservableCollection<Software>();
            this.Dispatcher = Application.Current.Dispatcher;
        }

        public Dispatcher Dispatcher { get; set; }

        public ObservableCollection<Software> Software
        {
            get { return this.software; }
            private set
            {
                this.software = value;
                NotifyPropertyChanged();
            }
        }

        public void Add(Software software)
        {
            NotifyCollectionChangedEventArgs args;
            lock (_syncRoot)
            {
                if (this.software.Contains(software))
                    return;

                args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, software);

                this.AddTask(args);
            }
        }

        public void Remove(Software software)
        {
            NotifyCollectionChangedEventArgs args;
            lock (_syncRoot)
            {
                if (!this.software.Contains(software))
                    return;

                args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, software);

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
                            foreach (Software item in args.NewItems)
                            {
                                this.Software.Insert(args.NewStartingIndex + offset, item);
                                offset++;
                            }
                        }
                        else
                        {
                            foreach (Software item in args.NewItems)
                            {
                                this.Software.Add(item);
                            }
                        }
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        if (args.NewStartingIndex >= 0)
                        {
                            this.Software.RemoveAt(args.NewStartingIndex);
                        }
                        else
                        {
                            foreach (Software item in args.OldItems)
                            {
                                this.Software.Remove(item);
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
            foreach (Software software in this.software)
            {
                this.Remove(software);
            }
        }
    }
}
