﻿using System.Reflection;
using System.Resources;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

[assembly: AssemblyTitle("Transformerizer")]
[assembly: AssemblyDescription("Transformerizer: Parallel transform library for .NET")]
#if DEBUG

[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyCompany("David Love")]
[assembly: AssemblyProduct("Transformerizer")]
[assembly: AssemblyCopyright("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: NeutralResourcesLanguage("en")]

// Version information for an assembly consists of the following four values.
// We will increase these values in the following way:
//    Major Version : Increased when there is a release that breaks a public api
//    Minor Version : Increased for each non-api-breaking release
//    Build Number : 0 for alpha versions, 1 for beta versions, 2 for release candidates, 3 for releases
//    Revision : Always 0 for release versions, always 1+ for alpha, beta, rc versions to indicate the alpha/beta/rc number

[assembly: AssemblyVersion("1.1.3.0")]
[assembly: AssemblyFileVersion("1.1.3.0")]

// This version number will roughly follow semantic versioning : http://semver.org
// The first three numbers will always match the first the numbers of the version above.

[assembly: AssemblyInformationalVersion("1.1.3")]