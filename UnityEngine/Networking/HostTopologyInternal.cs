﻿namespace UnityEngine.Networking
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Scripting;

    internal sealed class HostTopologyInternal : IDisposable
    {
        internal IntPtr m_Ptr;

        public HostTopologyInternal(HostTopology topology)
        {
            ConnectionConfigInternal config = new ConnectionConfigInternal(topology.DefaultConfig);
            this.InitWrapper(config, topology.MaxDefaultConnections);
            for (int i = 1; i <= topology.SpecialConnectionConfigsCount; i++)
            {
                ConnectionConfigInternal internal3 = new ConnectionConfigInternal(topology.GetSpecialConnectionConfig(i));
                this.AddSpecialConnectionConfig(internal3);
            }
            this.InitOtherParameters(topology);
        }

        private int AddSpecialConnectionConfig(ConnectionConfigInternal config) => 
            this.AddSpecialConnectionConfigWrapper(config);

        [MethodImpl(MethodImplOptions.InternalCall), GeneratedByOldBindingsGenerator]
        public extern int AddSpecialConnectionConfigWrapper(ConnectionConfigInternal config);
        [MethodImpl(MethodImplOptions.InternalCall), ThreadAndSerializationSafe, GeneratedByOldBindingsGenerator]
        public extern void Dispose();
        ~HostTopologyInternal()
        {
            this.Dispose();
        }

        [MethodImpl(MethodImplOptions.InternalCall), GeneratedByOldBindingsGenerator]
        public extern void InitMessagePoolSizeGrowthFactor(float factor);
        private void InitOtherParameters(HostTopology topology)
        {
            this.InitReceivedPoolSize(topology.ReceivedMessagePoolSize);
            this.InitSentMessagePoolSize(topology.SentMessagePoolSize);
            this.InitMessagePoolSizeGrowthFactor(topology.MessagePoolSizeGrowthFactor);
        }

        [MethodImpl(MethodImplOptions.InternalCall), GeneratedByOldBindingsGenerator]
        public extern void InitReceivedPoolSize(ushort pool);
        [MethodImpl(MethodImplOptions.InternalCall), GeneratedByOldBindingsGenerator]
        public extern void InitSentMessagePoolSize(ushort pool);
        [MethodImpl(MethodImplOptions.InternalCall), GeneratedByOldBindingsGenerator]
        public extern void InitWrapper(ConnectionConfigInternal config, int maxDefaultConnections);
    }
}

