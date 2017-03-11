﻿namespace UnityEditorInternal
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [StructLayout(LayoutKind.Sequential)]
    public struct NativeProfilerTimeline_InitializeArgs
    {
        public Color[] profilerColors;
        public Color nativeAllocationColor;
        public float ghostAlpha;
        public float nonSelectedAlpha;
        public IntPtr guiStyle;
        public float lineHeight;
        public float textFadeOutWidth;
        public float textFadeStartWidth;
        public void Reset()
        {
            this.profilerColors = null;
            this.nativeAllocationColor = Color.clear;
            this.ghostAlpha = 0f;
            this.nonSelectedAlpha = 0f;
            this.guiStyle = IntPtr.Zero;
            this.lineHeight = 0f;
            this.textFadeOutWidth = 0f;
            this.textFadeStartWidth = 0f;
        }
    }
}
