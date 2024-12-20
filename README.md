# JSON Serializer Context Registration Code Generator

The purpose of this project is to generate C# source code files to simplify using the System.Text.Json Source Generator. The System.Text.Json Source Generator creates code to serialize/deserialize JSON, and this project is additive to that.

If you're unfamiliar with the System.Text.Json Source Generator, Microsoft has a good primer at: https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/source-generation. But as a recap, the purpose of the System.Text.Json Source Generator is to generate code that can serialize/deserialize JSON objects without the use of reflection. The benefits of this are 1) the code generated is much faster than it is with reflection, and 2) you are able to use this in AOT code because AOT does not allow reflection.

## The Problem with System.Text.Json Source Generation

The built-in source generator for System.Text.Json Serialization Context works great. That's not the problem. The "problem" is in how we as developers write the code to use it. The example below shows what happens when you have an ASP.NET Core project with 4 endpoints, and each has a request and response object to be serialized. The example has a `[JsonSerializable()]` attribute line for each request and response class (8 lines in total). These attributes register those classes so the source generator knows what code to generate. As your application grows, this file grows with it. 4 endpoints is pretty small. A real life application will likely have, I don't know, 50 endpoints? Let's say 50. That's 100 `[JsonSerializable()]` attribute lines above your Serializer Context class. 

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

More important than the length of the file, you as a developer have to remember to manually add the class registrations to this file. If you don't add the registrations, you will get a runtime exception when the app tries to serialize/deserialize the class. Not the best time to find out it was missed. And it's not like you regularly look at this file either. It's off to the side and you only look at it when you need to add/remove a `[JsonSerializable()]` attribute. The built-in System.Text.Json Source Generator requires these to all be in a single file.

This project fixes that "problem" by letting you spread out the registrations to multiple files and generates code that combines those registrations into a single file that is then used by the built-in System.Text.Json Source Generator.

## How to use this in your own code

There are 2 steps you must complete. Step 1 - add code to opt-in to the code generation. Step 2 - setup your project generate the code files. These steps are detailed below. For your reference, the project in the `~/src/Sample` directory has a fully setup ASP.NET Core project using the code generation of this project.

### Step 1: Setup Your Code

1. In your C# project, add a NuGet reference to `ProgrammerAL.JsonSerializerRegistrationGenerator.Attributes`
1. Create your JSON Serializer Context class like in the `JsonSerializerContext Example` below. It must follow the rules:
    1. Be a `partial` class
    1. Inherits from `System.Text.Json.Serialization.JsonSerializerContext`
    1. Add the `[JsonSourceGenerationOptions]` attribute, add any
    1. Add the `[GeneratedJsonSerializerContext]` attribute
        - Note: adding this attribute is the only part that's different from the normal way you would use the System.Text.Json Source Generator
1. For each class you want to register, add the `[RegisterJsonSerialization]` attribute to it like in the `Registration Example` below. It must follow the rules:
    1. Is a Class or a Record
    1. The type given to the constructor matches the `JsonSerializerContext` you created previously
        - Note: When using the source generation of this project, you cannot add `[JsonSerializable()]` registrations to your JSON Serializer Context class
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

### Step 2: Run the .NET Tool From This Project

Now that your code has opted in to code generation, you need to generate the code files using the the .NET Tool `ProgrammerAL.JsonSerializerRegistrationGenerator.Runner`.

You can manually run the tool by following these steps.

1. Install the .NET tool using `dotnet tool install --global ProgrammerAL.JsonSerializerRegistrationGenerator.Runner --version 1.0.0.46`
1. Run the .NET Tool

Additionally you can automate these steps and make them run each time the project is built. You can create a script that will install/update the .NET tool, and then run it. The below PowerShell code snippets show how to enable this. The snippet uses a PowerShell script that will install/update/run the .NET tool, and the second snippet goes inside the csproj file to run the PowerShell script before the build starts. A full example of this is in the `~/src/Sample` directory.

Create a PowerShell Script named `run-json-serializer-registration-code-generation.ps1`:

```console
#Make sure a known version of the .NET Tool is installed
& "dotnet" tool install --global ProgrammerAL.JsonSerializerRegistrationGenerator.Runner --version 0.0.1-preview.38

#Make sure we're using the latest version
& "dotnet" tool update --global ProgrammerAL.JsonSerializerRegistrationGenerator.Runner

& "json-serializer-context-registrations-code-generator" --sources "$PSScriptRoot" --output "$PSScriptRoot/GeneratedCode/JsonSerializerContexRegistrations"
```

Running the PowerShell script during the build:
```xml
<Target Name="PreBuild" BeforeTargets="PreBuildEvent">
  <Exec Command="pwsh ./run-json-serializer-registration-code-generation.ps1" />
</Target>
```

## Step 3: Compile your Code

Now that the code has been updated, and the generator has been automated. Compile your project to make sure this works correctly. One unfortunate issue due to this code being exported to a new file is that the first time you compile, the build will fail. Each compile after that should be successful.

## Generic Registrations

When registering generic classes with the System.Text.Json Source Generator, you need to specify all possible combinations of generic types you may need. For example, if you know your generic will only ever work with numbers, you need to register `int`, `float`, `double`, `decimal`, etc.

The example below is of a single class, but there are 3 possible generic types that are being registered(string, int, and DateTime). The source generator will output a line in the generated JSON Serializer Context class for each of the generic types.

```csharp
[RegisterJsonSerialization(typeof(AppJsonSerializerContext), genericTypes: ["string", "int", "System.DateTime"])]
public record GenericRootCheckEndpointResponse<T>(T UtcTime);
```

To see a more fleshed out example of this, find the `GenericRootCheckEndpoint` and the `MultipleGenericRootEndpoint` classes in the `~/src/Sample` example project.

## Why Generate Files? Why not use a Source Generator?

Source Generators are great, I love them. But there is no control over the order they run. For this project to work, we need to guarantee that our Source Generator runs before the built-in System.Text.Json generator. And unfortunatly, there's no way to do that. There's a ticket open on GitHub if you want to monitor it: https://github.com/dotnet/roslyn/issues/57239. If that ever happens, this project becomes just a single Source Generator.

Internally this project uses a source generator to create the code. It then outputs the generated code to a file. As a developer, you commit the generated file to source control.

### There's a flag to output Source Generator code to a file. Why not use that?

That flag, `EmitCompilerGeneratedFiles`, is global for all source generators in the *.csproj project. If you have a lot of source generators, you end up with unnecessary code added to your codebase you don't need.

## .NET Tool CLI Options

The .NET Tool that generates the code files has 2 required CLI inputs. The are:

- sources, -s
  - Required
  - Path to the root directory of a set of code files
- output, -o
  - Required
  - Path to the directory to output files to

