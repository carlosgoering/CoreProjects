Before we start, some well deserved credits 
Huge thanks to Saidur Rahman Akash this medium article helped a lot to make the SecretService (https://medium.com/@imAkash25/hashing-and-salting-passwords-in-c-0ee223f07e20)

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

to generate the project .dll with all references use the following command
dotnet publish -c Release -o ./publish