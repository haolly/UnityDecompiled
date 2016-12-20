﻿namespace UnityEngine.WSA
{
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// <para>Cursor API for Windows Store Apps.</para>
    /// </summary>
    public sealed class Cursor
    {
        /// <summary>
        /// <para>Set a custom cursor.</para>
        /// </summary>
        /// <param name="id">The cursor resource id.</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void SetCustomCursor(uint id);
    }
}

