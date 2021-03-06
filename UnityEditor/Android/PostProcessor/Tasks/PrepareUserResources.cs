﻿namespace UnityEditor.Android.PostProcessor.Tasks
{
    using System;
    using System.IO;
    using System.Threading;
    using UnityEditor;
    using UnityEditor.Android;
    using UnityEditor.Android.PostProcessor;
    using UnityEngine;

    internal class PrepareUserResources : IPostProcessorTask
    {
        public event ProgressHandler OnProgress;

        public void Execute(PostProcessorContext context)
        {
            if (this.OnProgress != null)
            {
                this.OnProgress(this, "Processing user-provided Android resources");
            }
            string str = context.Get<string>("AndroidPluginsPath");
            string str2 = context.Get<string>("TargetLibrariesFolder");
            string path = Path.Combine(str, "res");
            if (Directory.Exists(path))
            {
                Debug.LogWarning("OBSOLETE - Providing Android resources in Assets/Plugins/Android/res is deprecated, please move your resources to an Android Library. See \"Building Plugins for Android\" section of the Manual.");
                this.GenerateAndroidLibraryWithResources(context, path, Path.Combine(str2, "unity-android-resources"));
            }
        }

        private void GenerateAndroidLibraryWithResources(PostProcessorContext context, string sourceDir, string targetDir)
        {
            int num = context.Get<int>("TargetSDKVersion");
            Directory.CreateDirectory(targetDir);
            string path = Path.Combine(targetDir, "res");
            Directory.CreateDirectory(path);
            File.WriteAllText(Path.Combine(targetDir, AndroidLibraries.ProjectPropertiesFileName), $"android.library=true

target=android-{num}");
            File.WriteAllText(Path.Combine(targetDir, "AndroidManifest.xml"), $"<?xml version="1.0" encoding="utf-8"?><manifest xmlns:android="http://schemas.android.com/apk/res/android" package="{PlayerSettings.bundleIdentifier}.resources"
android:versionCode="1" android:versionName="1.0"></manifest>");
            FileUtil.CopyDirectoryRecursiveForPostprocess(sourceDir, path, true);
        }

        public string Name =>
            "Processing resources";
    }
}

