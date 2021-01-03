using FantasyModuleParser.NPC.Models.Action;
using Newtonsoft.Json;
using System;
using System.Diagnostics.Contracts;

namespace FantasyModuleParser.Converters
{
    public class SelectableActionModelConverter : JsonConverter<SelectableActionModel>
    {
        public override SelectableActionModel ReadJson(JsonReader reader, Type objectType, SelectableActionModel existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if(reader != null)
                return (SelectableActionModel)reader.Value;
            return null;
        }

        /// <summary>
        //  {
        //      "$type": "FantasyModuleParser.NPC.Models.Action.SelectableActionModel, FantasyModuleParser",
        //      "Selected": true,
        //      "ActionName": "NoSpecial",
        //      "ActionDescription": "No special weapon resistance"
        //  }
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, SelectableActionModel value, JsonSerializer serializer)
        {
            Contract.Requires(value != null);
            Contract.Requires(writer != null);
            if (value.Selected)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("$type");
                writer.WriteValue(typeof(SelectableActionModel).ToString() + ", FantasyModuleParser");
                writer.WritePropertyName(nameof(value.Selected));
                writer.WriteValue(value.Selected);
                writer.WritePropertyName(nameof(value.ActionName));
                writer.WriteValue(value.ActionName);
                writer.WritePropertyName(nameof(value.ActionDescription));
                writer.WriteValue(value.ActionDescription);
                writer.WriteEndObject();
            }
        }
    }
}