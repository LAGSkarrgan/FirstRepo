using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace ConsoleApplication1
{
    class Program
    {
        static Object locker = new object();
        static List<string> result = new List<string>();
        static string fileName = "threadFile.txt";
        static string filePath;

        static void Main(string[] args)
        {
            int threadCount = 20;
            Directory.CreateDirectory(Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "ThreadWrite"
            ));
            filePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "ThreadWrite",
                fileName
            );

            Console.WriteLine($"Writing to file {filePath}.");
            Console.WriteLine("Press any key to start writing.");
            Console.ReadKey();

            Thread[] threads = new Thread[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                StartNewThread(threads, i);
            }
            
            while(true)
            {
                for (int i = 0; i < threadCount; i++)
                {
                    if (threads[i].ThreadState != ThreadState.Running)
                        StartNewThread(threads, i);
                }
            }
        }
        static void StartNewThread(Thread[] threads, int i)
        {
            Thread thread = new Thread(new ThreadStart(ThreadWrite))
            {
                Name = "thread" + i
            };
            threads[i] = thread;
            thread.Start();
        }
        static void ThreadWrite()
        {
            lock (locker)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(new FileStream(filePath, FileMode.Append)))
                    {
                        writer.WriteLine($"Zapisuje vlánko {Thread.CurrentThread.Name}.");
                        Console.WriteLine("Done.");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("TODO zalogovat exception.");
                }
            }
        }
    }
}
