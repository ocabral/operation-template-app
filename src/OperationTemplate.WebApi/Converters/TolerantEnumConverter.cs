using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace StoneCo.Buy4.OperationTemplate.WebApi.Converters
{
    /// <summary>
    /// Converts an System.Enum to and from its name string value,
    /// checking whether the value is originally contained in the enum. 
    /// Otherwise, it returns the first value of the enum.
    /// </summary>
    /// <seealso cref="StringEnumConverter" />
    public class TolerantEnumConverter : StringEnumConverter
    {
        /// <summary>
        /// The enum type.
        /// </summary>
        private Type _enumType;

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            Type type = (objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(Nullable<>)) ? Nullable.GetUnderlyingType(objectType) : objectType;
            this._enumType = type;
            return type.IsEnum;
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <exception cref="ArgumentException">Thrown when there is no option in the Enum (an empty Enum) or if the option is not originally contained in the Enum.</exception>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (!Enum.IsDefined(this._enumType, value))
            {
                throw new ArgumentException($"Value '{value}' is not originally contained in the Enum '{this._enumType.FullName}'.", this._enumType.Name);
            }
            else
            {
                writer.WriteValue(value.ToString());
            }
        }
    }
}

