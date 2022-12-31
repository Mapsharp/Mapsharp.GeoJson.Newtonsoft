using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Mapsharp.GeoJson.Newtonsoft.Converters
{
    public abstract class GeoJsonConverterBase<T> : JsonConverter<T>
    {
        protected NamingStrategy DefaultNamingStrategy { get; private set; } = new CamelCaseNamingStrategy();

        protected TProp ReadRequiredProperty<TProp>(JsonSerializer serializer, JObject jo, string propertyName)
        {
            var jprop = jo.Property(propertyName);

            if (jprop == null)
            {
                throw new FormatException($"The required property {propertyName} could not be found.");
            }

            var prop = serializer.Deserialize<TProp>(jprop.Value.CreateReader());

            if (prop == null)
            {
                throw new FormatException($"The required property {propertyName} could not be parsed.");
            }

            return prop;
        }

        protected string GetPropertyTokenName(string propertyName, JsonSerializer serializer)
        {
            if (serializer.ContractResolver is DefaultContractResolver dcr)
            {
                return dcr.GetResolvedPropertyName(propertyName);
            }

            return DefaultNamingStrategy.GetPropertyName(propertyName, false);
        }

        protected void ThrowIfNotValid(T t)
        {
            if (t == null) throw new ArgumentNullException(nameof(t));

            List<ValidationResult> validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(t);

            if (!Validator.TryValidateObject(t, validationContext, validationResults, true))
            {
                throw new JsonSerializationException(string.Join(", ", validationResults.Select(vr => vr.ErrorMessage).ToArray()));
            }
        }
    }

}
