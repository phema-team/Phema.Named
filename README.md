# Phema.Named

Register and resolve named dependencies with `Microsoft.Extensions.DepepdencyInjection`

## Usage

```csharp
services.AddSingleton<IDepepdency, DepepdencyA>("A")
  .AddTransient<IDepepdency, DepepdencyB>("B");

provider.GetService<IDependency>("A");
provider.GetRequiredService<IDependency>("B");
```

## Installation [![Nuget](https://img.shields.io/nuget/v/Phema.Named.svg)](https://www.nuget.org/packages/Phema.Named)

```bash
  $> dotnet add package Phema.Named
```
