﻿using System;

internal static class Universal_8_1_CSharpTemplates
{
    public static string GetProjItemsTemplate()
    {
        string[] textArray1 = new string[] { "<?xml version=\"1.0\" encoding=\"utf-8\"?>", "<Project xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\">", "  <PropertyGroup>", "    <MSBuildAllProjects>$(MSBuildAllProjects);$(MSBuildThisFileFullPath)</MSBuildAllProjects>", "  </PropertyGroup>", "  <ItemGroup>", "{0}", "    <Content Include=\"$(MSBuildThisFileDirectory)Data\\**\" />", "  </ItemGroup>", "</Project>" };
        return string.Join("\r\n", textArray1);
    }

    public static string GetSHProjTemplate()
    {
        string[] textArray1 = new string[] { "<?xml version=\"1.0\" encoding=\"utf-8\"?>", "<Project ToolsVersion=\"12.0\" DefaultTargets=\"Build\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\">", "  <PropertyGroup Label=\"Globals\">", "    <ProjectGuid>d550ebee-7f16-4918-b2c6-7bef519b3973</ProjectGuid>", "  </PropertyGroup>", "  <Import Project=\"$(MSBuildExtensionsPath)\\$(MSBuildToolsVersion)\\Microsoft.Common.props\" Condition=\"Exists('$(MSBuildExtensionsPath)\\$(MSBuildToolsVersion)\\Microsoft.Common.props')\" />", "  <Import Project=\"$(MSBuildExtensionsPath32)\\Microsoft\\VisualStudio\\v$(VisualStudioVersion)\\CodeSharing\\Microsoft.CodeSharing.Common.Default.props\" />", "  <Import Project=\"$(MSBuildExtensionsPath32)\\Microsoft\\VisualStudio\\v$(VisualStudioVersion)\\CodeSharing\\Microsoft.CodeSharing.Common.props\" />", "  <PropertyGroup>", "    <RootNamespace>{1}</RootNamespace>", "  </PropertyGroup>", "  <Import Project=\"{0}\" Label=\"Shared\" />", "  <Import Project=\"$(MSBuildExtensionsPath32)\\Microsoft\\VisualStudio\\v$(VisualStudioVersion)\\CodeSharing\\Microsoft.CodeSharing.CSharp.targets\" />", "</Project>" };
        return string.Join("\r\n", textArray1);
    }

    public static string GetSolutionTemplate()
    {
        string[] textArray1 = new string[] { 
            "Microsoft Visual Studio Solution File, Format Version 12.00", "# Visual Studio 2013", "VisualStudioVersion = 12.0.30203.2", "MinimumVisualStudioVersion = 10.0.40219.1", "Project(\"{{D954291E-2A0B-460D-934E-DC6B0785DB48}}\") = \"{0}.Shared\", \"{0}\\{0}.Shared\\{0}.Shared.shproj\", \"{{D550EBEE-7F16-4918-B2C6-7BEF519B3973}}\"", "EndProject", "Project(\"{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}\") = \"{0}.Windows\", \"{0}\\{0}.Windows\\{0}.Windows.csproj\", \"{{35D6339A-CE46-45E5-A16C-57F576011BFF}}\"", "  ProjectSection(ProjectDependencies) = postProject", "{1}  EndProjectSection", "EndProject", "Project(\"{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}\") = \"{0}.WindowsPhone\", \"{0}\\{0}.WindowsPhone\\{0}.WindowsPhone.csproj\", \"{{D378C147-0ABA-4592-AB27-04F0DB138842}}\"", "  ProjectSection(ProjectDependencies) = postProject", "{2}  EndProjectSection", "EndProject", "{3}", "Global",
            "    GlobalSection(SolutionConfigurationPlatforms) = preSolution", "        Debug|ARM = Debug|ARM", "        Debug|x86 = Debug|x86", "        Master|ARM = Master|ARM", "        Master|x86 = Master|x86", "        Release|ARM = Release|ARM", "        Release|x86 = Release|x86", "    EndGlobalSection", "    GlobalSection(ProjectConfigurationPlatforms) = postSolution", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Debug|ARM.ActiveCfg = Debug|ARM", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Debug|ARM.Build.0 = Debug|ARM", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Debug|ARM.Deploy.0 = Debug|ARM", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Debug|x86.ActiveCfg = Debug|x86", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Debug|x86.Build.0 = Debug|x86", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Debug|x86.Deploy.0 = Debug|x86", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Master|ARM.ActiveCfg = Master|ARM",
            "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Master|ARM.Build.0 = Master|ARM", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Master|ARM.Deploy.0 = Master|ARM", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Master|x86.ActiveCfg = Master|x86", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Master|x86.Build.0 = Master|x86", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Master|x86.Deploy.0 = Master|x86", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Release|ARM.ActiveCfg = Release|ARM", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Release|ARM.Build.0 = Release|ARM", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Release|ARM.Deploy.0 = Release|ARM", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Release|x86.ActiveCfg = Release|x86", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Release|x86.Build.0 = Release|x86", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Release|x86.Deploy.0 = Release|x86", "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Debug|ARM.ActiveCfg = Debug|ARM", "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Debug|ARM.Build.0 = Debug|ARM", "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Debug|ARM.Deploy.0 = Debug|ARM", "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Debug|x86.ActiveCfg = Debug|x86", "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Debug|x86.Build.0 = Debug|x86",
            "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Debug|x86.Deploy.0 = Debug|x86", "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Master|ARM.ActiveCfg = Master|ARM", "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Master|ARM.Build.0 = Master|ARM", "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Master|ARM.Deploy.0 = Master|ARM", "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Master|x86.ActiveCfg = Master|x86", "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Master|x86.Build.0 = Master|x86", "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Master|x86.Deploy.0 = Master|x86", "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Release|ARM.ActiveCfg = Release|ARM", "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Release|ARM.Build.0 = Release|ARM", "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Release|ARM.Deploy.0 = Release|ARM", "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Release|x86.ActiveCfg = Release|x86", "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Release|x86.Build.0 = Release|x86", "{4} EndGlobalSection", "    GlobalSection(SolutionProperties) = preSolution", "        HideSolutionNode = FALSE", "    EndGlobalSection",
            "EndGlobal"
        };
        return string.Join("\r\n", textArray1).Replace("    ", "\t");
    }

    public static string GetSourceBuildSolutionTemplate()
    {
        string[] textArray1 = new string[] { 
            "Microsoft Visual Studio Solution File, Format Version 12.00", "# Visual Studio 2013", "VisualStudioVersion = 12.0.30203.2", "MinimumVisualStudioVersion = 10.0.40219.1", "Project(\"{{D954291E-2A0B-460D-934E-DC6B0785DB48}}\") = \"{0}.Shared\", \"{0}\\{0}.Shared\\{0}.Shared.shproj\", \"{{D550EBEE-7F16-4918-B2C6-7BEF519B3973}}\"", "EndProject", "Project(\"{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}\") = \"{0}.Windows\", \"{0}\\{0}.Windows\\{0}.Windows.csproj\", \"{{35D6339A-CE46-45E5-A16C-57F576011BFF}}\"", "  ProjectSection(ProjectDependencies) = postProject", "{1}  EndProjectSection", "EndProject", "Project(\"{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}\") = \"{0}.WindowsPhone\", \"{0}\\{0}.WindowsPhone\\{0}.WindowsPhone.csproj\", \"{{D378C147-0ABA-4592-AB27-04F0DB138842}}\"", "  ProjectSection(ProjectDependencies) = postProject", "{2}  EndProjectSection", "EndProject", "{3}", "Global",
            "    GlobalSection(SolutionConfigurationPlatforms) = preSolution", "        Debug|ARM = Debug|ARM", "        Debug|x86 = Debug|x86", "        Master|ARM = Master|ARM", "        Master|x86 = Master|x86", "        Release|ARM = Release|ARM", "        Release|x86 = Release|x86", "    EndGlobalSection", "    GlobalSection(ProjectConfigurationPlatforms) = postSolution", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Debug|ARM.ActiveCfg = Debug|ARM", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Debug|ARM.Build.0 = Debug|ARM", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Debug|ARM.Deploy.0 = Debug|ARM", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Debug|x86.ActiveCfg = Debug|x86", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Debug|x86.Build.0 = Debug|x86", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Debug|x86.Deploy.0 = Debug|x86", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Master|ARM.ActiveCfg = Master|ARM",
            "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Master|ARM.Build.0 = Master|ARM", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Master|ARM.Deploy.0 = Master|ARM", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Master|x86.ActiveCfg = Master|x86", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Master|x86.Build.0 = Master|x86", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Master|x86.Deploy.0 = Master|x86", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Release|ARM.ActiveCfg = Release|ARM", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Release|ARM.Build.0 = Release|ARM", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Release|ARM.Deploy.0 = Release|ARM", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Release|x86.ActiveCfg = Release|x86", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Release|x86.Build.0 = Release|x86", "      {{35D6339A-CE46-45E5-A16C-57F576011BFF}}.Release|x86.Deploy.0 = Release|x86", "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Debug|ARM.ActiveCfg = Debug|ARM", "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Debug|ARM.Build.0 = Debug|ARM", "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Debug|ARM.Deploy.0 = Debug|ARM", "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Debug|x86.ActiveCfg = Debug|x86", "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Debug|x86.Build.0 = Debug|x86",
            "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Debug|x86.Deploy.0 = Debug|x86", "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Master|ARM.ActiveCfg = Master|ARM", "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Master|ARM.Build.0 = Master|ARM", "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Master|ARM.Deploy.0 = Master|ARM", "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Master|x86.ActiveCfg = Master|x86", "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Master|x86.Build.0 = Master|x86", "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Master|x86.Deploy.0 = Master|x86", "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Release|ARM.ActiveCfg = Release|ARM", "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Release|ARM.Build.0 = Release|ARM", "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Release|ARM.Deploy.0 = Release|ARM", "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Release|x86.ActiveCfg = Release|x86", "      {{D378C147-0ABA-4592-AB27-04F0DB138842}}.Release|x86.Build.0 = Release|x86", "{4} EndGlobalSection", "    GlobalSection(SolutionProperties) = preSolution", "        HideSolutionNode = FALSE", "    EndGlobalSection",
            "EndGlobal"
        };
        return string.Join("\r\n", textArray1).Replace("    ", "\t");
    }
}

