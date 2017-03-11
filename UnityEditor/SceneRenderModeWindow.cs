﻿namespace UnityEditor
{
    using System;
    using UnityEngine;

    internal class SceneRenderModeWindow : PopupWindowContent
    {
        private const float kFrameWidth = 1f;
        private const float kHeaderHorizontalPadding = 5f;
        private const float kHeaderVerticalPadding = 1f;
        private const int kMenuHeaderCount = 7;
        private const float kSeparatorHeight = 3f;
        private const float kShowLightmapResolutionHeight = 22f;
        private const float kTogglePadding = 7f;
        private SerializedProperty m_EnableBakedGI;
        private SerializedProperty m_EnableRealtimeGI;
        private bool m_PathTracerBackend = false;
        private readonly SceneView m_SceneView;
        private readonly float m_WindowHeight = (((sMenuRowCount * 16f) + 15f) + 22f);
        private const float m_WindowWidth = 205f;
        private static readonly int sMenuRowCount = (sRenderModeCount + 7);
        private static readonly int sRenderModeCount = Styles.sRenderModeOptions.Length;

        public SceneRenderModeWindow(SceneView sceneView)
        {
            this.m_SceneView = sceneView;
        }

        private void DoOneMode(EditorWindow caller, ref Rect rect, DrawCameraMode drawCameraMode)
        {
            using (new EditorGUI.DisabledScope(!this.m_SceneView.CheckDrawModeForRenderingPath(drawCameraMode)))
            {
                EditorGUI.BeginChangeCheck();
                GUI.Toggle(rect, this.m_SceneView.renderMode == drawCameraMode, GetGUIContent(drawCameraMode), Styles.sMenuItem);
                if (EditorGUI.EndChangeCheck())
                {
                    this.m_SceneView.renderMode = drawCameraMode;
                    this.m_SceneView.Repaint();
                    GUIUtility.ExitGUI();
                }
                rect.y += 16f;
            }
        }

        private void DoResolutionToggle(Rect rect, bool disabled)
        {
            GUI.Label(new Rect(1f, rect.y, 203f, 22f), "", EditorStyles.inspectorBig);
            rect.y += 3f;
            rect.x += 7f;
            using (new EditorGUI.DisabledScope(disabled))
            {
                EditorGUI.BeginChangeCheck();
                bool flag = GUI.Toggle(rect, LightmapVisualization.showResolution, Styles.sResolutionToggle);
                if (EditorGUI.EndChangeCheck())
                {
                    LightmapVisualization.showResolution = flag;
                    SceneView.RepaintAll();
                }
            }
        }

        private void Draw(EditorWindow caller, float listElementWidth)
        {
            Rect rect = new Rect(0f, 0f, listElementWidth, 16f);
            this.DrawHeader(ref rect, Styles.sShadedHeader);
            for (int i = 0; i < sRenderModeCount; i++)
            {
                DrawCameraMode mode = Styles.sRenderModeUIOrder[i];
                switch (mode)
                {
                    case DrawCameraMode.Systems:
                        this.DrawSeparator(ref rect);
                        this.DrawHeader(ref rect, Styles.sGlobalIlluminationHeader);
                        goto Label_010D;

                    case DrawCameraMode.RealtimeIndirect:
                        this.DrawSeparator(ref rect);
                        this.DrawHeader(ref rect, Styles.sRealtimeGIHeader);
                        goto Label_010D;

                    case DrawCameraMode.BakedLightmap:
                        this.DrawSeparator(ref rect);
                        this.DrawHeader(ref rect, Styles.sBakedGIHeader);
                        goto Label_010D;

                    default:
                        if (mode != DrawCameraMode.ShadowCascades)
                        {
                            if (mode == DrawCameraMode.DeferredDiffuse)
                            {
                                break;
                            }
                            if (mode == DrawCameraMode.ValidateAlbedo)
                            {
                                goto Label_00F3;
                            }
                        }
                        else
                        {
                            this.DrawSeparator(ref rect);
                            this.DrawHeader(ref rect, Styles.sMiscellaneous);
                        }
                        goto Label_010D;
                }
                this.DrawSeparator(ref rect);
                this.DrawHeader(ref rect, Styles.sDeferredHeader);
                goto Label_010D;
            Label_00F3:
                this.DrawSeparator(ref rect);
                this.DrawHeader(ref rect, Styles.sMaterialValidationHeader);
            Label_010D:
                using (new EditorGUI.DisabledScope(this.IsModeDisabled(mode)))
                {
                    this.DoOneMode(caller, ref rect, mode);
                }
            }
            bool disabled = (this.m_SceneView.renderMode < DrawCameraMode.RealtimeCharting) || this.IsModeDisabled(this.m_SceneView.renderMode);
            this.DoResolutionToggle(rect, disabled);
        }

        private void DrawHeader(ref Rect rect, GUIContent label)
        {
            Rect position = rect;
            position.y++;
            position.x += 5f;
            position.width = EditorStyles.miniLabel.CalcSize(label).x;
            position.height = EditorStyles.miniLabel.CalcSize(label).y;
            GUI.Label(position, label, EditorStyles.miniLabel);
            rect.y += 16f;
        }

        private void DrawSeparator(ref Rect rect)
        {
            Rect position = rect;
            position.x += 5f;
            position.y += 3f;
            position.width -= 10f;
            position.height = 3f;
            GUI.Label(position, GUIContent.none, Styles.sSeparator);
            rect.y += 3f;
        }

        public static GUIContent GetGUIContent(DrawCameraMode drawCameraMode) => 
            Styles.sRenderModeOptions[(int) drawCameraMode];

        public override Vector2 GetWindowSize() => 
            new Vector2(205f, this.m_WindowHeight);

        private bool IsModeDisabled(DrawCameraMode mode) => 
            (((((mode == DrawCameraMode.BakedLightmap) && !this.m_EnableBakedGI.boolValue) || ((mode == DrawCameraMode.BakedAlbedo) && (!this.m_EnableBakedGI.boolValue || !this.m_PathTracerBackend))) || ((mode == DrawCameraMode.BakedTexelValidity) && (!this.m_EnableBakedGI.boolValue || !this.m_PathTracerBackend))) || ((((mode >= DrawCameraMode.RealtimeCharting) && (mode < DrawCameraMode.BakedLightmap)) && !this.m_EnableRealtimeGI.boolValue) && (!this.m_EnableBakedGI.boolValue || (this.m_EnableBakedGI.boolValue && this.m_PathTracerBackend))));

        public override void OnGUI(Rect rect)
        {
            if (((this.m_SceneView != null) && (this.m_SceneView.m_SceneViewState != null)) && (Event.current.type != EventType.Layout))
            {
                this.Draw(base.editorWindow, rect.width);
                if (Event.current.type == EventType.MouseMove)
                {
                    Event.current.Use();
                }
                if ((Event.current.type == EventType.KeyDown) && (Event.current.keyCode == KeyCode.Escape))
                {
                    base.editorWindow.Close();
                    GUIUtility.ExitGUI();
                }
            }
        }

        public override void OnOpen()
        {
            SerializedObject obj2 = new SerializedObject(LightmapEditorSettings.GetLightmapSettings());
            this.m_EnableRealtimeGI = obj2.FindProperty("m_GISettings.m_EnableRealtimeLightmaps");
            this.m_EnableBakedGI = obj2.FindProperty("m_GISettings.m_EnableBakedLightmaps");
            this.m_PathTracerBackend = LightmapEditorSettings.giBakeBackend == LightmapEditorSettings.GIBakeBackend.PathTracer;
        }

        private class Styles
        {
            public static readonly GUIContent sBakedGIHeader = EditorGUIUtility.TextContent("Baked GI");
            public static readonly GUIContent sDeferredHeader = EditorGUIUtility.TextContent("Deferred");
            public static readonly GUIContent sGlobalIlluminationHeader = EditorGUIUtility.TextContent("Global Illumination");
            public static readonly GUIContent sMaterialValidationHeader = EditorGUIUtility.TextContent("Material Validation");
            public static readonly GUIStyle sMenuItem = "MenuItem";
            public static readonly GUIContent sMiscellaneous = EditorGUIUtility.TextContent("Miscellaneous");
            public static readonly GUIContent sRealtimeGIHeader = EditorGUIUtility.TextContent("Realtime GI");
            public static readonly GUIContent[] sRenderModeOptions = new GUIContent[] { 
                EditorGUIUtility.TextContent("Shaded"), EditorGUIUtility.TextContent("Wireframe"), EditorGUIUtility.TextContent("Shaded Wireframe"), EditorGUIUtility.TextContent("Shadow Cascades"), EditorGUIUtility.TextContent("Render Paths"), EditorGUIUtility.TextContent("Alpha Channel"), EditorGUIUtility.TextContent("Overdraw"), EditorGUIUtility.TextContent("Mipmaps"), EditorGUIUtility.TextContent("Albedo"), EditorGUIUtility.TextContent("Specular"), EditorGUIUtility.TextContent("Smoothness"), EditorGUIUtility.TextContent("Normal"), EditorGUIUtility.TextContent("UV Charts"), EditorGUIUtility.TextContent("Systems"), EditorGUIUtility.TextContent("Albedo"), EditorGUIUtility.TextContent("Emissive"),
                EditorGUIUtility.TextContent("Indirect"), EditorGUIUtility.TextContent("Directionality"), EditorGUIUtility.TextContent("Baked Lightmap"), EditorGUIUtility.TextContent("Clustering"), EditorGUIUtility.TextContent("Lit Clustering"), EditorGUIUtility.TextContent("Validate Albedo"), EditorGUIUtility.TextContent("Validate Metal Specular"), EditorGUIUtility.TextContent("Shadowmask"), EditorGUIUtility.TextContent("Light Overlap"), EditorGUIUtility.TextContent("Albedo"), EditorGUIUtility.TextContent("Emissive"), EditorGUIUtility.TextContent("Directionality"), EditorGUIUtility.TextContent("Texel Validity"), EditorGUIUtility.TextContent("Lightmap Indices"), EditorGUIUtility.TextContent("UV Charts")
            };
            public static DrawCameraMode[] sRenderModeUIOrder = new DrawCameraMode[] { DrawCameraMode.Textured };
            public static readonly GUIContent sResolutionToggle = EditorGUIUtility.TextContent("Show Lightmap Resolution");
            public static readonly GUIStyle sSeparator = "sv_iconselector_sep";
            public static readonly GUIContent sShadedHeader = EditorGUIUtility.TextContent("Shading Mode");
        }
    }
}

