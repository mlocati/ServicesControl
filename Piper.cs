using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Text;
using System.Threading;

namespace MLocati.ServicesControl
{
    abstract class Piper
    {
        protected void Write(PipeStream pipe, string command)
        {
            var bytes = Encoding.UTF8.GetBytes(command + "\x01");
            pipe.Write(bytes, 0, bytes.Length);
            pipe.Flush();
        }

        protected string Read(PipeStream pipe)
        {
            var bytes = new List<Byte>();
            for (; ; )
            {
                int b = pipe.ReadByte();
                if (b == '\x01')
                {
                    break;
                }
                if (b == -1)
                {
                    Thread.Sleep(50);
                    continue;
                }
                bytes.Add((byte)b);
            }
            return Encoding.UTF8.GetString(bytes.ToArray());
        }
    }
}
