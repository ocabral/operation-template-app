using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StoneCo.Buy4.OperationTemplate.WebApi.Converters;
using System.Collections.Generic;

namespace StoneCo.Buy4.OperationTemplate.WebApi.Settings
{
    /// <summary>
    /// Responsible for defining serialization settings.
    /// </summary>
    public static class SerializationSettings
    {
        /// <summary>
        /// Return Json serialization settings.
        /// </summary>
        /// <param name="enumSerializationOptions"></param>
        /// <returns></returns>
        public static JsonSerializerSettings GetJsonSerializationSettings(EnumSerializationOptions enumSerializationOptions = EnumSerializationOptions.Undefined)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                // do not indent child objects when serializing objects.
                Formatting = Formatting.None,

                // do not ignore members where the member value is the same as the member's default value when serializing objects.
                NullValueHandling = NullValueHandling.Include,

                // Dates are written in the ISO 8601 format.
                DateFormatHandling = DateFormatHandling.IsoDateFormat,

                // treat as a UTC. If the datetime object represents a local time, it is converted to a UTC.
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,

                // Dates are parsed to DateTimeOffset
                DateParseHandling = DateParseHandling.DateTimeOffset,

                // Resolves member mappings for a type, camel casing property names.
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            switch (enumSerializationOptions)
            {
                case EnumSerializationOptions.Undefined:
                case EnumSerializationOptions.SerializeAsString:
                    settings.Converters = new List<JsonConverter> { new TolerantEnumConverter() };
                    break;
            }

            return settings;
        }
    }
}
