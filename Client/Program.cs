using System;
using System.Threading;
using Grpc.Core;
using Helloworld;

namespace GreeterClient
{
    class Program
    {
        private static Channel channel;
        public static void Main(string[] args)
        {
            channel = new Channel("127.0.0.1:30051", ChannelCredentials.Insecure);

            ThreadPool.SetMaxThreads(50, 50);
            ThreadPool.SetMinThreads(50, 50);

            for (int i = 0; i < 200; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(TaskCallBack), i);
            }

            //channel.ShutdownAsync().Wait();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }


        private static void TaskCallBack(Object ThreadNumber)
        {
            try
            {
                Console.WriteLine(ThreadNumber);
                string ThreadName = "Thread " + ThreadNumber.ToString();
                Console.WriteLine(ThreadName + ": Started");
                var startTime = DateTime.Now;
                //var channel = new Channel("127.0.0.1:30051", ChannelCredentials.Insecure);
                var client = new Greeter.GreeterClient(channel);
                String user = "you";
                var reply = client.SayHello(new HelloRequest { Name = user });

                Console.WriteLine(ThreadName + ": Finished after " + (DateTime.Now-startTime).TotalMilliseconds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
    }

}