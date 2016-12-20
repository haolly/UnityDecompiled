﻿namespace UnityEditor
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEditor.Audio;
    using UnityEngine;

    internal static class AudioMixerEffectGUI
    {
        [CompilerGenerated]
        private static GenericMenu.MenuFunction2 <>f__mg$cache0;
        [CompilerGenerated]
        private static GenericMenu.MenuFunction2 <>f__mg$cache1;
        [CompilerGenerated]
        private static GenericMenu.MenuFunction2 <>f__mg$cache2;
        [CompilerGenerated]
        private static GenericMenu.MenuFunction2 <>f__mg$cache3;
        [CompilerGenerated]
        private static GenericMenu.MenuFunction2 <>f__mg$cache4;
        [CompilerGenerated]
        private static GenericMenu.MenuFunction2 <>f__mg$cache5;
        [CompilerGenerated]
        private static GenericMenu.MenuFunction2 <>f__mg$cache6;
        [CompilerGenerated]
        private static GenericMenu.MenuFunction2 <>f__mg$cache7;
        private const string kAudioSliderFloatFormat = "F2";
        private const string kExposedParameterUnicodeChar = " ➔";

        public static void EffectHeader(string text)
        {
            GUILayout.Label(text, styles.headerStyle, new GUILayoutOption[0]);
        }

        public static void ExposePopupCallback(object obj)
        {
            ExposedParamContext context = (ExposedParamContext) obj;
            Undo.RecordObject(context.controller, "Expose Mixer Parameter");
            context.controller.AddExposedParameter(context.path);
            AudioMixerUtility.RepaintAudioMixerAndInspectors();
        }

        public static void ParameterTransitionOverrideCallback(object obj)
        {
            ParameterTransitionOverrideContext context = (ParameterTransitionOverrideContext) obj;
            Undo.RecordObject(context.controller, "Change Parameter Transition Type");
            if (context.type == ParameterTransitionType.Lerp)
            {
                context.controller.TargetSnapshot.ClearTransitionTypeOverride(context.parameter);
            }
            else
            {
                context.controller.TargetSnapshot.SetTransitionTypeOverride(context.parameter, context.type);
            }
        }

        public static bool PopupButton(GUIContent label, GUIContent buttonContent, GUIStyle style, out Rect buttonRect, params GUILayoutOption[] options)
        {
            if (label != null)
            {
                Rect position = EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options);
                int id = GUIUtility.GetControlID("EditorPopup".GetHashCode(), FocusType.Keyboard, position);
                buttonRect = EditorGUI.PrefixLabel(position, id, label);
            }
            else
            {
                Rect rect2 = GUILayoutUtility.GetRect(buttonContent, style, options);
                buttonRect = rect2;
            }
            return EditorGUI.ButtonMouseDown(buttonRect, buttonContent, FocusType.Passive, style);
        }

        public static bool Slider(GUIContent label, ref float value, float displayScale, float displayExponent, string unit, float leftValue, float rightValue, AudioMixerController controller, AudioParameterPath path, params GUILayoutOption[] options)
        {
            EditorGUI.BeginChangeCheck();
            float fieldWidth = EditorGUIUtility.fieldWidth;
            string kFloatFieldFormatString = EditorGUI.kFloatFieldFormatString;
            bool flag = controller.ContainsExposedParameter(path.parameter);
            EditorGUIUtility.fieldWidth = 70f;
            EditorGUI.kFloatFieldFormatString = "F2";
            EditorGUI.s_UnitString = unit;
            GUIContent content = label;
            if (flag)
            {
                content = GUIContent.Temp(label.text + " ➔", label.tooltip);
            }
            float num2 = value * displayScale;
            num2 = EditorGUILayout.PowerSlider(content, num2, leftValue * displayScale, rightValue * displayScale, displayExponent, options);
            EditorGUI.s_UnitString = null;
            EditorGUI.kFloatFieldFormatString = kFloatFieldFormatString;
            EditorGUIUtility.fieldWidth = fieldWidth;
            if ((Event.current.type == EventType.ContextClick) && GUILayoutUtility.topLevel.GetLast().Contains(Event.current.mousePosition))
            {
                ParameterTransitionType type;
                Event.current.Use();
                GenericMenu menu = new GenericMenu();
                if (!flag)
                {
                    if (<>f__mg$cache0 == null)
                    {
                        <>f__mg$cache0 = new GenericMenu.MenuFunction2(AudioMixerEffectGUI.ExposePopupCallback);
                    }
                    menu.AddItem(new GUIContent("Expose '" + path.ResolveStringPath(false) + "' to script"), false, <>f__mg$cache0, new ExposedParamContext(controller, path));
                }
                else
                {
                    if (<>f__mg$cache1 == null)
                    {
                        <>f__mg$cache1 = new GenericMenu.MenuFunction2(AudioMixerEffectGUI.UnexposePopupCallback);
                    }
                    menu.AddItem(new GUIContent("Unexpose"), false, <>f__mg$cache1, new ExposedParamContext(controller, path));
                }
                bool transitionTypeOverride = controller.TargetSnapshot.GetTransitionTypeOverride(path.parameter, out type);
                menu.AddSeparator(string.Empty);
                if (<>f__mg$cache2 == null)
                {
                    <>f__mg$cache2 = new GenericMenu.MenuFunction2(AudioMixerEffectGUI.ParameterTransitionOverrideCallback);
                }
                menu.AddItem(new GUIContent("Linear Snapshot Transition"), type == ParameterTransitionType.Lerp, <>f__mg$cache2, new ParameterTransitionOverrideContext(controller, path.parameter, ParameterTransitionType.Lerp));
                if (<>f__mg$cache3 == null)
                {
                    <>f__mg$cache3 = new GenericMenu.MenuFunction2(AudioMixerEffectGUI.ParameterTransitionOverrideCallback);
                }
                menu.AddItem(new GUIContent("Smoothstep Snapshot Transition"), type == ParameterTransitionType.Smoothstep, <>f__mg$cache3, new ParameterTransitionOverrideContext(controller, path.parameter, ParameterTransitionType.Smoothstep));
                if (<>f__mg$cache4 == null)
                {
                    <>f__mg$cache4 = new GenericMenu.MenuFunction2(AudioMixerEffectGUI.ParameterTransitionOverrideCallback);
                }
                menu.AddItem(new GUIContent("Squared Snapshot Transition"), type == ParameterTransitionType.Squared, <>f__mg$cache4, new ParameterTransitionOverrideContext(controller, path.parameter, ParameterTransitionType.Squared));
                if (<>f__mg$cache5 == null)
                {
                    <>f__mg$cache5 = new GenericMenu.MenuFunction2(AudioMixerEffectGUI.ParameterTransitionOverrideCallback);
                }
                menu.AddItem(new GUIContent("SquareRoot Snapshot Transition"), type == ParameterTransitionType.SquareRoot, <>f__mg$cache5, new ParameterTransitionOverrideContext(controller, path.parameter, ParameterTransitionType.SquareRoot));
                if (<>f__mg$cache6 == null)
                {
                    <>f__mg$cache6 = new GenericMenu.MenuFunction2(AudioMixerEffectGUI.ParameterTransitionOverrideCallback);
                }
                menu.AddItem(new GUIContent("BrickwallStart Snapshot Transition"), type == ParameterTransitionType.BrickwallStart, <>f__mg$cache6, new ParameterTransitionOverrideContext(controller, path.parameter, ParameterTransitionType.BrickwallStart));
                if (<>f__mg$cache7 == null)
                {
                    <>f__mg$cache7 = new GenericMenu.MenuFunction2(AudioMixerEffectGUI.ParameterTransitionOverrideCallback);
                }
                menu.AddItem(new GUIContent("BrickwallEnd Snapshot Transition"), type == ParameterTransitionType.BrickwallEnd, <>f__mg$cache7, new ParameterTransitionOverrideContext(controller, path.parameter, ParameterTransitionType.BrickwallEnd));
                menu.AddSeparator(string.Empty);
                menu.ShowAsContext();
            }
            if (EditorGUI.EndChangeCheck())
            {
                value = num2 / displayScale;
                return true;
            }
            return false;
        }

        public static void UnexposePopupCallback(object obj)
        {
            ExposedParamContext context = (ExposedParamContext) obj;
            Undo.RecordObject(context.controller, "Unexpose Mixer Parameter");
            context.controller.RemoveExposedParameter(context.path.parameter);
            AudioMixerUtility.RepaintAudioMixerAndInspectors();
        }

        private static AudioMixerDrawUtils.Styles styles
        {
            get
            {
                return AudioMixerDrawUtils.styles;
            }
        }

        private class ExposedParamContext
        {
            public AudioMixerController controller;
            public AudioParameterPath path;

            public ExposedParamContext(AudioMixerController controller, AudioParameterPath path)
            {
                this.controller = controller;
                this.path = path;
            }
        }

        private class ParameterTransitionOverrideContext
        {
            public AudioMixerController controller;
            public GUID parameter;
            public ParameterTransitionType type;

            public ParameterTransitionOverrideContext(AudioMixerController controller, GUID parameter, ParameterTransitionType type)
            {
                this.controller = controller;
                this.parameter = parameter;
                this.type = type;
            }
        }

        private class ParameterTransitionOverrideRemoveContext
        {
            public AudioMixerController controller;
            public GUID parameter;

            public ParameterTransitionOverrideRemoveContext(AudioMixerController controller, GUID parameter)
            {
                this.controller = controller;
                this.parameter = parameter;
            }
        }
    }
}

