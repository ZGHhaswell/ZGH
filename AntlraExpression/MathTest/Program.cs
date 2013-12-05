using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathTest
{
    class Program
    {
        static void Main(string[] args)
        {
            double B = 1600;
            double H = 2100;
            double H1 = 300;
            double T = 55;

            double degrees = Math.Asin(B * H1 / (Math.Pow(H1, 2)+ 0.25 * Math.Pow(B, 2))) * (Math.Pow(H1, 2) + 0.25 * Math.Pow(B, 2)) / H1 + 2 * H + B;
            Console.Write(degrees);
        }
    }
}
