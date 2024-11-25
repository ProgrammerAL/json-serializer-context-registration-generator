using System;

namespace ProgrammerAL.SourceGenerators.JsonSerializerContextRegistrationGenerator.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class RegisterJsonSerializationAttribute : Attribute
{
    public RegisterJsonSerializationAttribute(Type jsonSerializerContext)
    {
        JsonSerializerContext = jsonSerializerContext;
    }

    public Type JsonSerializerContext { get; }

    public static class Constants
    {
        public const string AttributeName = nameof(RegisterJsonSerializationAttribute);
        public static string AttributeNameSpace = typeof(RegisterJsonSerializationAttribute).Namespace;
        public static string AttributeFullName = typeof(RegisterJsonSerializationAttribute).FullName;
    }
}
