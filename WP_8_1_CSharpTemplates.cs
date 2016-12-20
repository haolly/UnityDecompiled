﻿using System;

internal static class WP_8_1_CSharpTemplates
{
    public static string GetSolutionTemplate()
    {
        string[] textArray1 = new string[] { 
            "Microsoft Visual Studio Solution File, Format Version 12.00", "# Visual Studio 2013", "VisualStudioVersion = 12.0.21113.0", "MinimumVisualStudioVersion = 10.0.40219.1", "Project(\"{{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}\") = \"{0}\", \"{0}\\{0}.csproj\", \"{{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}\"", "  ProjectSection(ProjectDependencies) = postProject", "{1}  EndProjectSection", "EndProject", "{2}", "Global", "    GlobalSection(SolutionConfigurationPlatforms) = preSolution", "        Debug|ARM = Debug|ARM", "        Debug|x86 = Debug|x86", "        Master|ARM = Master|ARM", "        Master|x86 = Master|x86", "        Release|ARM = Release|ARM",
            "        Release|x86 = Release|x86", "    EndGlobalSection", "    GlobalSection(ProjectConfigurationPlatforms) = postSolution", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Debug|ARM.ActiveCfg = Debug|ARM", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Debug|ARM.Build.0 = Debug|ARM", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Debug|ARM.Deploy.0 = Debug|ARM", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Debug|x86.ActiveCfg = Debug|x86", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Debug|x86.Build.0 = Debug|x86", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Debug|x86.Deploy.0 = Debug|x86", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Master|ARM.ActiveCfg = Master|ARM", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Master|ARM.Build.0 = Master|ARM", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Master|ARM.Deploy.0 = Master|ARM", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Master|x86.ActiveCfg = Master|x86", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Master|x86.Build.0 = Master|x86", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Master|x86.Deploy.0 = Master|x86", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Release|ARM.ActiveCfg = Release|ARM",
            "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Release|ARM.Build.0 = Release|ARM", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Release|ARM.Deploy.0 = Release|ARM", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Release|x86.ActiveCfg = Release|x86", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Release|x86.Build.0 = Release|x86", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Release|x86.Deploy.0 = Release|x86", "{3} EndGlobalSection", "    GlobalSection(SolutionProperties) = preSolution", "        HideSolutionNode = FALSE", "    EndGlobalSection", "EndGlobal"
        };
        return string.Join("\r\n", textArray1).Replace("    ", "\t");
    }

    public static string GetSourceBuildSolutionTemplate()
    {
        string[] textArray1 = new string[] { 
            "Microsoft Visual Studio Solution File, Format Version 12.00", "# Visual Studio 2013", "VisualStudioVersion = 12.0.12.0.21113.0", "MinimumVisualStudioVersion = 10.0.40219.1", "Project(\"{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}\") = \"{0}\", \"{0}\\{0}.csproj\", \"{{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}\"", "  ProjectSection(ProjectDependencies) = postProject", "        {{B6936B32-2A3B-4BD6-8799-A7EE3C02E72E}} = {{B6936B32-2A3B-4BD6-8799-A7EE3C02E72E}}", "{1}  EndProjectSection", "EndProject", "{2}", "Global", "    GlobalSection(SolutionConfigurationPlatforms) = preSolution", "        Debug|ARM = Debug|ARM", "        Debug|x86 = Debug|x86", "        Master|ARM = Master|ARM", "        Master|x86 = Master|x86",
            "        Release|ARM = Release|ARM", "        Release|x86 = Release|x86", "    EndGlobalSection", "    GlobalSection(ProjectConfigurationPlatforms) = postSolution", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Debug|ARM.ActiveCfg = Debug|ARM", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Debug|ARM.Build.0 = Debug|ARM", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Debug|ARM.Deploy.0 = Debug|ARM", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Debug|x86.ActiveCfg = Debug|x86", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Debug|x86.Build.0 = Debug|x86", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Debug|x86.Deploy.0 = Debug|x86", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Master|ARM.ActiveCfg = Master|ARM", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Master|ARM.Build.0 = Master|ARM", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Master|ARM.Deploy.0 = Master|ARM", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Master|x86.ActiveCfg = Master|x86", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Master|x86.Build.0 = Master|x86", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Master|x86.Deploy.0 = Master|x86",
            "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Release|ARM.ActiveCfg = Release|ARM", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Release|ARM.Build.0 = Release|ARM", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Release|ARM.Deploy.0 = Release|ARM", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Release|x86.ActiveCfg = Release|x86", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Release|x86.Build.0 = Release|x86", "        {{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}.Release|x86.Deploy.0 = Release|x86", "        {{B6936B32-2A3B-4BD6-8799-A7EE3C02E72E}}.Debug|ARM.ActiveCfg = Debug|ARM", "        {{B6936B32-2A3B-4BD6-8799-A7EE3C02E72E}}.Debug|ARM.Build.0 = Debug|ARM", "        {{B6936B32-2A3B-4BD6-8799-A7EE3C02E72E}}.Debug|x86.ActiveCfg = Debug|Win32", "        {{B6936B32-2A3B-4BD6-8799-A7EE3C02E72E}}.Debug|x86.Build.0 = Debug|Win32", "        {{B6936B32-2A3B-4BD6-8799-A7EE3C02E72E}}.Master|ARM.ActiveCfg = Master|ARM", "        {{B6936B32-2A3B-4BD6-8799-A7EE3C02E72E}}.Master|ARM.Build.0 = Master|ARM", "        {{B6936B32-2A3B-4BD6-8799-A7EE3C02E72E}}.Master|x86.ActiveCfg = Master|Win32", "        {{B6936B32-2A3B-4BD6-8799-A7EE3C02E72E}}.Master|x86.Build.0 = Master|Win32", "        {{B6936B32-2A3B-4BD6-8799-A7EE3C02E72E}}.Release|ARM.ActiveCfg = Release|ARM", "        {{B6936B32-2A3B-4BD6-8799-A7EE3C02E72E}}.Release|ARM.Build.0 = Release|ARM",
            "        {{B6936B32-2A3B-4BD6-8799-A7EE3C02E72E}}.Release|x86.ActiveCfg = Release|Win32", "        {{B6936B32-2A3B-4BD6-8799-A7EE3C02E72E}}.Release|x86.Build.0 = Release|Win32", "{3}    EndGlobalSection", "    GlobalSection(SolutionProperties) = preSolution", "        HideSolutionNode = FALSE", "    EndGlobalSection", "EndGlobal"
        };
        return string.Join("\r\n", textArray1).Replace("    ", "\t");
    }

    public static string GetVSProjTemplate()
    {
        string[] textArray1 = new string[] { 
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>", "<Project ToolsVersion=\"12.0\" DefaultTargets=\"Build\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\">", "  <Import Project=\"{4}\" />", "  <Import Project=\"$(MSBuildExtensionsPath)\\$(MSBuildToolsVersion)\\Microsoft.Common.props\" Condition=\"Exists('$(MSBuildExtensionsPath)\\$(MSBuildToolsVersion)\\Microsoft.Common.props')\" />", "  <PropertyGroup>", "    <Configuration Condition=\" '$(Configuration)' == '' \">Debug</Configuration>", "    <Platform Condition=\" '$(Platform)' == '' \">x86</Platform>", "    <ProjectGuid>{{9A513B19-3E02-41BF-8968-58BC58B0A6B3}}</ProjectGuid>", "    <OutputType>AppContainerExe</OutputType>", "    <AppDesignerFolder>Properties</AppDesignerFolder>", "    <RootNamespace>{6}</RootNamespace>", "    <AssemblyName>{7}</AssemblyName>", "    <DefaultLanguage>en-US</DefaultLanguage>", "    <TargetPlatformVersion>8.1</TargetPlatformVersion>", "    <MinimumVisualStudioVersion>12</MinimumVisualStudioVersion>", "    <FileAlignment>512</FileAlignment>",
            "    <ProjectTypeGuids>{{76F1466A-8B6D-4E39-A767-685A06062A39}};{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}</ProjectTypeGuids>", "    <PackageCertificateKeyFile>{0}</PackageCertificateKeyFile>", "    <PlatformShortName>$(Platform)</PlatformShortName>", "    {2}", "    <AllowedReferenceRelatedFileExtensions>", "      $(AllowedReferenceRelatedFileExtensions);", "      _WP81_$(Platform)_$(Configuration)_dotnet.pdb;", "      .pdb;", "    </AllowedReferenceRelatedFileExtensions>", "  </PropertyGroup>", "  <PropertyGroup Condition=\"'$(Configuration)|$(Platform)' == 'Debug|ARM'\">", "    <DebugSymbols>true</DebugSymbols>", @"    <OutputPath>bin\ARM\Debug\</OutputPath>", "    <DefineConstants>DEBUG;TRACE;NETFX_CORE;UNITY_METRO;UNITY_WSA;UNITY_WP_8_1</DefineConstants>", "    <NoWarn>;2008</NoWarn>", "    <DebugType>full</DebugType>",
            "    <PlatformTarget>ARM</PlatformTarget>", "    <UseVSHostingProcess>false</UseVSHostingProcess>", "    <ErrorReport>prompt</ErrorReport>", "    <Prefer32Bit>true</Prefer32Bit>", "  </PropertyGroup>", "  <PropertyGroup Condition=\"'$(Configuration)|$(Platform)' == 'Release|ARM'\">", @"    <OutputPath>bin\ARM\Release\</OutputPath>", "    <DefineConstants>TRACE;NETFX_CORE;UNITY_METRO;UNITY_WSA;UNITY_WP_8_1</DefineConstants>", "    <Optimize>true</Optimize>", "    <NoWarn>;2008</NoWarn>", "    <DebugType>pdbonly</DebugType>", "    <PlatformTarget>ARM</PlatformTarget>", "    <UseVSHostingProcess>false</UseVSHostingProcess>", "    <ErrorReport>prompt</ErrorReport>", "    <Prefer32Bit>true</Prefer32Bit>", "  </PropertyGroup>",
            "  <PropertyGroup Condition=\"'$(Configuration)|$(Platform)' == 'Debug|x86'\">", "    <DebugSymbols>true</DebugSymbols>", @"    <OutputPath>bin\x86\Debug\</OutputPath>", "    <DefineConstants>DEBUG;TRACE;NETFX_CORE;UNITY_METRO;UNITY_WSA;UNITY_WP_8_1</DefineConstants>", "    <NoWarn>;2008</NoWarn>", "    <DebugType>full</DebugType>", "    <PlatformTarget>x86</PlatformTarget>", "    <UseVSHostingProcess>false</UseVSHostingProcess>", "    <ErrorReport>prompt</ErrorReport>", "    <Prefer32Bit>true</Prefer32Bit>", "  </PropertyGroup>", "  <PropertyGroup Condition=\"'$(Configuration)|$(Platform)' == 'Release|x86'\">", @"    <OutputPath>bin\x86\Release\</OutputPath>", "    <DefineConstants>TRACE;NETFX_CORE;UNITY_METRO;UNITY_WSA;UNITY_WP_8_1</DefineConstants>", "    <Optimize>true</Optimize>", "    <NoWarn>;2008</NoWarn>",
            "    <DebugType>pdbonly</DebugType>", "    <PlatformTarget>x86</PlatformTarget>", "    <UseVSHostingProcess>false</UseVSHostingProcess>", "    <ErrorReport>prompt</ErrorReport>", "    <Prefer32Bit>true</Prefer32Bit>", "  </PropertyGroup>", "  <PropertyGroup Condition=\"'$(Configuration)|$(Platform)' == 'Master|ARM'\">", @"    <OutputPath>bin\ARM\Master\</OutputPath>", "    <DefineConstants>TRACE;NETFX_CORE;UNITY_METRO;UNITY_WSA;UNITY_WP_8_1</DefineConstants>", "    <Optimize>true</Optimize>", "    <NoWarn>;2008</NoWarn>", "    <DebugType>pdbonly</DebugType>", "    <PlatformTarget>ARM</PlatformTarget>", "    <UseVSHostingProcess>false</UseVSHostingProcess>", "    <ErrorReport>prompt</ErrorReport>", "    <Prefer32Bit>true</Prefer32Bit>",
            "  </PropertyGroup>", "  <PropertyGroup Condition=\"'$(Configuration)|$(Platform)' == 'Master|x86'\">", @"    <OutputPath>bin\x86\Master\</OutputPath>", "    <DefineConstants>TRACE;NETFX_CORE;UNITY_METRO;UNITY_WSA;UNITY_WP_8_1</DefineConstants>", "    <Optimize>true</Optimize>", "    <NoWarn>;2008</NoWarn>", "    <DebugType>pdbonly</DebugType>", "    <PlatformTarget>x86</PlatformTarget>", "    <UseVSHostingProcess>false</UseVSHostingProcess>", "    <ErrorReport>prompt</ErrorReport>", "    <Prefer32Bit>true</Prefer32Bit>", "  </PropertyGroup>", "", "  <ItemGroup>", "    <AppxManifest Include=\"Package.appxmanifest\">", "    <SubType>Designer</SubType>",
            "    </AppxManifest>", "  </ItemGroup>", "", "  <ItemGroup>", "{1}    <Content Include=\"Data\\**\" />", "  </ItemGroup>", "{5}", "  <ItemGroup>", "    <SDKReference Include=\"Microsoft.VCLibs, Version=12.0\">", "      <Name>Microsoft Visual C++ 2013 Runtime Package for Windows</Name>", "    </SDKReference>", "  </ItemGroup>", "", "  <ItemGroup>", "", "    <Reference Include=\"UnityPlayer, Version=255.255.255.255, Culture=neutral, processorArchitecture=MSIL\">",
            "      <SpecificVersion>False</SpecificVersion>", @"      <HintPath>..\Players\$(PlatformTarget)\$(Configuration)\UnityPlayer.winmd</HintPath>", "      <Implementation>UnityPlayer.dll</Implementation>", "    </Reference>", "", "    <Reference Include=\"BridgeInterface, Version=255.255.255.255, Culture=neutral, processorArchitecture=MSIL\">", "      <SpecificVersion>False</SpecificVersion>", @"      <HintPath>..\Players\$(PlatformTarget)\$(Configuration)\BridgeInterface.winmd</HintPath>", "      <Implementation>BridgeInterface.dll</Implementation>", "    </Reference>", "", "    <Reference Include=\"UnityEngineDelegates, Version=255.255.255.255, Culture=neutral, processorArchitecture=MSIL\">", "      <SpecificVersion>False</SpecificVersion>", @"      <HintPath>..\Players\$(PlatformTarget)\$(Configuration)\UnityEngineDelegates.winmd</HintPath>", "      <Implementation>UnityEngineDelegates.dll</Implementation>", "    </Reference>",
            "", "    <Reference Include=\"WinRTBridge\">", @"      <HintPath>..\Players\$(PlatformTarget)\$(Configuration)\WinRTBridge.winmd</HintPath>", "    </Reference>", "    <Reference Include=\"UnityEngineProxy\">", @"      <HintPath>..\Players\$(PlatformTarget)\$(Configuration)\UnityEngineProxy.dll</HintPath>", "    </Reference>", "  </ItemGroup>", "", "  <PropertyGroup Condition=\" '$(VisualStudioVersion)' == '' or '$(VisualStudioVersion)' &lt; '12.0' \">", "    <VisualStudioVersion>12.0</VisualStudioVersion>", "  </PropertyGroup>", "", "  <PropertyGroup Condition=\" '$(TargetPlatformIdentifier)' == '' \">", "    <TargetPlatformIdentifier>WindowsPhoneApp</TargetPlatformIdentifier>", "  </PropertyGroup>",
            "", "{3}", "  <Import Project=\"$(MSBuildExtensionsPath)\\Microsoft\\WindowsXaml\\v$(VisualStudioVersion)\\Microsoft.Windows.UI.Xaml.CSharp.targets\" />", "  <ItemGroup>", "    <UnprocessedFile Include=\"$(ProjectDir)Unprocessed\\*\" />", "  </ItemGroup>", "  <!-- To modify your build process, add your task inside one of the targets below and uncomment it. ", "   Other similar extension points exist, see Microsoft.Common.targets.", "  <Target Name=\"BeforeBuild\">", "  </Target>", "  -->", "{8}", "</Project>"
        };
        return string.Join("\r\n", textArray1);
    }

    public static string GetVSProjUserPropsTemplate()
    {
        string[] textArray1 = new string[] { "<?xml version=\"1.0\" encoding=\"utf-8\"?>", "<Project ToolsVersion=\"12.0\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\">", "  <PropertyGroup Condition=\"'$(Configuration)|$(Platform)' == 'Debug|x86'\">", "    <DebugEngines>{92EF0900-2251-11D2-B72E-0000F87572EF}</DebugEngines>", "    <EnableUnmanagedDebugging>true</EnableUnmanagedDebugging>", "  </PropertyGroup>", "  <PropertyGroup Condition=\"'$(Configuration)|$(Platform)' == 'Release|x86'\">", "    <DebugEngines>{92EF0900-2251-11D2-B72E-0000F87572EF}</DebugEngines>", "    <EnableUnmanagedDebugging>true</EnableUnmanagedDebugging>", "  </PropertyGroup>", "</Project>" };
        return string.Join("\r\n", textArray1);
    }
}

