using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cmd
{
    class Program
    {

        public static Dictionary<string, string> d = new Dictionary<string, string>();

        public delegate void CallBackDelegate(string message);

        static void Main(string[] args)
        {
            CallBackDelegate cbd = CallBack;
            Thread th = new Thread(Fun);
            th.Start(cbd);
            Console.ReadLine();
        }


        public static void Fun(object o)
        {

            CallBackDelegate cbd = o as CallBackDelegate;
            cbd("这个线程传回的消息");
        }

        private static void CallBack(string message)
        {
            Console.WriteLine(message);
        }
    }
}
