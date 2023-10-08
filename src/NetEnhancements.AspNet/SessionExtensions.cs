using System.Text.Json;
using Microsoft.AspNetCore.Http;
using NetEnhancements.Util.Json;

namespace NetEnhancements.AspNet
{
    public static class SessionExtensions
    {
        private static readonly JsonSerializerOptions SerializerOptions;

        static SessionExtensions()
        {
            SerializerOptions = new JsonSerializerOptions
            {
                Converters =
                {
                    new DateOnlyConverter(),
                    new TimeOnlyConverter(),
                }
            };
        }

        /// <summary>
        /// Reads a JSON-serialized object from session key.
        /// </summary>
        public static TModel? ReadObject<TModel>(this ISession session, string key)
        {
            var json = session.GetString(key);

            return string.IsNullOrWhiteSpace(json)
                ? default
                : JsonSerializer.Deserialize<TModel>(json, SerializerOptions);
        }

        /// <summary>
        /// Writes a JSON-serialized string to a session key.
        /// </summary>
        public static void WriteObject<TModel>(this ISession session, string key, TModel model)
        {
            session.SetString(key, JsonSerializer.Serialize(model, SerializerOptions));
        }
    }
}
