using System;
using System.Text;
using System.Net.Sockets;

namespace BuildStar
{
    public class Comms
    {
        private Socket socket;
        private byte[] buffer = new byte[4096];
        private bool auth = false;

        public Comms(Socket connectedSocket, int timeoutMilliseconds = 5000)
        {
            if (connectedSocket.Connected == false)
            {
                throw new Exception("connect the socket first!");
            }
            socket = connectedSocket;
            socket.ReceiveTimeout = timeoutMilliseconds;
        }

        public bool login(string password)
        {
            socket.Send(new RCON(RCON.CmdType.Login, Encoding.ASCII.GetBytes(password)).make());

            socket.Receive(buffer, 4096, SocketFlags.None);

            var response = new RCON(buffer, true);

            if (response.RequestId == -1)
            {
                Console.Out.WriteLine("auth failed");
                return false;
            }

            auth = true;
            return true;
        }

        public string execute(string command)
        {
            if (auth == false)
            {
                throw new Exception("call login() first / login failed previously (returned false)!");
            }

            socket.Send(new RCON(RCON.CmdType.Command, Encoding.ASCII.GetBytes(command)).make());

            socket.Receive(buffer, 4096, SocketFlags.None);

            return Encoding.ASCII.GetString(new RCON(buffer, true).Payload);
        }
    }
}
