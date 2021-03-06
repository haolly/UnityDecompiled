﻿namespace UnityEngineInternal
{
    using System;
    using System.Reflection;

    public class ScriptingUtils
    {
        public static Delegate CreateDelegate(Type type, MethodInfo methodInfo) => 
            Delegate.CreateDelegate(type, methodInfo);
    }
}

