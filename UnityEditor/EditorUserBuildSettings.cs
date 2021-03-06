﻿namespace UnityEditor
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// <para>User build settings for the Editor</para>
    /// </summary>
    public sealed class EditorUserBuildSettings
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static string <xboxOneAdditionalDebugPorts>k__BackingField;
        /// <summary>
        /// <para>Triggered in response to SwitchActiveBuildTarget.</para>
        /// </summary>
        public static System.Action activeBuildTargetChanged;

        /// <summary>
        /// <para>Get the current location for the build.</para>
        /// </summary>
        /// <param name="target"></param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern string GetBuildLocation(BuildTarget target);
        /// <summary>
        /// <para>Returns value for platform specifc Editor setting.</para>
        /// </summary>
        /// <param name="platformName">The name of the platform.</param>
        /// <param name="name">The name of the setting.</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern string GetPlatformSettings(string platformName, string name);
        /// <summary>
        /// <para>Is .NET Native enabled for specific build configuration.
        /// More information - https:msdn.microsoft.comen-uslibrary/dn584397(v=vs.110).aspx.</para>
        /// </summary>
        /// <param name="config">Build configuration.</param>
        /// <returns>
        /// <para>True if .NET Native is enabled for the specific build configuration.</para>
        /// </returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern bool GetWSADotNetNative(WSABuildType config);
        internal static void Internal_ActiveBuildTargetChanged()
        {
            if (activeBuildTargetChanged != null)
            {
                activeBuildTargetChanged();
            }
        }

        /// <summary>
        /// <para>Set a new location for the build.</para>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="location"></param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void SetBuildLocation(BuildTarget target, string location);
        /// <summary>
        /// <para>Set platform specifc Editor setting.</para>
        /// </summary>
        /// <param name="platformName">The name of the platform.</param>
        /// <param name="name">The name of the setting.</param>
        /// <param name="value">Setting value.</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void SetPlatformSettings(string platformName, string name, string value);
        /// <summary>
        /// <para>Enables or Disables .NET Native for specific build configuration.
        /// More information - https:msdn.microsoft.comen-uslibrary/dn584397(v=vs.110).aspx.</para>
        /// </summary>
        /// <param name="config">Build configuration.</param>
        /// <param name="enabled">Is enabled?</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void SetWSADotNetNative(WSABuildType config, bool enabled);
        /// <summary>
        /// <para>Select a new build target to be active.</para>
        /// </summary>
        /// <param name="target"></param>
        /// <returns>
        /// <para>True if the build target was successfully switched, false otherwise (for example, if license checks fail, files are missing, or if the user has cancelled the operation via the UI).</para>
        /// </returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern bool SwitchActiveBuildTarget(BuildTarget target);

        /// <summary>
        /// <para>The currently active build target.</para>
        /// </summary>
        public static BuildTarget activeBuildTarget { [MethodImpl(MethodImplOptions.InternalCall)] get; }

        /// <summary>
        /// <para>DEFINE directives for the compiler.</para>
        /// </summary>
        public static string[] activeScriptCompilationDefines { [MethodImpl(MethodImplOptions.InternalCall)] get; }

        /// <summary>
        /// <para>Enable source-level debuggers to connect.</para>
        /// </summary>
        public static bool allowDebugging { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Android platform options.</para>
        /// </summary>
        public static MobileTextureSubtarget androidBuildSubtarget { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Set which build system to use for building the Android package.</para>
        /// </summary>
        public static AndroidBuildSystem androidBuildSystem { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Is build script only enabled.</para>
        /// </summary>
        public static bool buildScriptsOnly { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Compress files in package.</para>
        /// </summary>
        public static bool compressFilesInPackage { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Build data compressed with PSArc.</para>
        /// </summary>
        public static bool compressWithPsArc { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Start the player with a connection to the profiler.</para>
        /// </summary>
        public static bool connectProfiler { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Enables a development build.</para>
        /// </summary>
        public static bool development { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Enables a Linux headless build.</para>
        /// </summary>
        public static bool enableHeadlessMode { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Are divide by zero's actively checked?</para>
        /// </summary>
        public static bool explicitDivideByZeroChecks { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Are null references actively checked?</para>
        /// </summary>
        public static bool explicitNullChecks { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Export Android Project for use with Android StudioGradle or EclipseADT.</para>
        /// </summary>
        public static bool exportAsGoogleAndroidProject { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Force installation of package, even if error.</para>
        /// </summary>
        public static bool forceInstallation { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Force full optimizations for script complilation in Development builds.</para>
        /// </summary>
        public static bool forceOptimizeScriptCompilation { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Place the built player in the build folder.</para>
        /// </summary>
        public static bool installInBuildFolder { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Scheme with which the project will be run in Xcode.</para>
        /// </summary>
        public static iOSBuildType iOSBuildConfigType { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Create a .cia "download image" for deploying to test kits (3DS).</para>
        /// </summary>
        public static bool n3dsCreateCIAFile { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Build submission materials.</para>
        /// </summary>
        public static bool needSubmissionMaterials { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>PS4 Build Subtarget.</para>
        /// </summary>
        public static PS4BuildSubtarget ps4BuildSubtarget { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Specifies which version of PS4 hardware to target.</para>
        /// </summary>
        public static PS4HardwareTarget ps4HardwareTarget { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>PSM Build Subtarget.</para>
        /// </summary>
        public static PSMBuildSubtarget psmBuildSubtarget { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>PS Vita Build subtarget.</para>
        /// </summary>
        public static PSP2BuildSubtarget psp2BuildSubtarget { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>The currently selected build target group.</para>
        /// </summary>
        public static BuildTargetGroup selectedBuildTargetGroup { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>The currently selected target for a standalone build.</para>
        /// </summary>
        public static BuildTarget selectedStandaloneTarget { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>When building an Xbox One Streaming Install package (makepkg.exe) The layout generation code in Unity will assign each scene and associated assets to individual chunks. Unity will mark scene 0 as being part of the launch range, IE the set of chunks required to launch the game, you may include additional scenes in this launch range if you desire, this specifies a range of scenes (starting at 0) to be included in the launch set. </para>
        /// </summary>
        public static int streamingInstallLaunchRange { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Symlink runtime libraries with an iOS Xcode project.</para>
        /// </summary>
        public static bool symlinkLibraries { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        internal static bool symlinkTrampoline { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>The texture compression type to be used when building.</para>
        /// </summary>
        public static MobileTextureSubtarget tizenBuildSubtarget { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Use prebuilt JavaScript version of Unity engine.</para>
        /// </summary>
        public static bool webGLUsePreBuiltUnityEngine { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Build the webplayer along with the UnityObject.js file (so it doesn't need to be downloaded).</para>
        /// </summary>
        public static bool webPlayerOfflineDeployment { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Select the streaming option for a webplayer build.</para>
        /// </summary>
        public static bool webPlayerStreamed { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Boot mode of a devkit.</para>
        /// </summary>
        public static int wiiUBootMode { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Wii U player debug level.</para>
        /// </summary>
        public static WiiUBuildDebugLevel wiiUBuildDebugLevel { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Built player postprocessing options.</para>
        /// </summary>
        public static WiiUBuildOutput wiiuBuildOutput { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Enable network API.</para>
        /// </summary>
        public static bool wiiUEnableNetAPI { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        public static WSABuildAndRunDeployTarget wsaBuildAndRunDeployTarget { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Generate and reference C# projects from your main solution.</para>
        /// </summary>
        public static bool wsaGenerateReferenceProjects { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Target Windows SDK.</para>
        /// </summary>
        public static WSASDK wsaSDK { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Sets and gets target device type for the application to run on when building to Windows Store platform.</para>
        /// </summary>
        public static WSASubtarget wsaSubtarget { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        public static WSAUWPBuildType wsaUWPBuildType { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Sets and gets target UWP SDK to build Windows Store application against.</para>
        /// </summary>
        public static string wsaUWPSDK { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Xbox Build subtarget.</para>
        /// </summary>
        public static XboxBuildSubtarget xboxBuildSubtarget { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        public static string xboxOneAdditionalDebugPorts
        {
            [CompilerGenerated]
            get => 
                <xboxOneAdditionalDebugPorts>k__BackingField;
            [CompilerGenerated]
            set
            {
                <xboxOneAdditionalDebugPorts>k__BackingField = value;
            }
        }

        /// <summary>
        /// <para>The currently selected Xbox One Deploy Method.</para>
        /// </summary>
        public static XboxOneDeployMethod xboxOneDeployMethod { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Network shared folder path e.g.
        /// MYCOMPUTER\SHAREDFOLDER\.</para>
        /// </summary>
        public static string xboxOneNetworkSharePath { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Windows account username associated with PC share folder.</para>
        /// </summary>
        public static string xboxOneUsername { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }
    }
}

