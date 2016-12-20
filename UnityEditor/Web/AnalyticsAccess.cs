﻿namespace UnityEditor.Web
{
    using System;
    using UnityEditor;
    using UnityEditor.Analytics;
    using UnityEditor.Connect;

    [InitializeOnLoad]
    internal class AnalyticsAccess : CloudServiceAccess
    {
        private const string kServiceDisplayName = "Analytics";
        private const string kServiceName = "Analytics";
        private const string kServiceUrl = "https://public-cdn.cloud.unity3d.com/editor/production/cloud/analytics";

        static AnalyticsAccess()
        {
            UnityConnectServiceData cloudService = new UnityConnectServiceData("Analytics", "https://public-cdn.cloud.unity3d.com/editor/production/cloud/analytics", new AnalyticsAccess(), "unity/project/cloud/analytics");
            UnityConnectServiceCollection.instance.AddService(cloudService);
        }

        public override void EnableService(bool enabled)
        {
            AnalyticsSettings.enabled = enabled;
        }

        public override string GetServiceDisplayName()
        {
            return "Analytics";
        }

        public override string GetServiceName()
        {
            return "Analytics";
        }

        public override bool IsServiceEnabled()
        {
            return AnalyticsSettings.enabled;
        }

        public bool IsTestModeEnabled()
        {
            return AnalyticsSettings.testMode;
        }

        public void SetTestModeEnabled(bool enabled)
        {
            AnalyticsSettings.testMode = enabled;
        }
    }
}

