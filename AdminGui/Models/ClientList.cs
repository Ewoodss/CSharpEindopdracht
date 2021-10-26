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

namespace AdminGui.Models
{
    public class ClientList : ObservableObject
    {
        private ObservableCollection<Client> clients;
        private readonly object _syncRoot = new object();
        private readonly ConcurrentQueue<NotifyCollectionChangedEventArgs> _uiItemQueue = new ConcurrentQueue<NotifyCollectionChangedEventArgs>();

        public ClientList()
        {
            this.clients = new ObservableCollection<Client>();
            this.Dispatcher = Application.Current.Dispatcher;
        }

        public Dispatcher Dispatcher { get; set; }

        public ObservableCollection<Client> Clients
        {
            get { return this.clients; }
            private set
            {
                this.clients = value;
                NotifyPropertyChanged();
            }
        }

        public void Add(string ip)
        {
            if (this.clients.Any(x => x.IPAdress == ip))
                return;

            this.Add(new Client() { IPAdress = ip});
        }

        public void Add(Client client)
        {
            NotifyCollectionChangedEventArgs args;
            lock (_syncRoot)
            {
                if (this.clients.Contains(client))
                    return;

                args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, client);

                this.AddTask(args);
            }
        }

        public void Remove(string ip)
        {
            Client client = this.clients.FirstOrDefault(x => x.IPAdress == ip);
            this.Remove(client);
        }

        public void Remove(Client client)
        {
            NotifyCollectionChangedEventArgs args;
            lock (_syncRoot)
            {
                if (!this.clients.Contains(client))
                    return;

                args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, client);

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
                            foreach (Client item in args.NewItems)
                            {
                                this.Clients.Insert(args.NewStartingIndex + offset, item);
                                offset++;
                            }
                        }
                        else
                        {
                            foreach (Client item in args.OldItems)
                            {
                                this.Clients.Add(item);
                            }
                        }
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        if (args.NewStartingIndex >= 0)
                        {
                            this.Clients.RemoveAt(args.NewStartingIndex);
                        }
                        else
                        {
                            foreach (Client item in args.OldItems)
                            {
                                this.Clients.Remove(item);
                            }
                        }
                        break;
                    default:
						throw new Exception("Unsupported NotifyCollectionChangedEventArgs.Action");
				}
			}
		}
	}
}
