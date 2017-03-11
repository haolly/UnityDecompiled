﻿namespace UnityEditor
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    /// <summary>
    /// <para>Helper class for constructing displayable names for objects.</para>
    /// </summary>
    public sealed class ObjectNames
    {
        /// <summary>
        /// <para>Class name of an object.</para>
        /// </summary>
        /// <param name="obj"></param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern string GetClassName(UnityEngine.Object obj);
        /// <summary>
        /// <para>Drag and drop title for an object.</para>
        /// </summary>
        /// <param name="obj"></param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern string GetDragAndDropTitle(UnityEngine.Object obj);
        /// <summary>
        /// <para>Inspector title for an object.</para>
        /// </summary>
        /// <param name="obj"></param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern string GetInspectorTitle(UnityEngine.Object obj);
        [Obsolete("Please use GetInspectorTitle instead")]
        public static string GetPropertyEditorTitle(UnityEngine.Object obj) => 
            GetInspectorTitle(obj);

        internal static string GetTypeName(UnityEngine.Object obj)
        {
            if (obj == null)
            {
                return "Object";
            }
            string path = AssetDatabase.GetAssetPath(obj).ToLower();
            if (path.EndsWith(".unity"))
            {
                return "Scene";
            }
            if (path.EndsWith(".guiskin"))
            {
                return "GUI Skin";
            }
            if (Directory.Exists(AssetDatabase.GetAssetPath(obj)))
            {
                return "Folder";
            }
            if (obj.GetType() == typeof(UnityEngine.Object))
            {
                return (Path.GetExtension(path) + " File");
            }
            return GetClassName(obj);
        }

        /// <summary>
        /// <para>Make a unique name using the provided name as a base. 
        /// 
        /// If the target name is in the provided list of existing names, a unique name is generated by appending the next available numerical increment.</para>
        /// </summary>
        /// <param name="existingNames">A list of pre-existing names.</param>
        /// <param name="name">Desired name to be used as is, or as a base.</param>
        /// <returns>
        /// <para>A name not found in the list of pre-existing names.</para>
        /// </returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern string GetUniqueName(string[] existingNames, string name);
        [Obsolete("Please use NicifyVariableName instead")]
        public static string MangleVariableName(string name) => 
            NicifyVariableName(name);

        /// <summary>
        /// <para>Make a displayable name for a variable.</para>
        /// </summary>
        /// <param name="name"></param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern string NicifyVariableName(string name);
        /// <summary>
        /// <para>Sets the name of an Object.</para>
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void SetNameSmart(UnityEngine.Object obj, string name);
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void SetNameSmartWithInstanceID(int instanceID, string name);
    }
}

