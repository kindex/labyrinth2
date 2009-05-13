using System;
using System.Collections.Generic;
using System.Text;
using Lidgren.Network;
using System.Threading;

namespace Game.Networing
{
    class Server
    {
        NetPeer s_peer;

        void Run()
        {
            // create a configuration for the server
            NetConfiguration config = new NetConfiguration("Labyrinth2");
            config.MaxConnections = 16;
            config.Port = 2009;

            // create server and start listening for connections
            NetServer server = new NetServer(config);
            server.SetMessageTypeEnabled(NetMessageType.ConnectionApproval, true);
            server.Start();

            // create a buffer to read data into
            NetBuffer buffer = server.CreateBuffer();

            // keep running until the user presses a key
            bool keepRunning = true;
            while (keepRunning)
            {
                NetMessageType type;
                NetConnection sender;

                // check if any messages has been received
                while (server.ReadMessage(buffer, out type, out sender))
                {
                    switch (type)
                    {
                        case NetMessageType.DebugMessage:
                            Logger.write("Net", buffer.ReadString());
                            break;

                        case NetMessageType.ConnectionApproval:
                            Logger.write("Net", "Approval; hail is " + buffer.ReadString());
                            sender.Approve();
                            break;

                        case NetMessageType.StatusChanged:
                            Logger.write("Net", "New status for " + sender + ": " + sender.Status + " (" + buffer.ReadString() + ")");
                            break;

                        case NetMessageType.Data:
                            // A client sent this data!
                            string msg = buffer.ReadString();

                            // send to everyone, including sender
                            NetBuffer sendBuffer = server.CreateBuffer();
                            sendBuffer.Write(sender.RemoteEndpoint.ToString() + " wrote: " + msg);

                            // send using ReliableInOrder
                            server.SendToAll(sendBuffer, NetChannel.ReliableInOrder1);
                            break;
                    }
                }

                Thread.Sleep(1);
            }

            server.Shutdown("Application exiting");
        }
    }
}
