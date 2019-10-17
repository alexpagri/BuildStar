using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace BuildStar
{
    class rgb
    {
        public Color[][] rgbs { get; private set; }

        public rgb(Bitmap bitmap)
        {
            int height = bitmap.Height;
            int width = bitmap.Width;
            rgbs = new Color[height][];
            for (int i = 0; i < height; i++)
            {
                rgbs[i] = new Color[width];
                for (int j = 0; j < width; j++)
                {
                    rgbs[0][0] = bitmap.GetPixel(i, j);
                }
            }
        }
    }

    class BlockPool
    {
        BitmapStatistics[] blocks;
    }

    class Comms
    {
        private Socket socket;
        private byte[] buffer = new byte[4096];
        private bool auth = false;
        public class RCON
        {
            private static int counter = 0;
            public enum CmdType : int
            {
                CommandResponse = 0,
                Command = 2,
                Login = 3
            }
            public int Length;
            public int RequestId;
            public CmdType Type;
            public byte[] Payload;

            public RCON(CmdType tp, byte[] str)
            {
                RequestId = counter++;
                Type = tp;
                Payload = (byte[])str.Clone();
                Length = Payload.Length + 2 * sizeof(int) + 2 * sizeof(byte);
            }

            public RCON(byte[] inp, bool bypassAssert = false)
            {
                var r = (byte[])inp.Clone();

                //Array.Reverse(r, 0, sizeof(int));

                //Array.Reverse(r, sizeof(int), sizeof(int));

                //Array.Reverse(r, 2 * sizeof(int), sizeof(int));

                Length = BitConverter.ToInt32(r, 0);

                if (inp.Length - sizeof(int) != Length && !bypassAssert)
                {
                    throw new Exception("assert failed at constructor");
                }

                RequestId = BitConverter.ToInt32(r, sizeof(int));

                Type = (CmdType)BitConverter.ToInt32(r, sizeof(int));

                Payload = new byte[Length - 2 * sizeof(byte) - 2 * sizeof(int)];

                Array.Copy(r, 3 * sizeof(int), Payload, 0, Length - 2 * sizeof(byte) - 2 * sizeof(int));
            }

            public byte[] make()
            {
                if (Payload.Length + 2 * sizeof(int) + 2 * sizeof(byte) != Length)
                {
                    throw new Exception("assert failed at make");
                }

                var r = new byte[Length + sizeof(int)];

                var len = BitConverter.GetBytes(Length);

                var rid = BitConverter.GetBytes(RequestId);

                var tp = BitConverter.GetBytes((int)Type);

                Array.Copy(len, 0, r, 0, sizeof(int));

                Array.Copy(rid, 0, r, sizeof(int), sizeof(int));

                Array.Copy(tp, 0, r, 2 * sizeof(int), sizeof(int));

                Array.Copy(Payload, 0, r, 3 * sizeof(int), Payload.Length);

                return r;
            }
        }

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
    class Program
    {
        static void Main(string[] args)
        {
            Bitmap bitmap = new Bitmap(100, 100);

            var x = new rgb(bitmap);

            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Connect("192.168.2.1", 25575);

            var comms = new Comms(socket);

            comms.login("123454321Testmynewpassword");

            Console.Out.WriteLine(comms.execute("help"));

            Console.ReadKey();
        }
    }
}
