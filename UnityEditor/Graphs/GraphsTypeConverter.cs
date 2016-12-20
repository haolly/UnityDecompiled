﻿namespace UnityEditor.Graphs
{
    using System;
    using System.CodeDom;
    using System.ComponentModel;

    public class GraphsTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return (sourceType == typeof(string));
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return ((destinationType == typeof(string)) || destinationType.IsAssignableFrom(typeof(CodeExpression)));
        }
    }
}

