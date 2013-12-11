using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathTest
{
    public class A : IDisposable
    {
        public A A1 { get; set; }
        public A()
        {
            
        }

        ~A()
        {
            Console.WriteLine("destructor!");
        }

        public void Dispose()
        {
            Console.WriteLine("diposed!");
            GC.SuppressFinalize(this);
        }
        
    }
    class Program
    {
        static void Main(string[] args)
        {
            var a = new A();
            var refa  = new A();
            a.A1 = refa;
            //a = null;
            GC.Collect();
            Console.ReadKey();
            refa.Dispose();
            
        }

        public static void Call()
        {
            
        }
    }
}
