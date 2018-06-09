using Cache.General;
using CachingSolutions.Task1;
using CachingSolutions.Task2;
using NorthwindLibrary;
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
        private static string categoriesPrefix = "Cache_Categories";
        private static string customersPrefix = "Cache_Customers";
        private static string suppliersPrefix = "Cache_Suppliers";

        static void Main(string[] args)
        {
            FibonacciMemoryCacheTest();
            FibonacciRedisCache();

            CategoriesMemoryCache();
            CategoriesRedisCache();

            CustomersMemoryCache();
            CustomersRedisCache();

            SuppliersMemoryCache();
            SuppliersRedisCache();

            SqlMonitorsTest();

            Console.ReadKey();
        }

        static void FibonacciMemoryCacheTest()
        {
            Console.WriteLine("Fibonacci MemoryCache test");
            var fibonacci = new Fibonacci(new MemoryCache<int>(fibonacciPrefix));

            for (var i = 1; i < 20; i++)
            {
                Console.WriteLine(fibonacci.CalculateFibonacci(i));
            }

        }

        static void FibonacciRedisCache()
        {
            Console.WriteLine("Fibonacci Redis test");

            var fibonacci = new Fibonacci(new RedisCache<int>("localhost", fibonacciPrefix));

            for (var i = 1; i < 20; i++)
            {
                Console.WriteLine(fibonacci.CalculateFibonacci(i));
            }
        }

        static void CategoriesMemoryCache()
        {
            Console.WriteLine("CategoriesMemoryCache");
            var entitiesManager = new EntitiesManager<Category>(new MemoryCache<IEnumerable<Category>>(categoriesPrefix));

            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(entitiesManager.GetEntities().Count());
                Thread.Sleep(100);
            }
        }

        static void CategoriesRedisCache()
        {
            Console.WriteLine("CategoriesRedisCache");
            var entitiesManager = new EntitiesManager<Category>(new RedisCache<IEnumerable<Category>>("localhost", categoriesPrefix));

            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(entitiesManager.GetEntities().Count());
                Thread.Sleep(100);
            }
        }

        static void CustomersMemoryCache()
        {
            Console.WriteLine("CustomersMemoryCache");
            var entitiesManager = new EntitiesManager<Customer>(new MemoryCache<IEnumerable<Customer>>(customersPrefix));

            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(entitiesManager.GetEntities().Count());
                Thread.Sleep(100);
            }
        }

        static void CustomersRedisCache()
        {
            Console.WriteLine("CustomersRedisCache");
            var entitiesManager = new EntitiesManager<Customer>(new RedisCache<IEnumerable<Customer>>("localhost", customersPrefix));

            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(entitiesManager.GetEntities().Count());
                Thread.Sleep(100);
            }
        }

        static void SuppliersMemoryCache()
        {
            Console.WriteLine("SuppliersMemoryCache");
            var entitiesManager = new EntitiesManager<Supplier>(new MemoryCache<IEnumerable<Supplier>>(suppliersPrefix));

            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(entitiesManager.GetEntities().Count());
                Thread.Sleep(100);
            }
        }

        static void SuppliersRedisCache()
        {
            Console.WriteLine("SuppliersRedisCache");
            var entitiesManager = new EntitiesManager<Supplier>(new RedisCache<IEnumerable<Supplier>>("localhost", suppliersPrefix));

            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(entitiesManager.GetEntities().Count());
                Thread.Sleep(100);
            }
        }

        static void SqlMonitorsTest()
        {
            Console.WriteLine("SqlMonitorsTest");
            var entitiesManager = new MemoryEntitiesManager<Supplier>(new MemoryCache<IEnumerable<Supplier>>(suppliersPrefix),
                "select [SupplierID],[CompanyName],[ContactName],[ContactTitle],[Address],[City],[Region],[PostalCode],[Country],[Phone],[Fax] from [dbo].[Suppliers]");
            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(entitiesManager.GetEntities().Count());
                Thread.Sleep(1000);
            }
        }
    }
}
