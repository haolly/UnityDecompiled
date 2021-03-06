﻿namespace UnityEditorInternal
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public sealed class ProfilerProperty
    {
        private IntPtr m_Ptr;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ProfilerProperty();
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void Cleanup();
        [MethodImpl(MethodImplOptions.InternalCall), ThreadAndSerializationSafe]
        public extern void Dispose();
        ~ProfilerProperty()
        {
            this.Dispose();
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern AudioProfilerClipInfo[] GetAudioProfilerClipInfo();
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern AudioProfilerDSPInfo[] GetAudioProfilerDSPInfo();
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern AudioProfilerGroupInfo[] GetAudioProfilerGroupInfo();
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern string GetAudioProfilerNameByOffset(int offset);
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern string GetColumn(ProfilerColumn column);
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern string GetTooltip(ProfilerColumn column);
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void InitializeDetailProperty(ProfilerProperty source);
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool Next(bool enterChildren);
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void SetRoot(int frame, ProfilerColumn profilerSortColumn, ProfilerViewType viewType);

        public int depth { [MethodImpl(MethodImplOptions.InternalCall)] get; }

        public bool frameDataReady { [MethodImpl(MethodImplOptions.InternalCall)] get; }

        public string frameFPS { [MethodImpl(MethodImplOptions.InternalCall)] get; }

        public string frameGpuTime { [MethodImpl(MethodImplOptions.InternalCall)] get; }

        public string frameTime { [MethodImpl(MethodImplOptions.InternalCall)] get; }

        public bool HasChildren { [MethodImpl(MethodImplOptions.InternalCall)] get; }

        public int[] instanceIDs { [MethodImpl(MethodImplOptions.InternalCall)] get; }

        public bool onlyShowGPUSamples { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        public string propertyPath { [MethodImpl(MethodImplOptions.InternalCall)] get; }
    }
}

