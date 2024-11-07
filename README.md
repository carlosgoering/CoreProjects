# How to Create and Use a Local NuGet Package

## Generating the NuGet Package

Compile your projects and package them into a `.nupkg` file using the following command:

`bash`
dotnet pack --output ./nupkgs

This command will create the .nupkg package in the specified directory (./nupkgs).


## Installing the Package in Other Projects
Add the Local Repository as a Package Source

Add your local package repository to the `NuGet.config` file:

`xml`
<configuration>
  <packageSources>
    <add key="LocalPackages" value="c:\\path\\to\\nupkgs" />
  </packageSources>
</configuration>

## Add the Package Reference

In the project that will consume the package, add the reference to the package using the command:

`bash`
dotnet add package MySharedLibrary --version 1.0.0

This will install the package from your local repository to the consuming project.