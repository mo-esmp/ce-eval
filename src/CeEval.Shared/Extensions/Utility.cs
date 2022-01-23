using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CeEval.Shared.Extensions;

/// <summary>
///   The utility class contains common helper methods.
/// </summary>
internal static class Utility
{
    /// <summary>
    ///   Gets the enum member value from <see cref="EnumMemberAttribute"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>A string? .</returns>
    public static string GetEnumMemberValue<T>(T value)
        where T : struct, IConvertible
    {
        return typeof(T)
            .GetTypeInfo()
            .DeclaredMembers
            .SingleOrDefault(x => x.Name == value.ToString())
            ?.GetCustomAttribute<EnumMemberAttribute>(false)
            ?.Value;
    }

    public static TEnum ToEnum<TEnum>(this string value) where TEnum : Enum
    {
        var jsonString = $"'{value.ToLower()}'";
        return JsonConvert.DeserializeObject<TEnum>(jsonString, new StringEnumConverter());
    }

    public static T DeserializeAnonymousType<T>(string json, T anonymousTypeObject, JsonSerializerOptions options = default)
        => JsonSerializer.Deserialize<T>(json, options);

    public static ValueTask<TValue> DeserializeAnonymousTypeAsync<TValue>(Stream stream, TValue anonymousTypeObject, JsonSerializerOptions options = default, CancellationToken cancellationToken = default)
        => JsonSerializer.DeserializeAsync<TValue>(stream, options, cancellationToken);
}