﻿namespace UnityEditor
{
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEditor.VisualStudioIntegration;
    using UnityEngine;

    public sealed class EditorSettings : UnityEngine.Object
    {
        [CompilerGenerated]
        private static Func<string, string> <>f__am$cache0;

        public static EditorBehaviorMode defaultBehaviorMode { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        public static string externalVersionControl { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        internal static string Internal_ProjectGenerationUserExtensions { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        internal static string Internal_UserGeneratedProjectSuffix { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        public static string[] projectGenerationBuiltinExtensions =>
            SolutionSynchronizer.BuiltinSupportedExtensions.Keys.ToArray<string>();

        public static string projectGenerationRootNamespace { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        public static string[] projectGenerationUserExtensions
        {
            get
            {
                char[] separator = new char[] { ';' };
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = s => s.TrimStart(new char[] { '.', '*' });
                }
                return Enumerable.Select<string, string>(Internal_ProjectGenerationUserExtensions.Split(separator, StringSplitOptions.RemoveEmptyEntries), <>f__am$cache0).ToArray<string>();
            }
            set
            {
                Internal_ProjectGenerationUserExtensions = string.Join(";", value);
            }
        }

        public static SerializationMode serializationMode { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        public static SpritePackerMode spritePackerMode { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        public static int spritePackerPaddingPower { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        public static string unityRemoteCompression { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        public static string unityRemoteDevice { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        public static string unityRemoteJoystickSource { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        public static string unityRemoteResolution { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        [Obsolete("EditorSettings.webSecurityEmulationEnabled is no longer supported, since the Unity Web Player is no longer supported by Unity.")]
        public static bool webSecurityEmulationEnabled { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        [Obsolete("EditorSettings.webSecurityEmulationHostUrl is no longer supported, since the Unity Web Player is no longer supported by Unity.")]
        public static string webSecurityEmulationHostUrl { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }
    }
}

