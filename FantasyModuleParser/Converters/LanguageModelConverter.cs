using FantasyModuleParser.NPC.Models.Skills;
using Newtonsoft.Json;
using System;

namespace FantasyModuleParser.Converters
{
    public class LanguageModelConverter : JsonConverter<LanguageModel>
    {
        public override LanguageModel ReadJson(JsonReader reader, Type objectType, LanguageModel existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if(reader != null)
                return (LanguageModel)reader.Value;
            return null;
        }

        public override void WriteJson(JsonWriter writer, LanguageModel value, JsonSerializer serializer)
        {
            if (writer != null && value != null && value.Selected)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("$type");
                writer.WriteValue(typeof(LanguageModel).ToString() + ", FantasyModuleParser");
                writer.WritePropertyName(nameof(value.Selected));
                writer.WriteValue(value.Selected);
                writer.WritePropertyName(nameof(value.Language));
                writer.WriteValue(value.Language);
                writer.WriteEndObject();
            }
        }
    }
}