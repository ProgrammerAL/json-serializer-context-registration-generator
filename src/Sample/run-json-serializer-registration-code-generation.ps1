
#Make sure a known version of the .NET Tool is installed
& "dotnet" tool install --global ProgrammerAL.JsonSerializerRegistrationGenerator.Runner --version 1.0.0.46

#Make sure we're using the latest version
& "dotnet" tool update --global ProgrammerAL.JsonSerializerRegistrationGenerator.Runner

& "json-serializer-context-registrations-code-generator" --sources "$PSScriptRoot" --output "$PSScriptRoot/GeneratedCode/JsonSerializerContexRegistrations"

