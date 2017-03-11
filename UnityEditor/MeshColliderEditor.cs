﻿namespace UnityEditor
{
    using System;
    using UnityEngine;

    [CustomEditor(typeof(MeshCollider)), CanEditMultipleObjects]
    internal class MeshColliderEditor : Collider3DEditorBase
    {
        private SerializedProperty m_Convex;
        private SerializedProperty m_InflateMesh;
        private SerializedProperty m_Mesh;
        private SerializedProperty m_SkinWidth;

        public override void OnEnable()
        {
            base.OnEnable();
            this.m_Mesh = base.serializedObject.FindProperty("m_Mesh");
            this.m_Convex = base.serializedObject.FindProperty("m_Convex");
            this.m_InflateMesh = base.serializedObject.FindProperty("m_InflateMesh");
            this.m_SkinWidth = base.serializedObject.FindProperty("m_SkinWidth");
        }

        public override void OnInspectorGUI()
        {
            base.serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(this.m_Convex, Texts.convextText, new GUILayoutOption[0]);
            if (EditorGUI.EndChangeCheck() && !this.m_Convex.boolValue)
            {
                base.m_IsTrigger.boolValue = false;
            }
            EditorGUI.indentLevel++;
            using (new EditorGUI.DisabledScope(!this.m_Convex.boolValue))
            {
                EditorGUILayout.PropertyField(this.m_InflateMesh, Texts.inflateMeshText, new GUILayoutOption[0]);
            }
            EditorGUI.indentLevel++;
            using (new EditorGUI.DisabledScope(!this.m_InflateMesh.boolValue))
            {
                EditorGUILayout.PropertyField(this.m_SkinWidth, Texts.skinWidthText, new GUILayoutOption[0]);
            }
            EditorGUI.indentLevel--;
            EditorGUI.indentLevel--;
            EditorGUI.indentLevel++;
            using (new EditorGUI.DisabledScope(!this.m_Convex.boolValue))
            {
                EditorGUILayout.PropertyField(base.m_IsTrigger, Texts.isTriggerText, new GUILayoutOption[0]);
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.PropertyField(base.m_Material, new GUILayoutOption[0]);
            EditorGUILayout.PropertyField(this.m_Mesh, new GUILayoutOption[0]);
            base.serializedObject.ApplyModifiedProperties();
        }

        private static class Texts
        {
            public static GUIContent convextText = new GUIContent("Convex", "Is this collider convex?");
            public static GUIContent inflateMeshText = new GUIContent("Inflate Mesh", "Should collision generation inflate the mesh.");
            public static GUIContent isTriggerText = new GUIContent("Is Trigger", "Is this collider a trigger? Triggers are only supported on convex colliders.");
            public static GUIContent skinWidthText = new GUIContent("Skin Width", "How far out to inflate the mesh when building collision mesh.");
        }
    }
}

