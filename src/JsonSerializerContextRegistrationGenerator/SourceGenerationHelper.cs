using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace ProgrammerAL.SourceGenerators.JsonSerializerContextRegistrationGenerator;

public static class SourceGenerationHelper
{
    public static string GenerateSource(in ImmutableArray<RegistrationToGenerateInfo?> infos)
    {
        return "";
        //return info.GenerateInterfaceDefinitionString();
    }
}
