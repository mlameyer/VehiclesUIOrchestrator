using System.Text.Json;
using System.Text.Json.Serialization;

namespace VehiclesUIOrchestrator.Managers.Models
{
    public record class Vehicles
    {
        public int Count { get; set; }
        public string? Message { get; set; }
        public string? SearchCriteria { get; set; }
        public List<Result> Results { get; set; } = new List<Result>();
    }

    public record class Result
    {
        [JsonConverter(typeof(FromIntToStringJsonConverter))]
        public string? Country { get; set; }
        public string? Mfr_CommonName { get; set; }

        [JsonConverter(typeof(FromStringToIntJsonConverter))]
        public int Mfr_ID { get; set; }
        public string? Mfr_Name { get; set; }
        public List<VehicleType> VehicleTypes { get; set; } = new List<VehicleType>();
    }

    public record class VehicleType
    {
        public bool IsPrimary { get; set; }
        [JsonConverter(typeof(TrimWhiteSpace))]
        public string? Name { get; set; }
    }

    internal class TrimWhiteSpace : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt32().ToString();
            }
            else
            {
                return reader.GetString().Trim();
            }
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }

    internal class FromIntToStringJsonConverter : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt32().ToString();
            } else
            {
                return reader.GetString();
            }
        }
                
        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }

    internal class FromStringToIntJsonConverter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                if(Int32.TryParse(reader.GetString(), out int result))
                {
                    return result;
                }

                return 0;
            }
            else
            {
                return reader.GetInt32();
            }
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
