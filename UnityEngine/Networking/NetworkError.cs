﻿namespace UnityEngine.Networking
{
    using System;

    /// <summary>
    /// <para>Possible Networking.NetworkTransport errors.</para>
    /// </summary>
    public enum NetworkError
    {
        Ok,
        WrongHost,
        WrongConnection,
        WrongChannel,
        NoResources,
        BadMessage,
        Timeout,
        MessageToLong,
        WrongOperation,
        VersionMismatch,
        CRCMismatch,
        DNSFailure
    }
}

