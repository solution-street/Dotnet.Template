using Newtonsoft.Json;

namespace Dotnet.Template.Server.Core.Extensions;

public static class ObjectExtensions
{
    /// <summary>
    /// Creates a deep clone of an object.
    /// <para>
    /// (Deep clone: Creating a new instance of an object that is a complete, independent copy of the original object, including all objects referenced by it.
    /// This means that any changes made to the cloned object or its referenced objects do not affect the original object.)
    /// </para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static T DeepClone<T>(this T obj)
    {
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));

        var settings = new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize
        };

        var serializedObject = JsonConvert.SerializeObject(obj, settings);
        var clonedObject = JsonConvert.DeserializeObject<T>(serializedObject, settings);

        if (clonedObject == null)
        {
            throw new InvalidOperationException($"Deserialization returned null for type: {typeof(T)}.");
        }

        return clonedObject;
    }
}
