﻿namespace UnityEditor.Web
{
    using System;

    internal class JspmPropertyInfo
    {
        public string name;
        public object value = null;

        public JspmPropertyInfo(string name, object value)
        {
            this.name = name;
            this.value = value;
        }
    }
}

