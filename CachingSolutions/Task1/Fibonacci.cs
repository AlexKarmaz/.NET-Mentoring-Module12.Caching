using Cache.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CachingSolutions.Task1
{
    public class Fibonacci
    {
        private readonly ICache<int> cache;

        public Fibonacci(ICache<int> cache)
        {
            this.cache = cache;
        }

        public int CalculateFibonacci(int index)
        {
            if (index <= 0)
            {
                throw new ArgumentException($"{nameof(index)} must be positive number");
            }

            if (index == 1 || index == 2)
            {
                return 1;
            }

            int fromCache = cache.Get(index.ToString());
            if (fromCache != default(int))
            {
                Console.WriteLine($"From cache: {fromCache}");
                return fromCache;
            }

            int result = CalculateFibonacci(index - 1) + CalculateFibonacci(index - 2);
            Console.WriteLine($"Computed: {result}");
            cache.Set(index.ToString(), result, DateTimeOffset.Now.AddMilliseconds(300));
            return result;
        }
    }
}
