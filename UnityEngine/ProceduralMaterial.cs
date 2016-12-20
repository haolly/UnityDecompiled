﻿namespace UnityEngine
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    /// <summary>
    /// <para>Class for ProceduralMaterial handling.</para>
    /// </summary>
    public sealed class ProceduralMaterial : Material
    {
        internal ProceduralMaterial() : base((Material) null)
        {
        }

        /// <summary>
        /// <para>Specifies if a named ProceduralProperty should be cached for efficient runtime tweaking.</para>
        /// </summary>
        /// <param name="inputName"></param>
        /// <param name="value"></param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void CacheProceduralProperty(string inputName, bool value);
        /// <summary>
        /// <para>Clear the Procedural cache.</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void ClearCache();
        /// <summary>
        /// <para>Render a ProceduralMaterial immutable and release the underlying data to decrease the memory footprint.</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void FreezeAndReleaseSourceData();
        /// <summary>
        /// <para>This allows to get a reference to a ProceduralTexture generated by a ProceduralMaterial using its name.</para>
        /// </summary>
        /// <param name="textureName">The name of the ProceduralTexture to get.</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ProceduralTexture GetGeneratedTexture(string textureName);
        /// <summary>
        /// <para>Get generated textures.</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Texture[] GetGeneratedTextures();
        /// <summary>
        /// <para>Get a named Procedural boolean property.</para>
        /// </summary>
        /// <param name="inputName"></param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool GetProceduralBoolean(string inputName);
        /// <summary>
        /// <para>Get a named Procedural color property.</para>
        /// </summary>
        /// <param name="inputName"></param>
        public Color GetProceduralColor(string inputName)
        {
            Color color;
            INTERNAL_CALL_GetProceduralColor(this, inputName, out color);
            return color;
        }

        /// <summary>
        /// <para>Get a named Procedural enum property.</para>
        /// </summary>
        /// <param name="inputName"></param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern int GetProceduralEnum(string inputName);
        /// <summary>
        /// <para>Get a named Procedural float property.</para>
        /// </summary>
        /// <param name="inputName"></param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern float GetProceduralFloat(string inputName);
        /// <summary>
        /// <para>Get an array of descriptions of all the ProceduralProperties this ProceduralMaterial has.</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ProceduralPropertyDescription[] GetProceduralPropertyDescriptions();
        /// <summary>
        /// <para>Get a named Procedural texture property.</para>
        /// </summary>
        /// <param name="inputName"></param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Texture2D GetProceduralTexture(string inputName);
        /// <summary>
        /// <para>Get a named Procedural vector property.</para>
        /// </summary>
        /// <param name="inputName"></param>
        public Vector4 GetProceduralVector(string inputName)
        {
            Vector4 vector;
            INTERNAL_CALL_GetProceduralVector(this, inputName, out vector);
            return vector;
        }

        /// <summary>
        /// <para>Checks if the ProceduralMaterial has a ProceduralProperty of a given name.</para>
        /// </summary>
        /// <param name="inputName"></param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool HasProceduralProperty(string inputName);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void INTERNAL_CALL_GetProceduralColor(ProceduralMaterial self, string inputName, out Color value);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void INTERNAL_CALL_GetProceduralVector(ProceduralMaterial self, string inputName, out Vector4 value);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void INTERNAL_CALL_SetProceduralColor(ProceduralMaterial self, string inputName, ref Color value);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void INTERNAL_CALL_SetProceduralVector(ProceduralMaterial self, string inputName, ref Vector4 value);
        /// <summary>
        /// <para>Checks if a named ProceduralProperty is cached for efficient runtime tweaking.</para>
        /// </summary>
        /// <param name="inputName"></param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool IsProceduralPropertyCached(string inputName);
        /// <summary>
        /// <para>Checks if a given ProceduralProperty is visible according to the values of this ProceduralMaterial's other ProceduralProperties and to the ProceduralProperty's visibleIf expression.</para>
        /// </summary>
        /// <param name="inputName">The name of the ProceduralProperty whose visibility is evaluated.</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool IsProceduralPropertyVisible(string inputName);
        /// <summary>
        /// <para>Triggers an asynchronous rebuild of this ProceduralMaterial's dirty textures.</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void RebuildTextures();
        /// <summary>
        /// <para>Triggers an immediate (synchronous) rebuild of this ProceduralMaterial's dirty textures.</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void RebuildTexturesImmediately();
        /// <summary>
        /// <para>Set a named Procedural boolean property.</para>
        /// </summary>
        /// <param name="inputName"></param>
        /// <param name="value"></param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void SetProceduralBoolean(string inputName, bool value);
        /// <summary>
        /// <para>Set a named Procedural color property.</para>
        /// </summary>
        /// <param name="inputName"></param>
        /// <param name="value"></param>
        public void SetProceduralColor(string inputName, Color value)
        {
            INTERNAL_CALL_SetProceduralColor(this, inputName, ref value);
        }

        /// <summary>
        /// <para>Set a named Procedural enum property.</para>
        /// </summary>
        /// <param name="inputName"></param>
        /// <param name="value"></param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void SetProceduralEnum(string inputName, int value);
        /// <summary>
        /// <para>Set a named Procedural float property.</para>
        /// </summary>
        /// <param name="inputName"></param>
        /// <param name="value"></param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void SetProceduralFloat(string inputName, float value);
        /// <summary>
        /// <para>Set a named Procedural texture property.</para>
        /// </summary>
        /// <param name="inputName"></param>
        /// <param name="value"></param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void SetProceduralTexture(string inputName, Texture2D value);
        /// <summary>
        /// <para>Set a named Procedural vector property.</para>
        /// </summary>
        /// <param name="inputName"></param>
        /// <param name="value"></param>
        public void SetProceduralVector(string inputName, Vector4 value)
        {
            INTERNAL_CALL_SetProceduralVector(this, inputName, ref value);
        }

        /// <summary>
        /// <para>Discard all the queued ProceduralMaterial rendering operations that have not started yet.</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void StopRebuilds();

        /// <summary>
        /// <para>Set or get the update rate in millisecond of the animated substance.</para>
        /// </summary>
        public int animationUpdateRate { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Set or get the Procedural cache budget.</para>
        /// </summary>
        public ProceduralCacheSize cacheSize { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Indicates whether cached data is available for this ProceduralMaterial's textures (only relevant for Cache and DoNothingAndCache loading behaviors).</para>
        /// </summary>
        public bool isCachedDataAvailable { [MethodImpl(MethodImplOptions.InternalCall)] get; }

        /// <summary>
        /// <para>Returns true if FreezeAndReleaseSourceData was called on this ProceduralMaterial.</para>
        /// </summary>
        public bool isFrozen { [MethodImpl(MethodImplOptions.InternalCall)] get; }

        /// <summary>
        /// <para>Should the ProceduralMaterial be generated at load time?</para>
        /// </summary>
        public bool isLoadTimeGenerated { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Check if the ProceduralTextures from this ProceduralMaterial are currently being rebuilt.</para>
        /// </summary>
        public bool isProcessing { [MethodImpl(MethodImplOptions.InternalCall)] get; }

        /// <summary>
        /// <para>Set or get the "Readable" flag for a ProceduralMaterial.</para>
        /// </summary>
        public bool isReadable { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Check if ProceduralMaterials are supported on the current platform.</para>
        /// </summary>
        public static bool isSupported { [MethodImpl(MethodImplOptions.InternalCall)] get; }

        /// <summary>
        /// <para>Get ProceduralMaterial loading behavior.</para>
        /// </summary>
        public ProceduralLoadingBehavior loadingBehavior { [MethodImpl(MethodImplOptions.InternalCall)] get; }

        /// <summary>
        /// <para>Set or get an XML string of "input/value" pairs (setting the preset rebuilds the textures).</para>
        /// </summary>
        public string preset { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Used to specify the Substance engine CPU usage.</para>
        /// </summary>
        public static ProceduralProcessorUsage substanceProcessorUsage { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }
    }
}

