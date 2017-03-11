﻿namespace UnityEngine.Networking
{
    using System;

    /// <summary>
    /// <para>Defines size of the buffer holding reliable messages, before they will be acknowledged.</para>
    /// </summary>
    public enum ConnectionAcksType
    {
        /// <summary>
        /// <para>Ack buffer can hold 128 messages.</para>
        /// </summary>
        Acks128 = 4,
        /// <summary>
        /// <para>Ack buffer can hold 32 messages.</para>
        /// </summary>
        Acks32 = 1,
        /// <summary>
        /// <para>Ack buffer can hold 64 messages.</para>
        /// </summary>
        Acks64 = 2,
        /// <summary>
        /// <para>Ack buffer can hold 96 messages.</para>
        /// </summary>
        Acks96 = 3
    }
}

