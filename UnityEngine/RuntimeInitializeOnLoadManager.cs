﻿namespace UnityEngine
{
    using System;
    using System.Runtime.CompilerServices;

    internal sealed class RuntimeInitializeOnLoadManager
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void UpdateMethodExecutionOrders(int[] changedIndices, int[] changedOrder);

        internal static string[] dontStripClassNames { [MethodImpl(MethodImplOptions.InternalCall)] get; }

        internal static RuntimeInitializeMethodInfo[] methodInfos { [MethodImpl(MethodImplOptions.InternalCall)] get; }
    }
}

