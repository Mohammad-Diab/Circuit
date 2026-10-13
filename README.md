# Circuit

Impedance Converter: a small Windows app that converts an RC or RL circuit between its series and parallel forms at a given frequency. Written in C# with WPF on .NET Framework 4.0.

## Features

- Pick the impedance type (**Capacitor** or **Inductor**) and the direction (**Series to parallel** or **Parallel to series**).
- Enter the frequency (Hz, KHz, MHz, GHz), the resistance (Ω, KΩ, MΩ, GΩ) and the capacitor (F, mF, μF, nF) or inductor (H, mH, μH, nH) value.
- Shows the equivalent resistance and capacitance or inductance (Rp and Cp, Rs and Ls, ...) with a fitting unit prefix.
- A **Conversion Lows** tab lists the formulas used.

## Requirements

Windows with .NET Framework 4.0 or later. Windows 8, 10 and 11 already include it.

## Usage

Download `Circuit-v1.0.0.7z` from [Releases](https://github.com/Mohammad-Diab/Circuit/releases), extract it, and run `Circuit.exe`. There is no installer.

The exe is not signed, so Windows SmartScreen may warn the first time you run it: click **More info**, then **Run anyway**.

## Building

```
git clone https://github.com/Mohammad-Diab/Circuit.git
```

Open `Circuit.sln` in Visual Studio and build, or run:

```
MSBuild Circuit.sln /p:Configuration=Release
```

The exe is written to `Circuit\bin\Release`.

The project targets .NET Framework 4.0. Recent Visual Studio versions don't ship its reference assemblies; if the build stops with `MSB3644`, extract the [Microsoft.NETFramework.ReferenceAssemblies.net40](https://www.nuget.org/packages/Microsoft.NETFramework.ReferenceAssemblies.net40) NuGet package and point MSBuild at its `build` folder:

```
MSBuild Circuit.sln /p:Configuration=Release /p:TargetFrameworkRootPath=<package>\build
```

## Author

Mohammad Diab

## License

[Apache 2.0](LICENSE)
