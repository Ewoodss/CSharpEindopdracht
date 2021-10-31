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

namespace AdminGui.Util
{
    public class ThreadSafeObservableList<T> : ObservableObject
    {
        private ObservableCollection<T> items;
        private readonly object _syncRoot = new object();
        private readonly ConcurrentQueue<NotifyCollectionChangedEventArgs> _uiItemQueue = new ConcurrentQueue<NotifyCollectionChangedEventArgs>();

        public ThreadSafeObservableList()
        {
            this.items = new ObservableCollection<T>();
            this.Dispatcher = Application.Current.Dispatcher;
        }

        public Dispatcher Dispatcher { get; set; }

        public ObservableCollection<T> Items
        {
            get { return this.items; }
            private set { this.items = value; NotifyPropertyChanged(); }
        }

        public void Add(T item)
        {
            NotifyCollectionChangedEventArgs args;
            lock (_syncRoot)
            {
                if (this.items.Contains(item))
                    return;

                args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item);

                this.AddTask(args);
            }
        }

        public void Remove(T item)
        {
            NotifyCollectionChangedEventArgs args;
            lock (_syncRoot)
            {
                if (!this.items.Contains(item))
                    return;

                args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item);

                this.AddTask(args);
            }
        }

        public virtual void Clear()
        {
            NotifyCollectionChangedEventArgs args = null;
            lock (_syncRoot)
            {
                args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                AddTask(args);
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
                            foreach (T item in args.NewItems)
                            {
                                this.Items.Insert(args.NewStartingIndex + offset, item);
                                offset++;
                            }
                        }
                        else
                        {
                            foreach (T item in args.NewItems)
                            {
                                this.Items.Add(item);
                            }
                        }
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        if (args.NewStartingIndex >= 0)
                        {
                            this.Items.RemoveAt(args.NewStartingIndex);
                        }
                        else
                        {
                            foreach (T item in args.OldItems)
                            {
                                this.Items.Remove(item);
                            }
                        }
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        this.Items.Clear();
                        break;
                    default:
                        throw new Exception("Unsupported NotifyCollectionChangedEventArgs.Action");
                }
            }
        }
    }
}
