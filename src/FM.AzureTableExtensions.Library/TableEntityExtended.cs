using System.Collections.Generic;
using FM.AzureTableExtensions.Library.Converters;
using Microsoft.Azure.Cosmos.Table;

namespace FM.AzureTableExtensions.Library
{
    public class TableEntityExtended : TableEntity
    {
        public override IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            var results = base.WriteEntity(operationContext);
            EntityJsonPropertyConverter.Serialize(this, results);
            NullableEntityEnumPropertyConverter.Serialize(this, results);
            EntityEnumPropertyConverter.Serialize(this, results);
            return results;
        }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties,
            OperationContext operationContext)
        {
            base.ReadEntity(properties, operationContext);
            EntityJsonPropertyConverter.Deserialize(this, properties);
            NullableEntityEnumPropertyConverter.Deserialize(this, properties);
            EntityEnumPropertyConverter.Deserialize(this, properties);
        }
    }
}