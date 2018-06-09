using Cache.General;
using CachingSolutions.Task1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CachingSolutions
{
    class Program
    {
        private static string fibonacciPrefix = "_fibonacci";

        static void Main(string[] args)
        {
            FibonacciMemoryCacheTest();
            FibonacciRedisCache();

            Console.ReadKey();
        }

        static void FibonacciMemoryCacheTest()
        {
            Console.WriteLine($"Fibonacci MemoryCache test");
            var fibonacci = new Fibonacci(new MemoryCache<int>(fibonacciPrefix));

            for (var i = 1; i < 20; i++)
            {
                Console.WriteLine(fibonacci.CalculateFibonacci(i));
                //Thread.Sleep(100);
            }

        }

        static void FibonacciRedisCache()
        {
            Console.WriteLine($"Fibonacci Redis test");

            var fibonacci = new Fibonacci(new RedisCache<int>("localhost", fibonacciPrefix));

            for (var i = 1; i < 20; i++)
            {
                Console.WriteLine(fibonacci.CalculateFibonacci(i));
                Thread.Sleep(100);
            }
        }
    }
}
