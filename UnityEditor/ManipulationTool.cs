﻿namespace UnityEditor
{
    using System;
    using UnityEngine;

    internal abstract class ManipulationTool
    {
        protected ManipulationTool()
        {
        }

        protected virtual void OnToolGUI(SceneView view)
        {
            if ((Selection.activeTransform != null) && !Tools.s_Hidden)
            {
                bool disabled = (!Tools.s_Hidden && EditorApplication.isPlaying) && GameObjectUtility.ContainsStatic(Selection.gameObjects);
                using (new EditorGUI.DisabledScope(disabled))
                {
                    Vector3 handlePosition = Tools.handlePosition;
                    this.ToolGUI(view, handlePosition, disabled);
                    Handles.ShowStaticLabelIfNeeded(handlePosition);
                }
            }
        }

        public abstract void ToolGUI(SceneView view, Vector3 handlePosition, bool isStatic);
    }
}

