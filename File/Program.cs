﻿using System;
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
        static List<string> result = new List<string>();
        static void Main(string[] args)
        {
            DirectoryInfo curr = new DirectoryInfo(".");
            string filePath = curr.FullName + @"\res.txt";
            int count = 40;

            Thread[] ths = new Thread[count];
            for (int i = 0; i < count; i++)
            {
                Thread t = new Thread(new ThreadStart(ThreadWrite));
                t.Name = "th" + i;
                ths[i] = t;
                t.Start();
            }

            for (int i = 0; i < count; i++)
            {
                ths[i].Join();
            }

            File.WriteAllLines(filePath, result);
            Console.WriteLine(filePath);
            Console.ReadKey();
        }

        public static void ThreadWrite()
        {
            lock (result)
            {
                result.Add(string.Concat(Thread.CurrentThread.Name, ", ", new Random().NextDouble()));
                Thread.Sleep(15);
            }
            Console.WriteLine("Thread finished." + Thread.CurrentThread.Name);
        }
    }
}
