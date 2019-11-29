using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Infra.Extensions
{
    internal static class ModelBuilderExtensions
    {
        public static void Initialize<T>(this ModelBuilder modelBuilder)
        {
            var typesToMapping = (from x in Assembly.GetExecutingAssembly().GetTypes()
                                  where x.IsClass && typeof(T).IsAssignableFrom(x)
                                  select x).ToList();

            foreach (var mapping in typesToMapping)
            {
                dynamic mappingClass = Activator.CreateInstance(mapping);
                modelBuilder.ApplyConfiguration(mappingClass);
            }
        }

        public static void UseConventions(this ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var key in entity.GetKeys())
                {
                    key.Relational().Name = key.Relational().Name.ToShortName();
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.Relational().Name = key.Relational().Name.ToShortName();
                }

                foreach (var index in entity.GetIndexes())
                {
                    index.Relational().Name = index.Relational().Name.ToShortName();
                }
            }
        }

        private static string ToShortName(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return input.Length > 64 ? input.Substring(0, 64).ToUpper() : input.ToUpper();
        }
    }
}