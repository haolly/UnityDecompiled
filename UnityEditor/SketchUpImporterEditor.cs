﻿namespace UnityEditor
{
    using System;

    [CustomEditor(typeof(SketchUpImporter)), CanEditMultipleObjects]
    internal class SketchUpImporterEditor : ModelImporterEditor
    {
        internal override void OnEnable()
        {
            if (base.m_SubEditorTypes == null)
            {
                base.m_SubEditorTypes = new System.Type[] { typeof(SketchUpImporterModelEditor), typeof(ModelImporterRigEditor), typeof(ModelImporterClipEditor) };
                base.m_SubEditorNames = new string[] { "Sketch Up", "Rig", "Animations" };
            }
            base.OnEnable();
        }

        internal override bool showImportedObject =>
            (base.activeEditor is SketchUpImporterModelEditor);
    }
}

