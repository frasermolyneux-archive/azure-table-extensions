using System;
using System.Collections.Generic;
using System.Linq;
using FM.AzureTableExtensions.Library.Attributes;
using Microsoft.Azure.Cosmos.Table;

namespace FM.AzureTableExtensions.Library.Converters
{
    public class EntityEnumPropertyConverter
    {
        public static void Serialize<TEntity>(TEntity entity, IDictionary<string, EntityProperty> results)
        {
            entity.GetType().GetProperties()
                .Where(x => x.GetCustomAttributes(typeof(EntityEnumPropertyConverterAttribute), false).Any())
                .ToList()
                .ForEach(x => results.Add(x.Name,
                    new EntityProperty(x.GetValue(entity) != null ? x.GetValue(entity).ToString() : null)));
        }

        public static void Deserialize<TEntity>(TEntity entity, IDictionary<string, EntityProperty> properties)
        {
            var customProperties = entity.GetType().GetProperties()
                .Where(x => x.GetCustomAttributes(typeof(EntityEnumPropertyConverterAttribute), false).Any())
                .ToList();

            foreach (var customProperty in customProperties)
                if (properties.ContainsKey(customProperty.Name))
                    customProperty.SetValue(entity,
                        Enum.Parse(customProperty.PropertyType, properties[customProperty.Name].StringValue));
                else
                    try
                    {
                        customProperty.SetValue(entity, null);
                    }
                    catch
                    {
                        throw new Exception($"Could not set {customProperty.Name} value to null");
                    }
        }
    }
}