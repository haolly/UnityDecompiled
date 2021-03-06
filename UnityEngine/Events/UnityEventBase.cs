﻿namespace UnityEngine.Events
{
    using System;
    using System.Reflection;
    using UnityEngine;
    using UnityEngine.Serialization;

    /// <summary>
    /// <para>Abstract base class for UnityEvents.</para>
    /// </summary>
    [Serializable]
    public abstract class UnityEventBase : ISerializationCallbackReceiver
    {
        private InvokableCallList m_Calls = new InvokableCallList();
        private bool m_CallsDirty = true;
        [FormerlySerializedAs("m_PersistentListeners"), SerializeField]
        private PersistentCallGroup m_PersistentCalls = new PersistentCallGroup();
        [SerializeField]
        private string m_TypeName;

        protected UnityEventBase()
        {
            this.m_TypeName = base.GetType().AssemblyQualifiedName;
        }

        internal void AddBoolPersistentListener(UnityAction<bool> call, bool argument)
        {
            int persistentEventCount = this.GetPersistentEventCount();
            this.AddPersistentListener();
            this.RegisterBoolPersistentListener(persistentEventCount, call, argument);
        }

        internal void AddCall(BaseInvokableCall call)
        {
            this.m_Calls.AddListener(call);
        }

        internal void AddFloatPersistentListener(UnityAction<float> call, float argument)
        {
            int persistentEventCount = this.GetPersistentEventCount();
            this.AddPersistentListener();
            this.RegisterFloatPersistentListener(persistentEventCount, call, argument);
        }

        internal void AddIntPersistentListener(UnityAction<int> call, int argument)
        {
            int persistentEventCount = this.GetPersistentEventCount();
            this.AddPersistentListener();
            this.RegisterIntPersistentListener(persistentEventCount, call, argument);
        }

        protected void AddListener(object targetObj, MethodInfo method)
        {
            this.m_Calls.AddListener(this.GetDelegate(targetObj, method));
        }

        internal void AddObjectPersistentListener<T>(UnityAction<T> call, T argument) where T: UnityEngine.Object
        {
            int persistentEventCount = this.GetPersistentEventCount();
            this.AddPersistentListener();
            this.RegisterObjectPersistentListener<T>(persistentEventCount, call, argument);
        }

        internal void AddPersistentListener()
        {
            this.m_PersistentCalls.AddListener();
        }

        internal void AddStringPersistentListener(UnityAction<string> call, string argument)
        {
            int persistentEventCount = this.GetPersistentEventCount();
            this.AddPersistentListener();
            this.RegisterStringPersistentListener(persistentEventCount, call, argument);
        }

        internal void AddVoidPersistentListener(UnityAction call)
        {
            int persistentEventCount = this.GetPersistentEventCount();
            this.AddPersistentListener();
            this.RegisterVoidPersistentListener(persistentEventCount, call);
        }

        private void DirtyPersistentCalls()
        {
            this.m_Calls.ClearPersistent();
            this.m_CallsDirty = true;
        }

        internal MethodInfo FindMethod(PersistentCall call)
        {
            System.Type argumentType = typeof(UnityEngine.Object);
            if (!string.IsNullOrEmpty(call.arguments.unityObjectArgumentAssemblyTypeName))
            {
                System.Type type = System.Type.GetType(call.arguments.unityObjectArgumentAssemblyTypeName, false);
                if (type != null)
                {
                    argumentType = type;
                }
                else
                {
                    argumentType = typeof(UnityEngine.Object);
                }
            }
            return this.FindMethod(call.methodName, call.target, call.mode, argumentType);
        }

        internal MethodInfo FindMethod(string name, object listener, PersistentListenerMode mode, System.Type argumentType)
        {
            System.Type expressionStack_E6_0;
            int expressionStack_E6_1;
            System.Type[] expressionStack_E6_2;
            System.Type[] expressionStack_E6_3;
            string expressionStack_E6_4;
            object expressionStack_E6_5;
            switch (mode)
            {
                case PersistentListenerMode.EventDefined:
                    return this.FindMethod_Impl(name, listener);

                case PersistentListenerMode.Void:
                    return GetValidMethodInfo(listener, name, new System.Type[0]);

                case PersistentListenerMode.Object:
                {
                    int expressionStack_DB_1;
                    System.Type[] expressionStack_DB_2;
                    System.Type[] expressionStack_DB_3;
                    string expressionStack_DB_4;
                    object expressionStack_DB_5;
                    System.Type[] typeArray10 = new System.Type[1];
                    if (argumentType != null)
                    {
                        expressionStack_E6_5 = listener;
                        expressionStack_E6_4 = name;
                        expressionStack_E6_3 = typeArray10;
                        expressionStack_E6_2 = typeArray10;
                        expressionStack_E6_1 = 0;
                        expressionStack_E6_0 = argumentType;
                        break;
                    }
                    else
                    {
                        expressionStack_DB_5 = listener;
                        expressionStack_DB_4 = name;
                        expressionStack_DB_3 = typeArray10;
                        expressionStack_DB_2 = typeArray10;
                        expressionStack_DB_1 = 0;
                        System.Type expressionStack_DB_0 = argumentType;
                    }
                    expressionStack_E6_5 = expressionStack_DB_5;
                    expressionStack_E6_4 = expressionStack_DB_4;
                    expressionStack_E6_3 = expressionStack_DB_3;
                    expressionStack_E6_2 = expressionStack_DB_2;
                    expressionStack_E6_1 = expressionStack_DB_1;
                    expressionStack_E6_0 = typeof(UnityEngine.Object);
                    break;
                }
                case PersistentListenerMode.Int:
                {
                    System.Type[] argumentTypes = new System.Type[] { typeof(int) };
                    return GetValidMethodInfo(listener, name, argumentTypes);
                }
                case PersistentListenerMode.Float:
                {
                    System.Type[] typeArray6 = new System.Type[] { typeof(float) };
                    return GetValidMethodInfo(listener, name, typeArray6);
                }
                case PersistentListenerMode.String:
                {
                    System.Type[] typeArray9 = new System.Type[] { typeof(string) };
                    return GetValidMethodInfo(listener, name, typeArray9);
                }
                case PersistentListenerMode.Bool:
                {
                    System.Type[] typeArray8 = new System.Type[] { typeof(bool) };
                    return GetValidMethodInfo(listener, name, typeArray8);
                }
                default:
                    return null;
            }
            expressionStack_E6_2[expressionStack_E6_1] = expressionStack_E6_0;
            return GetValidMethodInfo(expressionStack_E6_5, expressionStack_E6_4, expressionStack_E6_3);
        }

        protected abstract MethodInfo FindMethod_Impl(string name, object targetObj);
        internal abstract BaseInvokableCall GetDelegate(object target, MethodInfo theFunction);
        /// <summary>
        /// <para>Get the number of registered persistent listeners.</para>
        /// </summary>
        public int GetPersistentEventCount() => 
            this.m_PersistentCalls.Count;

        /// <summary>
        /// <para>Get the target method name of the listener at index index.</para>
        /// </summary>
        /// <param name="index">Index of the listener to query.</param>
        public string GetPersistentMethodName(int index)
        {
            PersistentCall listener = this.m_PersistentCalls.GetListener(index);
            return ((listener == null) ? string.Empty : listener.methodName);
        }

        /// <summary>
        /// <para>Get the target component of the listener at index index.</para>
        /// </summary>
        /// <param name="index">Index of the listener to query.</param>
        public UnityEngine.Object GetPersistentTarget(int index)
        {
            PersistentCall listener = this.m_PersistentCalls.GetListener(index);
            return listener?.target;
        }

        /// <summary>
        /// <para>Given an object, function name, and a list of argument types; find the method that matches.</para>
        /// </summary>
        /// <param name="obj">Object to search for the method.</param>
        /// <param name="functionName">Function name to search for.</param>
        /// <param name="argumentTypes">Argument types for the function.</param>
        public static MethodInfo GetValidMethodInfo(object obj, string functionName, System.Type[] argumentTypes)
        {
            for (System.Type type = obj.GetType(); (type != typeof(object)) && (type != null); type = type.BaseType)
            {
                MethodInfo info = type.GetMethod(functionName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, argumentTypes, null);
                if (info != null)
                {
                    ParameterInfo[] parameters = info.GetParameters();
                    bool flag = true;
                    int index = 0;
                    foreach (ParameterInfo info2 in parameters)
                    {
                        System.Type type2 = argumentTypes[index];
                        System.Type parameterType = info2.ParameterType;
                        flag = type2.IsPrimitive == parameterType.IsPrimitive;
                        if (!flag)
                        {
                            break;
                        }
                        index++;
                    }
                    if (flag)
                    {
                        return info;
                    }
                }
            }
            return null;
        }

        protected void Invoke(object[] parameters)
        {
            this.RebuildPersistentCallsIfNeeded();
            this.m_Calls.Invoke(parameters);
        }

        private void RebuildPersistentCallsIfNeeded()
        {
            if (this.m_CallsDirty)
            {
                this.m_PersistentCalls.Initialize(this.m_Calls, this);
                this.m_CallsDirty = false;
            }
        }

        internal void RegisterBoolPersistentListener(int index, UnityAction<bool> call, bool argument)
        {
            if (call == null)
            {
                Debug.LogWarning("Registering a Listener requires an action");
            }
            else if (this.ValidateRegistration(call.Method, call.Target, PersistentListenerMode.Bool))
            {
                this.m_PersistentCalls.RegisterBoolPersistentListener(index, call.Target as UnityEngine.Object, argument, call.Method.Name);
                this.DirtyPersistentCalls();
            }
        }

        internal void RegisterFloatPersistentListener(int index, UnityAction<float> call, float argument)
        {
            if (call == null)
            {
                Debug.LogWarning("Registering a Listener requires an action");
            }
            else if (this.ValidateRegistration(call.Method, call.Target, PersistentListenerMode.Float))
            {
                this.m_PersistentCalls.RegisterFloatPersistentListener(index, call.Target as UnityEngine.Object, argument, call.Method.Name);
                this.DirtyPersistentCalls();
            }
        }

        internal void RegisterIntPersistentListener(int index, UnityAction<int> call, int argument)
        {
            if (call == null)
            {
                Debug.LogWarning("Registering a Listener requires an action");
            }
            else if (this.ValidateRegistration(call.Method, call.Target, PersistentListenerMode.Int))
            {
                this.m_PersistentCalls.RegisterIntPersistentListener(index, call.Target as UnityEngine.Object, argument, call.Method.Name);
                this.DirtyPersistentCalls();
            }
        }

        internal void RegisterObjectPersistentListener<T>(int index, UnityAction<T> call, T argument) where T: UnityEngine.Object
        {
            if (call == null)
            {
                throw new ArgumentNullException("call", "Registering a Listener requires a non null call");
            }
            if (this.ValidateRegistration(call.Method, call.Target, PersistentListenerMode.Object, (argument != null) ? argument.GetType() : typeof(UnityEngine.Object)))
            {
                this.m_PersistentCalls.RegisterObjectPersistentListener(index, call.Target as UnityEngine.Object, argument, call.Method.Name);
                this.DirtyPersistentCalls();
            }
        }

        protected void RegisterPersistentListener(int index, object targetObj, MethodInfo method)
        {
            if (this.ValidateRegistration(method, targetObj, PersistentListenerMode.EventDefined))
            {
                this.m_PersistentCalls.RegisterEventPersistentListener(index, targetObj as UnityEngine.Object, method.Name);
                this.DirtyPersistentCalls();
            }
        }

        internal void RegisterStringPersistentListener(int index, UnityAction<string> call, string argument)
        {
            if (call == null)
            {
                Debug.LogWarning("Registering a Listener requires an action");
            }
            else if (this.ValidateRegistration(call.Method, call.Target, PersistentListenerMode.String))
            {
                this.m_PersistentCalls.RegisterStringPersistentListener(index, call.Target as UnityEngine.Object, argument, call.Method.Name);
                this.DirtyPersistentCalls();
            }
        }

        internal void RegisterVoidPersistentListener(int index, UnityAction call)
        {
            if (call == null)
            {
                Debug.LogWarning("Registering a Listener requires an action");
            }
            else if (this.ValidateRegistration(call.Method, call.Target, PersistentListenerMode.Void))
            {
                this.m_PersistentCalls.RegisterVoidPersistentListener(index, call.Target as UnityEngine.Object, call.Method.Name);
                this.DirtyPersistentCalls();
            }
        }

        /// <summary>
        /// <para>Remove all non-persisent (ie created from script) listeners  from the event.</para>
        /// </summary>
        public void RemoveAllListeners()
        {
            this.m_Calls.Clear();
        }

        protected void RemoveListener(object targetObj, MethodInfo method)
        {
            this.m_Calls.RemoveListener(targetObj, method);
        }

        internal void RemovePersistentListener(int index)
        {
            this.m_PersistentCalls.RemoveListener(index);
            this.DirtyPersistentCalls();
        }

        internal void RemovePersistentListener(UnityEngine.Object target, MethodInfo method)
        {
            if ((((method != null) && !method.IsStatic) && (target != null)) && (target.GetInstanceID() != 0))
            {
                this.m_PersistentCalls.RemoveListeners(target, method.Name);
                this.DirtyPersistentCalls();
            }
        }

        /// <summary>
        /// <para>Modify the execution state of a persistent listener.</para>
        /// </summary>
        /// <param name="index">Index of the listener to query.</param>
        /// <param name="state">State to set.</param>
        public void SetPersistentListenerState(int index, UnityEventCallState state)
        {
            PersistentCall listener = this.m_PersistentCalls.GetListener(index);
            if (listener != null)
            {
                listener.callState = state;
            }
            this.DirtyPersistentCalls();
        }

        public override string ToString() => 
            (base.ToString() + " " + base.GetType().FullName);

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            this.DirtyPersistentCalls();
            this.m_TypeName = base.GetType().AssemblyQualifiedName;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }

        internal void UnregisterPersistentListener(int index)
        {
            this.m_PersistentCalls.UnregisterPersistentListener(index);
            this.DirtyPersistentCalls();
        }

        protected bool ValidateRegistration(MethodInfo method, object targetObj, PersistentListenerMode mode) => 
            this.ValidateRegistration(method, targetObj, mode, typeof(UnityEngine.Object));

        protected bool ValidateRegistration(MethodInfo method, object targetObj, PersistentListenerMode mode, System.Type argumentType)
        {
            if (method == null)
            {
                object[] args = new object[] { targetObj };
                throw new ArgumentNullException("method", UnityString.Format("Can not register null method on {0} for callback!", args));
            }
            UnityEngine.Object obj2 = targetObj as UnityEngine.Object;
            if ((obj2 == null) || (obj2.GetInstanceID() == 0))
            {
                object[] objArray2 = new object[] { method.Name, targetObj, (targetObj != null) ? targetObj.GetType().ToString() : "null" };
                throw new ArgumentException(UnityString.Format("Could not register callback {0} on {1}. The class {2} does not derive from UnityEngine.Object", objArray2));
            }
            if (method.IsStatic)
            {
                object[] objArray3 = new object[] { method, base.GetType() };
                throw new ArgumentException(UnityString.Format("Could not register listener {0} on {1} static functions are not supported.", objArray3));
            }
            if (this.FindMethod(method.Name, targetObj, mode, argumentType) == null)
            {
                object[] objArray4 = new object[] { targetObj, method, base.GetType() };
                Debug.LogWarning(UnityString.Format("Could not register listener {0}.{1} on {2} the method could not be found.", objArray4));
                return false;
            }
            return true;
        }
    }
}

