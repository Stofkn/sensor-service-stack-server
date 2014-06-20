using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace ServiceModel
{
    public interface ISensor
    {
        string Name { get; set; }
        double Value { get; set; }
        DateTime? LastDateValue { get; set; }
    }

    /*public class SensorValue
    {
        public double Value { get; set; }
        public DateTime? Date { get; set; }
    }*/

    public class Sensor : ISensor
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public double Value { get; set; }
        public DateTime? LastDateValue { get; set; }
    }

    public class SensorEntity : TableEntity, ISensor
    {
        public SensorEntity(string sensorId)
        {
            PartitionKey = sensorId;
            RowKey = Guid.NewGuid().ToString();
        }

        public SensorEntity() { }

        public string Name { get; set; }
        public double Value { get; set; }
        public DateTime? LastDateValue { get; set; }
    }

    public class SensorValueEntity : TableEntity
    {
        public SensorValueEntity(string sensorId, double value)
        {
            PartitionKey = sensorId;
            RowKey = Guid.NewGuid().ToString();
            Value = value;
            //LastDateValue = DateTime.UtcNow;
        }

        public SensorValueEntity() { }
        public double Value { get; set; }

    }
}
