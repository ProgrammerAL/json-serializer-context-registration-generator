
#Make sure a known version of the .NET Tool is installed
& "dotnet" tool install --global JsonSerializerContextRegistrationGenerator.Runner --version 0.0.1-preview.33

#Make sure we're using the latest version
& "dotnet" tool update --global ProgrammerAL.Tools.CodeUpdater

& "json-serializer-context-registrations-code-generator" --sources "$PSScriptRoot" --output "$PSScriptRoot/Generated"

