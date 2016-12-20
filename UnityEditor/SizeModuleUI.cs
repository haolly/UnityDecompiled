﻿namespace UnityEditor
{
    using System;
    using UnityEngine;

    internal class SizeModuleUI : ModuleUI
    {
        private SerializedProperty m_SeparateAxes;
        private SerializedMinMaxCurve m_X;
        private SerializedMinMaxCurve m_Y;
        private SerializedMinMaxCurve m_Z;
        private static Texts s_Texts;

        public SizeModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName) : base(owner, o, "SizeModule", displayName)
        {
            base.m_ToolTip = "Controls the size of each particle during its lifetime.";
        }

        protected override void Init()
        {
            if (this.m_X == null)
            {
                if (s_Texts == null)
                {
                    s_Texts = new Texts();
                }
                this.m_X = new SerializedMinMaxCurve(this, s_Texts.x, "curve");
                this.m_Y = new SerializedMinMaxCurve(this, s_Texts.y, "y");
                this.m_Z = new SerializedMinMaxCurve(this, s_Texts.z, "z");
                this.m_X.m_AllowConstant = false;
                this.m_Y.m_AllowConstant = false;
                this.m_Z.m_AllowConstant = false;
                this.m_SeparateAxes = base.GetProperty("separateAxes");
            }
        }

        public override void OnInspectorGUI(ParticleSystem s)
        {
            if (s_Texts == null)
            {
                s_Texts = new Texts();
            }
            EditorGUI.BeginChangeCheck();
            bool flag = ModuleUI.GUIToggle(s_Texts.separateAxes, this.m_SeparateAxes, new GUILayoutOption[0]);
            if (EditorGUI.EndChangeCheck())
            {
                if (flag)
                {
                    this.m_X.RemoveCurveFromEditor();
                }
                else
                {
                    this.m_X.RemoveCurveFromEditor();
                    this.m_Y.RemoveCurveFromEditor();
                    this.m_Z.RemoveCurveFromEditor();
                }
            }
            MinMaxCurveState state = this.m_X.state;
            this.m_Y.state = state;
            this.m_Z.state = state;
            if (flag)
            {
                this.m_X.m_DisplayName = s_Texts.x;
                base.GUITripleMinMaxCurve(GUIContent.none, s_Texts.x, this.m_X, s_Texts.y, this.m_Y, s_Texts.z, this.m_Z, null, new GUILayoutOption[0]);
            }
            else
            {
                this.m_X.m_DisplayName = s_Texts.size;
                ModuleUI.GUIMinMaxCurve(s_Texts.size, this.m_X, new GUILayoutOption[0]);
            }
        }

        private class Texts
        {
            public GUIContent separateAxes = EditorGUIUtility.TextContent("Separate Axes|If enabled, you can control the angular velocity limit separately for each axis.");
            public GUIContent size = EditorGUIUtility.TextContent("Size|Controls the size of each particle during its lifetime.");
            public GUIContent x = EditorGUIUtility.TextContent("X");
            public GUIContent y = EditorGUIUtility.TextContent("Y");
            public GUIContent z = EditorGUIUtility.TextContent("Z");
        }
    }
}

