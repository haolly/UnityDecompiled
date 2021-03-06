﻿namespace UnityEngine
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine.Scripting;

    internal sealed class UnhandledExceptionHandler
    {
        [CompilerGenerated]
        private static UnhandledExceptionEventHandler <>f__mg$cache0;

        private static void HandleUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Exception exceptionObject = args.ExceptionObject as Exception;
            if (exceptionObject != null)
            {
                PrintException("Unhandled Exception: ", exceptionObject);
            }
            NativeUnhandledExceptionHandler();
        }

        [MethodImpl(MethodImplOptions.InternalCall), ThreadAndSerializationSafe]
        private static extern void NativeUnhandledExceptionHandler();
        private static void PrintException(string title, Exception e)
        {
            Debug.LogException(e);
            if (e.InnerException != null)
            {
                PrintException("Inner Exception: ", e.InnerException);
            }
        }

        [RequiredByNativeCode]
        private static void RegisterUECatcher()
        {
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new UnhandledExceptionEventHandler(UnhandledExceptionHandler.HandleUnhandledException);
            }
            AppDomain.CurrentDomain.UnhandledException += <>f__mg$cache0;
        }
    }
}

