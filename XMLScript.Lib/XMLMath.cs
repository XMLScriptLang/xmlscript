using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLScript.Lib
{
    public class XMLMath
    {
        // Implement for various types because XMLScript currently does not support generics

        // int 
        public static int Add(int a, int b)
        {
            return a + b;
        }

        public static int Subtract(int a, int b)
        {
            return a - b;
        }

        public static int Multiply(int a, int b)
        {
            return a * b;
        }

        public static int Divide(int a, int b)
        {
            return a / b;
        }

        public static int Modulo(int a, int b)
        {
            return a % b;
        }

        public static int Power(int a, int b)
        {
            return (int)Math.Pow(a, b);
        }

        public static int Negate(int a)
        {
            return -a;
        }

        public static int Abs(int a)
        {
            return Math.Abs(a);
        }

        public static int Max(int a, int b)
        {
            return Math.Max(a, b);
        }

        // double
        public static double Add(double a, double b)
        {
            return a + b;
        }

        public static double Subtract(double a, double b)
        {
            return a - b;
        }

        public static double Multiply(double a, double b)
        {
            return a * b;
        }

        public static double Divide(double a, double b)
        {
            return a / b;
        }

        public static double Modulo(double a, double b)
        {
            return a % b;
        }

        public static double Power(double a, double b)
        {
            return Math.Pow(a, b);
        }

        public static double Negate(double a)
        {
            return -a;
        }

        public static double Abs(double a)
        {
            return Math.Abs(a);
        }
    }
}
