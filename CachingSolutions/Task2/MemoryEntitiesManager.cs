using Cache.General;
using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;

namespace CachingSolutions.Task2
{
    public class MemoryEntitiesManager<T> where T : class
    {
        private readonly MemoryCache<IEnumerable<T>> cache;
        private readonly string monitorCommand;

        public MemoryEntitiesManager(MemoryCache<IEnumerable<T>> cache, string monitorCommand)
        {
            this.cache = cache;
            this.monitorCommand = monitorCommand;
        }

        public IEnumerable<T> GetEntities()
        {
            Console.WriteLine("Get Entities");
            var user = Thread.CurrentPrincipal.Identity.Name;
            var entities = cache.Get(user);

            if (entities == null)
            {
                Console.WriteLine("From no cache storage");
                string connectionString;
                using (var dbContext = new Northwind())
                {
                    dbContext.Configuration.LazyLoadingEnabled = false;
                    dbContext.Configuration.ProxyCreationEnabled = false;
                    entities = dbContext.Set<T>().ToList();
                    connectionString = dbContext.Database.Connection.ConnectionString;
                }

                SqlDependency.Start(connectionString);
                cache.Set(user, entities, GetCachePolicy(monitorCommand, connectionString));
            }
            return entities;
        }

        private CacheItemPolicy GetCachePolicy(string monitorCommand, string connectionString)
        {
            return new CacheItemPolicy
            {
                ChangeMonitors = { GetMonitor(monitorCommand, connectionString) }
            };
        }

        private SqlChangeMonitor GetMonitor(string query, string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(query, connection);
                var monitor = new SqlChangeMonitor(new SqlDependency(command));
                command.ExecuteNonQuery();
                return monitor;
            }
        }
    }
}
