# JSON Serializer Context Registration Code Generator

This project generated C# source code files to automatically add classes to a JsonSerializerContext class. It's additive to the built in System.Text.Json Source Generator that creates code to serialize/deserialize JSON. If you're unfamiliar with source generation in System.Text.Json, Microsoft has a good primer at: https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/source-generation.

But as a recap, the purpose of source generation in System.Text.Json is to use a built-in C# Source Generator to generate code that can serialize/deserialize JSON objects without the use of reflection. The benefits of this are 1) the code generated is much faster than reflection, 2) you are able to use this in AOT code because AOT does not allow reflection.

## The Problem with System.Text.Json Source Generation

The built-in source generator for System.Text.Json Serialization Context works great. That's not the problem. The problem is in how we as developers write the code to use it. The example below shows what happens when you have 4 endpoints, and each has a request and response object to be serialized. The example below has a `[JsonSerializable()]` attribute line for each request and response class to register what code the source generator will generate. As your application grows, this file grows with it. 4 endpoints is...pretty small. A real life application will likely have, I don't know, 50 endpoints? Let's say 50. That's 100 `[JsonSerializable()]` attribute lines above your Serializer Context class. 

```csharp
[JsonSerializable(typeof(WeatherForecastRequest))]
[JsonSerializable(typeof(WeatherForecastResponse))]
[JsonSerializable(typeof(CurrentWeatherRequest))]
[JsonSerializable(typeof(CurrentWeatherResponse))]
[JsonSerializable(typeof(HistoricalWeatherRequest))]
[JsonSerializable(typeof(HistoricalWeatherResponse))]
[JsonSerializable(typeof(TodaysWeatherRequest))]
[JsonSerializable(typeof(TodaysWeatherResponse))]
[JsonSourceGenerationOptions(
    UseStringEnumConverter = true,
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    AllowTrailingCommas = true)]
internal partial class MyAppJsonSerializerContext : JsonSerializerContext
{
}
```

More important than the length of the file, you as a developer have to remember to manually add the endpoints to this file. It's not like you regularly look at this file either. It's off to the side and you only look at it when you need to add/remove a `[JsonSerializable()]` attribute. The built-in System.Text.Json Source Generator requires these to all be in a single file. So you can't even try a trick like creating this file everywhere you create your request/response class code, or something like that.

This project fixes that "problem" by letting you spread out the registrations to multiple files and generates code that combines those registrations into a single file that is then used by the built-in System.Text.Json source generator.

## How to use this in your own code

There are 2 steps you must complete. Step 1 is add some code to opt-in to what code will be gereated by this project. Step 2 is to setup your project to run this project to generate the code. The project in the `~/src/Sample` directory has a fully setup ASP.NET Core project using this project.

### Setp 1: Setup Your Code

1. In your C# project, add a NuGet reference to `ProgrammerAL.JsonSerializerRegistrationGenerator.Attributes`
1. Create a `JsonSerializerContext` class like the `JsonSerializerContext Example` below. It must follow the rules:
    1. Partial Class
    1. Inherits from `JsonSerializerContext`
    1. Add the `[JsonSourceGenerationOptions]` attribute
    1. Add the `[GeneratedJsonSerializerContext]` attribute
1. For each class you want to register, add the `[RegisterJsonSerialization]` attribute to it like in the `Registration Example` below. It must follow the rules:
    1. Is a Class or a Record
    1. The type given to the constructor matches the `JsonSerializerContext` you created previously
1. Repeat the above steps for each JSON Serializer Context you want to create


#### JsonSerializerContext Example:
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

#### Registration Example:
```csharp
[RegisterJsonSerialization(typeof(MyAppJsonSerializerContext))]
public record WeatherForecastRequest(string UtcTime);
```

### Step 2: Run This Project

Now that your code has opted in to code generation, you need to run the .NET Tool `ProgrammerAL.JsonSerializerRegistrationGenerator.Runner` to generate the code files.

You can manually run the tool by following these steps.

1. Install the .NET tool using `dotnet tool install --global ProgrammerAL.JsonSerializerRegistrationGenerator.Runner --version 0.0.1-preview.36`
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

## Step 3: Compile your Code

Now that the code has been updated, and the generator has been automated. Compile your project to make sure this works correctly. One unfortunate issue due to this code being exported to a new file is that the first time you compile, the build will fail. Each compile after that should be successful.

## Why Generate Files? Why not use a Source Generator?

Source Generators are great, I love them. But there is no control over the time they run. For this project to work, we need to guarantee that our Source Generator runs before 

Internally this project uses a source generator to create the code. It then outputs the generated code to a file.


## CLI Options



