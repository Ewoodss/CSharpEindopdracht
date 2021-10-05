﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;
using Newtonsoft.Json;

namespace Server
{
    public class ConnectionsManager
    {
        public List<Connection> Connections { get; private set; }

        public ConnectionsManager()
        {
            this.Connections = new List<Connection>();
        }

        public void AddConnection(Connection connection)
        {
            if (this.Connections.Contains(connection))
                return;

            this.Connections.Add(connection);

            RequestData<object> testRequestData = new RequestData<object>();
            testRequestData.Action = "hello";
            string serializeObject = JsonUtils.serializeStringData(testRequestData);
            Console.WriteLine("testing: " + serializeObject);
            connection.SendString(serializeObject);
        }

        public void RemoveConnection(Connection connection)
        {
            if (!this.Connections.Contains(connection))
                return;

            this.Connections.Remove(connection);
            Console.WriteLine($"Connection {connection} disconected");
        }
    }
}