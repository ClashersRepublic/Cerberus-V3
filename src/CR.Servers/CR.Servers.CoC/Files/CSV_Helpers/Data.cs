using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.Files.CSV_Data;
using CR.Servers.Files.CSV_Reader;
using Newtonsoft.Json;

namespace CR.Servers.CoC.Files.CSV_Helpers
{
    internal class Data : IData
    {
        internal Row Row;
        internal DataTable DataTable;

        public int GlobalId { get; set; }

        public int InstanceId { get; set; }

        public int Type { get; set; }

        internal Data()
        {
            // Data.
        }

        internal Data(Row Row, DataTable DataTable)
        {
            this.Row = Row;
            this.DataTable = DataTable;
            this.Type = DataTable.Index;
            this.InstanceId = DataTable.Datas.Count;
            this.GlobalId = CSV_Helpers.GlobalId.Create(this.Type, this.InstanceId);

            this.Load(Row);
        }

        public void Load(Row row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            var type = GetType();
            var properties = type.GetProperties();
            var table = row.Table;

            foreach (var property in properties)
            {
                if (property.DeclaringType == typeof(Data))
                    continue;

                var column = table.Columns[property.Name];
                var propertyType = property.PropertyType;

                if (propertyType.IsArray)
                {
                    // Calculate how many upgrade levels the logic data has.
                    var lvls = row.End - row.Start;
                    // Base type of the array.
                    var arrayType = propertyType.GetElementType();
                    // Array instance we're going to set the property value to.
                    var array = Array.CreateInstance(arrayType, lvls);

                    var prevStrValue = (string) null;
                    for (int j = 0; j < lvls; j++)
                    {
                        var strValue = column.Data[row.Start + j];

                        // If the data value is empty, we check if we
                        // have a non-empty previous data value to use.
                        if (strValue == string.Empty)
                        {
                            if (prevStrValue != null)
                            {
                                // No need to change the type of the value since its
                                // already a string and the property type is string.
                                if (propertyType == typeof(string))
                                {
                                    array.SetValue(prevStrValue, j);
                                }
                                else
                                {
                                    var value = Convert.ChangeType(prevStrValue, arrayType);
                                    array.SetValue(value, j);
                                }
                            }
                            continue;
                        }
                        // Else if the data is not empty, we use it directly.
                        else
                        {
                            // Mark the current value as the previous value or parent value.
                            prevStrValue = strValue;

                            // Update the current value to the one in the column data.
                            var value = Convert.ChangeType(strValue, arrayType);
                            array.SetValue(value, j);
                        }
                    }

                    property.SetValue(this, array);
                }
                else
                {
                    // Take the first value of the data in column from the row.
                    var strValue = column.Data[row.Start];
                    if (strValue != string.Empty)
                    {
                        // No need to change the type of the value since its
                        // already a string and the property type is string.
                        if (propertyType == typeof(string))
                        {
                            property.SetValue(this, strValue);
                        }
                        else
                        {
                            var value = Convert.ChangeType(strValue, propertyType);
                            property.SetValue(this, value);
                        }
                    }
                }
            }
        }

        internal int GetId()
        {
            return CSV_Helpers.GlobalId.GetID(this.GlobalId);
        }

        internal int GetDataType()
        {
            return this.DataTable.Index;
        }

        internal virtual void Process()
        {
        }
    }

    internal class DataConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Data Data = (Data) value;

            writer.WriteValue(Data?.GlobalId ?? 0);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            int Id = (int) (long) reader.Value;

            if (Id != 0)
            {
                Data Data = CSV.Tables.GetWithGlobalId(Id);

                if (objectType == typeof(Data) || Data.GetType() == objectType)
                {
                    return Data;
                }
#if DEBUG
                //Logging.Error(this.GetType(), "Data is not equals with objectType. Data:" + Data.GetType() + " objectType:" + objectType + ".");
#endif
            }

            return null;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.BaseType == typeof(Data) || objectType == typeof(Data);
        }
    }
}
