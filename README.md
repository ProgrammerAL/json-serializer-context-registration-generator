# JSON Serializer Context Registration Code Generator
C# Source Generator to automatically add classes to a JsonSerializerContext class


https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/source-generation

## The Problem with System.Text.Json Source Generation

The source generator for System.Text.Json Serialization Context works great. That's not the problem. The problem is in how we as developers write the code to use it. The example below shows what happens when you have 4 endpoints, and each has a request and response object to be serialized. As your application grows, this file grows with it. 4 endpoints is...pretty small. A real life application will likely have, I don't know, 50 endpoints? Let's say 50. That's 100 `[JsonSerializable()]` attribute lines in the below code snippet. 

More important than the length of the file, you as a developer have to remember to manually add the endpoints to this file. It's not like you regularly look at this file either. it's off to the side. The System.Text.Json Source Generator requires these to all be in a single file. So you can't try a trick like creating this file everywhere you create your request/response class code, or something like that.

```csharp
[JsonSerializable(typeof(WeatherForecastRequest))]
[JsonSerializable(typeof(WeatherForecastResponse))]
[JsonSerializable(typeof(CurrentWeatherRequest))]
[JsonSerializable(typeof(CurrentWeatherResponse))]
[JsonSerializable(typeof(HistoricalWeatherRequest))]
[JsonSerializable(typeof(HistoricalWeatherResponse))]
[JsonSerializable(typeof(TodaysWeatherRequest))]
[JsonSerializable(typeof(TodaysWeatherResponse))]
[JsonSourceGenerationOptions(WriteIndented = true)]
internal partial class MySourceGenerationContext : JsonSerializerContext
{
}
```

This project fixes the problem by letting you spread out the 

## How to use this in your own code

There are 2 steps you must complete. Step 1 is add some code to opt-in to what code will be gereated by this project. Step 2 is to setup your project to run this project to generate the code. The project in the `~/src/Sample` directory has a fully setup ASP.NET Core project using this project.

### Setup Your Code

1. In your code, add a NuGet reference to `TODO`
1. Create a `JsonSerializerContext` class like the `JsonSerializerContext Example` below. It must follow the rules:
  - Partial Class
  - Inherits from `JsonSerializerContext`
  - Add the `[JsonSourceGenerationOptions]` attribute
  - Add the `[GeneratedJsonSerializerContext]` attribute
1. For each class you want to register, add the `[RegisterJsonSerialization]` attribute to it like in the `Registration Example` below. It must follow the rules:
  - Is a Class or a Record
  - The type given to the constructor matches the `JsonSerializerContext` you created previously
1. Repeat the above steps for each JSON Serializer Context you want to create


`JsonSerializerContext Example`:
```csharp
[GeneratedJsonSerializerContext]
[JsonSourceGenerationOptions(
    UseStringEnumConverter = true,
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    AllowTrailingCommas = true)]
internal partial class MyAppJsonSerializerContext : JsonSerializerContext
{
}
```

`Registration Example`:
```csharp
[RegisterJsonSerialization(typeof(MyAppJsonSerializerContext))]
public record RootCheckEndpointResponse(string UtcTime);
```

### Run This Project

Now that your code has opted in to code generation, you need to run the .NET Tool `JsonSerializerContextRegistrationGenerator.Runner` to generate the code files.

You can manually run the tool by following these steps.

1. Install the .NET tool using `dotnet tool install --global JsonSerializerContextRegistrationGenerator.Runner --version 0.0.1-preview.33`
1. Run the .NET Tool

Additionally you can automate these steps and make them run each time the project is built. You can create a script that will install/update the .NET tool, and then run it. The below code snippets show how to enable this. You need a PowerShell script that will install/update/run the .NET tool, and the second snippet goes inside the csproj file to run the PowerShell script before the build starts. A full example of this is in the `~/src/Sample` directory.

PowerShell Script to install/update/run the .NET Tool. Assume this is stored as a local script called `run-json-serializer-registration-code-generation.ps1`:
```console
#Make sure a known version of the .NET Tool is installed
& "dotnet" tool install --global JsonSerializerContextRegistrationGenerator.Runner --version 0.0.1-preview.33

#Make sure we're using the latest version
& "dotnet" tool update --global ProgrammerAL.Tools.CodeUpdater

& "json-serializer-context-registrations-code-generator" --sources "$PSScriptRoot" --output "$PSScriptRoot/Generated"
```

Running the PowerShell script during the build:
```xml
<Target Name="PreBuild" BeforeTargets="PreBuildEvent">
  <Exec Command="pwsh ./run-json-serializer-registration-code-generation.ps1" />
</Target>
```

## Why Generate Files? Why not use a Source Generator?

Source Generators are great, I love them. But there is no control over the time they run. For this project to work, we need to guarantee that our Source Generator runs before 


## CLI Options



