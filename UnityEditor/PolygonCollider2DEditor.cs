﻿namespace UnityEditor
{
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEditor.Sprites;
    using UnityEngine;

    [CanEditMultipleObjects, CustomEditor(typeof(PolygonCollider2D))]
    internal class PolygonCollider2DEditor : Collider2DEditorBase
    {
        [CompilerGenerated]
        private static Func<Object, bool> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<Object, PolygonCollider2D> <>f__am$cache1;
        private SerializedProperty m_Points;
        private readonly PolygonEditorUtility m_PolyUtility = new PolygonEditorUtility();
        private bool m_ShowColliderInfo;

        private void HandleDragAndDrop(Rect targetRect)
        {
            if (((Event.current.type == EventType.DragPerform) || (Event.current.type == EventType.DragUpdated)) && targetRect.Contains(Event.current.mousePosition))
            {
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = new Func<Object, bool>(null, (IntPtr) <HandleDragAndDrop>m__0);
                }
                foreach (Object obj2 in Enumerable.Where<Object>(DragAndDrop.objectReferences, <>f__am$cache0))
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    if (Event.current.type == EventType.DragPerform)
                    {
                        Sprite sprite = !(obj2 is Sprite) ? SpriteUtility.TextureToSprite(obj2 as Texture2D) : (obj2 as Sprite);
                        if (<>f__am$cache1 == null)
                        {
                            <>f__am$cache1 = new Func<Object, PolygonCollider2D>(null, (IntPtr) <HandleDragAndDrop>m__1);
                        }
                        foreach (PolygonCollider2D colliderd in Enumerable.Select<Object, PolygonCollider2D>(base.targets, <>f__am$cache1))
                        {
                            Vector2[][] vectorArray;
                            SpriteUtility.GenerateOutlineFromSprite(sprite, 0.25f, 200, true, out vectorArray);
                            colliderd.pathCount = vectorArray.Length;
                            for (int i = 0; i < vectorArray.Length; i++)
                            {
                                colliderd.SetPath(i, vectorArray[i]);
                            }
                            this.m_PolyUtility.StopEditing();
                            DragAndDrop.AcceptDrag();
                        }
                    }
                    return;
                }
                DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
            }
        }

        protected override void OnEditEnd()
        {
            this.m_PolyUtility.StopEditing();
        }

        protected override void OnEditStart()
        {
            if (base.target != null)
            {
                this.m_PolyUtility.StartEditing(base.target as Collider2D);
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            this.m_Points = base.serializedObject.FindProperty("m_Points");
            this.m_Points.isExpanded = false;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical(new GUILayoutOption[0]);
            base.BeginColliderInspector();
            base.OnInspectorGUI();
            if (base.targets.Length == 1)
            {
                EditorGUI.BeginDisabledGroup(base.editingCollider);
                EditorGUILayout.PropertyField(this.m_Points, true, new GUILayoutOption[0]);
                EditorGUI.EndDisabledGroup();
            }
            base.EndColliderInspector();
            base.FinalizeInspectorGUI();
            EditorGUILayout.EndVertical();
            this.HandleDragAndDrop(GUILayoutUtility.GetLastRect());
        }

        public void OnSceneGUI()
        {
            if (base.editingCollider)
            {
                this.m_PolyUtility.OnSceneGUI();
            }
        }
    }
}

