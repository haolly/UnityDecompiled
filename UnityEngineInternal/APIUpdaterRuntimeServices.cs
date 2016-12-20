﻿namespace UnityEngineInternal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public sealed class APIUpdaterRuntimeServices
    {
        [CompilerGenerated]
        private static Func<Assembly, IEnumerable<System.Type>> <>f__am$cache0;
        private static IList<System.Type> ComponentsFromUnityEngine;

        static APIUpdaterRuntimeServices()
        {
            System.Type type = typeof(Component);
            ComponentsFromUnityEngine = Enumerable.ToList<System.Type>(Enumerable.Where<System.Type>(type.Assembly.GetTypes(), new Func<System.Type, bool>(type, (IntPtr) type.IsAssignableFrom)));
        }

        [Obsolete("AddComponent(string) has been deprecated. Use GameObject.AddComponent<T>() / GameObject.AddComponent(Type) instead.\nAPI Updater could not automatically update the original call to AddComponent(string name), because it was unable to resolve the type specified in parameter 'name'.\nInstead, this call has been replaced with a call to APIUpdaterRuntimeServices.AddComponent() so you can try to test your game in the editor.\nIn order to be able to build the game, replace this call (APIUpdaterRuntimeServices.AddComponent()) with a call to GameObject.AddComponent<T>() / GameObject.AddComponent(Type).")]
        public static Component AddComponent(GameObject go, string sourceInfo, string name)
        {
            object[] args = new object[] { name };
            Debug.LogWarningFormat("Performing a potentially slow search for component {0}.", args);
            System.Type componentType = ResolveType(name, Assembly.GetCallingAssembly(), sourceInfo);
            return ((componentType != null) ? go.AddComponent(componentType) : null);
        }

        private static bool IsMarkedAsObsolete(System.Type t)
        {
            return Enumerable.Any<object>(t.GetCustomAttributes(typeof(ObsoleteAttribute), false));
        }

        private static System.Type ResolveType(string name, Assembly callingAssembly, string sourceInfo)
        {
            <ResolveType>c__AnonStorey0 storey = new <ResolveType>c__AnonStorey0 {
                name = name
            };
            System.Type type = Enumerable.FirstOrDefault<System.Type>(ComponentsFromUnityEngine, new Func<System.Type, bool>(storey, (IntPtr) this.<>m__0));
            if (type != null)
            {
                object[] objArray1 = new object[] { storey.name, sourceInfo };
                Debug.LogWarningFormat("[{1}] Type '{0}' found in UnityEngine, consider replacing with go.AddComponent<{0}>();", objArray1);
                return type;
            }
            System.Type type3 = callingAssembly.GetType(storey.name);
            if (type3 != null)
            {
                object[] objArray2 = new object[] { type3.FullName, sourceInfo };
                Debug.LogWarningFormat("[{1}] Component type '{0}' found on caller assembly. Consider replacing the call method call with: AddComponent<{0}>()", objArray2);
                return type3;
            }
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = new Func<Assembly, IEnumerable<System.Type>>(null, (IntPtr) <ResolveType>m__0);
            }
            type3 = Enumerable.SingleOrDefault<System.Type>(Enumerable.SelectMany<Assembly, System.Type>(AppDomain.CurrentDomain.GetAssemblies(), <>f__am$cache0), new Func<System.Type, bool>(storey, (IntPtr) this.<>m__1));
            if (type3 != null)
            {
                object[] objArray3 = new object[] { type3.FullName, type3.Assembly.Location, sourceInfo };
                Debug.LogWarningFormat("[{2}] Component type '{0}' found on assembly {1}. Consider replacing the call method with: AddComponent<{0}>()", objArray3);
                return type3;
            }
            object[] args = new object[] { storey.name, sourceInfo };
            Debug.LogErrorFormat("[{1}] Component Type '{0}' not found.", args);
            return null;
        }

        [CompilerGenerated]
        private sealed class <ResolveType>c__AnonStorey0
        {
            internal string name;

            internal bool <>m__0(System.Type t)
            {
                return (((t.Name == this.name) || (t.FullName == this.name)) && !APIUpdaterRuntimeServices.IsMarkedAsObsolete(t));
            }

            internal bool <>m__1(System.Type t)
            {
                return ((t.Name == this.name) && typeof(Component).IsAssignableFrom(t));
            }
        }
    }
}

