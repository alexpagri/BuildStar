using System;

namespace BuildStar
{
    internal class RCON
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
}
