﻿namespace UnityEngine
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine.Scripting;

    internal sealed class DebugLogHandler : ILogHandler
    {
        [MethodImpl(MethodImplOptions.InternalCall), GeneratedByOldBindingsGenerator, ThreadAndSerializationSafe]
        internal static extern void Internal_Log(LogType level, string msg, [Writable] UnityEngine.Object obj);
        [MethodImpl(MethodImplOptions.InternalCall), ThreadAndSerializationSafe, GeneratedByOldBindingsGenerator]
        internal static extern void Internal_LogException(Exception exception, [Writable] UnityEngine.Object obj);
        public void LogException(Exception exception, UnityEngine.Object context)
        {
            Internal_LogException(exception, context);
        }

        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            Internal_Log(logType, string.Format(format, args), context);
        }
    }
}

