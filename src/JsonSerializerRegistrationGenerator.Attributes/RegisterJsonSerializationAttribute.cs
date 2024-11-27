using System;
using System.Collections.Immutable;

namespace ProgrammerAL.JsonSerializerRegistrationGenerator.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class RegisterJsonSerializationAttribute : Attribute
{
    public RegisterJsonSerializationAttribute(Type jsonSerializerContext, params string[] genericTypes)
    {
        JsonSerializerContext = jsonSerializerContext;
        GenericTypes = genericTypes.ToImmutableArray();
    }

    public Type JsonSerializerContext { get; }
    public ImmutableArray<string> GenericTypes { get; }

    public static class Constants
    {
        public const string AttributeName = nameof(RegisterJsonSerializationAttribute);
        public static string AttributeNameSpace = typeof(RegisterJsonSerializationAttribute).Namespace;
        public static string AttributeFullName = typeof(RegisterJsonSerializationAttribute).FullName;
    }
}
