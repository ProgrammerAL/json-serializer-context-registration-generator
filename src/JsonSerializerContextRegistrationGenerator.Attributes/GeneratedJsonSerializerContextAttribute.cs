
using System;

namespace ProgrammerAL.JsonSerializerRegistrationGenerator.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class GeneratedJsonSerializerContextAttribute : Attribute
{
    public static class Constants
    {
        public const string AttributeName = nameof(GeneratedJsonSerializerContextAttribute);
        public static string AttributeNameSpace = typeof(GeneratedJsonSerializerContextAttribute).Namespace;
        public static string AttributeFullName = typeof(GeneratedJsonSerializerContextAttribute).FullName;
    }
}
