using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;

namespace Demo.Info
{
    class ProInfo
    {
        public static void GetProcessInfo(string port) {
            ParameterizedThreadStart threadStart = new ParameterizedThreadStart(ThreadRun);
            Thread thread = new Thread(threadStart);
            thread.Start(port);
        }

        private static void ThreadRun(object port) {
            using (NamedPipeServerStream pipeServer = new NamedPipeServerStream("dtcz"+port, PipeDirection.Out))
            {
                // 等待连接，程序会阻塞在此处，直到有一个连接到达
                pipeServer.WaitForConnection();
                try
                {
                    using (StreamWriter sw = new StreamWriter(pipeServer))
                    {
                        sw.AutoFlush = true;
                        // 向连接的客户端发送消息
                        sw.WriteLine(port);
                    }
                }
                catch (IOException e)
                {
                    Console.WriteLine("ERROR: {0}", e.Message);
                }
            }
        }
    }
}
