using System;
using System.Collections.Generic;
using System.Text;

namespace ProgrammerAL.SourceGenerators.JsonSerializerContextRegistrationGenerator;

public static class SourceGenerationHelper
{
    public static string GenerateInterface(in RegistrationToGenerateInfo info)
    {
        return info.GenerateInterfaceDefinitionString();
    }
}
