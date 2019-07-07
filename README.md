# Phema.Named

Register and resolve named dependencies with `Microsoft.Extensions.DepepdencyInjection`

## Usage

```csharp
services.AddNamedSingleton<IDepepdency, DepepdencyA>("A")
  .AddNamedTransient<IDepepdency, DepepdencyB>("B");

provider.GetNamedService<IDependency>("A");
provider.GetRequiredNamedService<IDependency>("B");
```

## Installation [![Nuget](https://img.shields.io/nuget/v/Phema.Named.svg)](https://www.nuget.org/packages/Phema.Named)

```bash
  $> dotnet add package Phema.Named
```
