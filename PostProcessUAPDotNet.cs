﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEditor.Modules;
using UnityEngine;

internal class PostProcessUAPDotNet : PostProcessUAP
{
    private static string _referenceAssembliesDirectory;
    [CompilerGenerated]
    private static Func<string, string> <>f__am$cache0;
    [CompilerGenerated]
    private static Func<string, string> <>f__am$cache1;
    [CompilerGenerated]
    private static Func<string, string> <>f__am$cache2;

    public PostProcessUAPDotNet(BuildPostProcessArgs args, [Optional, DefaultParameterValue(null)] string stagingArea) : base(args, stagingArea)
    {
    }

    public override void CopyPlayerFiles()
    {
        string[] extensions = new string[] { ".rd.xml", ".dll", ".winmd", ".pdb", ".pri" };
        this.CopyPlayerFiles(extensions);
    }

    public override void CopyTemplate()
    {
        base.CopyTemplate();
        string templateDirectorySource = base.GetTemplateDirectorySource("UAP");
        string templateDirectoryTarget = this.GetTemplateDirectoryTarget();
        FileUtil.CopyDirectoryRecursive(templateDirectorySource, templateDirectoryTarget);
    }

    protected override string GetAlternativeReferenceRewritterMappings()
    {
        string[] textArray1 = new string[] { "System.Xml.Serialization", "System.Collections,System.Collections.NonGeneric", "System.Reflection,System.Reflection.TypeExtensions", "System.IO,System.IO.FileSystem", "System.Net,System.Net.Primitives", "System.Net.Sockets,System.Net.Primitives", "System.Xml,System.Xml.XmlDocument" };
        string str = string.Join(";", textArray1);
        if (<>f__am$cache2 == null)
        {
            <>f__am$cache2 = new Func<string, string>(null, (IntPtr) <GetAlternativeReferenceRewritterMappings>m__2);
        }
        foreach (string str2 in Enumerable.Select<string, string>(PostProcessUAP.UWPReferences, <>f__am$cache2))
        {
            str = str + ";<winmd>," + str2;
        }
        return str;
    }

    protected override string GetAssemblyConverterPlatform()
    {
        return "uap";
    }

    protected override IEnumerable<string> GetLangAssemblies()
    {
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = new Func<string, string>(null, (IntPtr) <GetLangAssemblies>m__0);
        }
        return Enumerable.Select<string, string>(base.GetLangAssemblies(), <>f__am$cache0);
    }

    protected override string GetPlayerFilesSourceDirectory()
    {
        return base.GetPlayerFilesSourceDirectory(Path.Combine("UAP", "dotnet"));
    }

    protected override string GetReferenceAssembliesDirectory()
    {
        if (_referenceAssembliesDirectory == null)
        {
            _referenceAssembliesDirectory = PostProcessWSA.GetReferenceAssembliesDirectory(WSASDK.UWP);
        }
        return _referenceAssembliesDirectory;
    }

    protected override string GetTemplateDirectorySource()
    {
        return ((EditorUserBuildSettings.wsaUWPBuildType != WSAUWPBuildType.XAML) ? Utility.CombinePath(new string[] { base.PlayerPackage, "Templates", "UWP_D3D" }) : base.GetTemplateDirectorySource("Windows81"));
    }

    protected override IEnumerable<string> GetUnityAssemblies()
    {
        return new string[] { @"UAP\UnityEngine.dll", @"UAP\WinRTLegacy.dll" };
    }

    protected override IEnumerable<string> GetUnityPluginOverwrites()
    {
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = new Func<string, string>(null, (IntPtr) <GetUnityPluginOverwrites>m__1);
        }
        return Enumerable.Select<string, string>(base.GetUnityPluginOverwrites(), <>f__am$cache1);
    }

    protected override void RunAssemblyConverterNoMetadata(string assembly)
    {
        string str6;
        string fileName = Utility.CombinePath(base.PlayerPackage, @"Tools\AssemblyConverter.exe");
        string arguments = string.Format("-metadata=0 -platform={0} -lock=\"{1}\" {2}", this.GetAssemblyConverterPlatform(), @"UWP\project.lock.json", assembly);
        HashSet<string> set = new HashSet<string> {
            Path.GetDirectoryName(assembly)
        };
        foreach (Library library in base.Libraries)
        {
            if (library.AnyCpu)
            {
                string item = Utility.CombinePath(base.StagingArea, Path.GetDirectoryName(library.ReferencePath));
                set.Add(item);
            }
        }
        foreach (string str4 in set)
        {
            string str5 = arguments;
            object[] objArray1 = new object[] { str5, " -path=\"", str4, '"' };
            arguments = string.Concat(objArray1);
        }
        if (Utility.RunAndWait(fileName, arguments, out str6, null) != 0)
        {
            throw new UnityException(string.Format("Failed to run assembly converter with command {0}.\n{1}", arguments, str6));
        }
        if (!string.IsNullOrEmpty(str6))
        {
            Debug.LogError(string.Format("Assembly converter: {0}", str6));
        }
    }
}

