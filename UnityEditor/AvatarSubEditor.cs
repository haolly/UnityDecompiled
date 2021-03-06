﻿namespace UnityEditor
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    internal class AvatarSubEditor : ScriptableObject
    {
        protected AvatarEditor m_Inspector;

        protected void Apply()
        {
            this.serializedObject.ApplyModifiedProperties();
        }

        public void ApplyAndImport()
        {
            this.Apply();
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(this.avatarAsset));
            this.ResetValues();
        }

        protected void ApplyRevertGUI()
        {
            EditorGUILayout.Space();
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            using (new EditorGUI.DisabledScope(!this.HasModified()))
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Revert", new GUILayoutOption[0]))
                {
                    this.ResetValues();
                    if (this.HasModified())
                    {
                        Debug.LogError("Avatar tool reports modified values after reset.");
                    }
                }
                if (GUILayout.Button("Apply", new GUILayoutOption[0]))
                {
                    this.ApplyAndImport();
                }
            }
            if (GUILayout.Button("Done", new GUILayoutOption[0]))
            {
                this.m_Inspector.SwitchToAssetMode();
                GUIUtility.ExitGUI();
            }
            GUILayout.EndHorizontal();
        }

        public virtual void Disable()
        {
        }

        private static void DoWriteAllAssets()
        {
            UnityEngine.Object[] objArray = UnityEngine.Resources.FindObjectsOfTypeAll(typeof(UnityEngine.Object));
            foreach (UnityEngine.Object obj2 in objArray)
            {
                if (AssetDatabase.Contains(obj2))
                {
                    EditorUtility.SetDirty(obj2);
                }
            }
            AssetDatabase.SaveAssets();
        }

        public virtual void Enable(AvatarEditor inspector)
        {
            this.m_Inspector = inspector;
        }

        protected bool HasModified() => 
            this.serializedObject.hasModifiedProperties;

        public virtual void OnDestroy()
        {
            if (this.HasModified())
            {
                AssetImporter atPath = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(this.avatarAsset));
                if (atPath != null)
                {
                    if (EditorUtility.DisplayDialog("Unapplied import settings", "Unapplied import settings for '" + atPath.assetPath + "'", "Apply", "Revert"))
                    {
                        this.ApplyAndImport();
                    }
                    else
                    {
                        this.ResetValues();
                    }
                }
            }
        }

        public virtual void OnInspectorGUI()
        {
        }

        public virtual void OnSceneGUI()
        {
        }

        protected virtual void ResetValues()
        {
            this.serializedObject.Update();
        }

        protected Avatar avatarAsset =>
            this.m_Inspector.avatar;

        protected GameObject gameObject =>
            this.m_Inspector.m_GameObject;

        protected Dictionary<Transform, bool> modelBones =>
            this.m_Inspector.m_ModelBones;

        protected GameObject prefab =>
            this.m_Inspector.prefab;

        protected Transform root =>
            this.gameObject?.transform;

        protected SerializedObject serializedObject =>
            this.m_Inspector.serializedObject;
    }
}

