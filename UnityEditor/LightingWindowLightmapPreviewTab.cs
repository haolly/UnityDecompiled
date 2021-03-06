﻿namespace UnityEditor
{
    using System;
    using System.Collections;
    using UnityEngine;

    internal class LightingWindowLightmapPreviewTab
    {
        private Vector2 m_ScrollPositionLightmaps = Vector2.zero;
        private Vector2 m_ScrollPositionMaps = Vector2.zero;
        private int m_SelectedLightmap = -1;
        private static Styles s_Styles;

        private static void Header(Rect rect, float maxLightmaps)
        {
            rect.width /= maxLightmaps;
            EditorGUI.DropShadowLabel(rect, "Intensity");
            rect.x += rect.width;
            EditorGUI.DropShadowLabel(rect, "Directionality");
        }

        private Texture2D LightmapField(Texture2D lightmap, int index)
        {
            Rect rect = GUILayoutUtility.GetRect(100f, 100f, EditorStyles.objectField);
            this.MenuSelectLightmapUsers(rect, index);
            Texture2D textured = EditorGUI.ObjectField(rect, lightmap, typeof(Texture2D), false) as Texture2D;
            if ((index == this.m_SelectedLightmap) && (Event.current.type == EventType.Repaint))
            {
                s_Styles.selectedLightmapHighlight.Draw(rect, false, false, false, false);
            }
            return textured;
        }

        public void LightmapPreview(Rect r)
        {
            if (s_Styles == null)
            {
                s_Styles = new Styles();
            }
            GUI.Box(r, "", "PreBackground");
            GUILayoutOption[] options = new GUILayoutOption[] { GUILayout.Height(r.height) };
            this.m_ScrollPositionLightmaps = EditorGUILayout.BeginScrollView(this.m_ScrollPositionLightmaps, options);
            int lightmapIndex = 0;
            float maxLightmaps = 2f;
            Header(GUILayoutUtility.GetRect(r.width, r.width, (float) 20f, (float) 20f), maxLightmaps);
            foreach (LightmapData data in LightmapSettings.lightmaps)
            {
                if ((data.lightmapLight == null) && (data.lightmapDir == null))
                {
                    lightmapIndex++;
                }
                else
                {
                    int num4 = (data.lightmapLight == null) ? -1 : Math.Max(data.lightmapLight.width, data.lightmapLight.height);
                    int num5 = (data.lightmapDir == null) ? -1 : Math.Max(data.lightmapDir.width, data.lightmapDir.height);
                    Texture2D textured = (num4 <= num5) ? data.lightmapDir : data.lightmapLight;
                    GUILayoutOption[] optionArray2 = new GUILayoutOption[2];
                    optionArray2[0] = GUILayout.MaxWidth(r.width);
                    int[] values = new int[] { textured.height };
                    optionArray2[1] = GUILayout.MaxHeight((float) Mathf.Min(values));
                    GUILayoutOption[] optionArray = optionArray2;
                    Rect aspectRect = GUILayoutUtility.GetAspectRect(maxLightmaps, optionArray);
                    aspectRect.width -= 5f;
                    aspectRect.width /= maxLightmaps;
                    EditorGUI.DrawPreviewTexture(aspectRect, data.lightmapLight);
                    this.MenuSelectLightmapUsers(aspectRect, lightmapIndex);
                    if (data.lightmapDir != null)
                    {
                        aspectRect.x += aspectRect.width + 5f;
                        EditorGUI.DrawPreviewTexture(aspectRect, data.lightmapDir);
                        this.MenuSelectLightmapUsers(aspectRect, lightmapIndex);
                    }
                    GUILayout.Space(10f);
                    lightmapIndex++;
                }
            }
            EditorGUILayout.EndScrollView();
        }

        public void Maps()
        {
            if (s_Styles == null)
            {
                s_Styles = new Styles();
            }
            GUI.changed = false;
            if (Lightmapping.giWorkflowMode == Lightmapping.GIWorkflowMode.OnDemand)
            {
                SerializedObject obj2 = new SerializedObject(LightmapEditorSettings.GetLightmapSettings());
                EditorGUILayout.PropertyField(obj2.FindProperty("m_LightingDataAsset"), s_Styles.LightingDataAsset, new GUILayoutOption[0]);
                obj2.ApplyModifiedProperties();
            }
            GUILayout.Space(10f);
            LightmapData[] lightmaps = LightmapSettings.lightmaps;
            this.m_ScrollPositionMaps = GUILayout.BeginScrollView(this.m_ScrollPositionMaps, new GUILayoutOption[0]);
            using (new EditorGUI.DisabledScope(true))
            {
                for (int i = 0; i < lightmaps.Length; i++)
                {
                    GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(i.ToString(), new GUILayoutOption[0]);
                    GUILayout.Space(5f);
                    lightmaps[i].lightmapLight = this.LightmapField(lightmaps[i].lightmapLight, i);
                    GUILayout.Space(10f);
                    lightmaps[i].lightmapDir = this.LightmapField(lightmaps[i].lightmapDir, i);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndScrollView();
        }

        private void MenuSelectLightmapUsers(Rect rect, int lightmapIndex)
        {
            if ((Event.current.type == EventType.ContextClick) && rect.Contains(Event.current.mousePosition))
            {
                string[] texts = new string[] { "Select Lightmap Users" };
                Rect position = new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 1f, 1f);
                EditorUtility.DisplayCustomMenu(position, EditorGUIUtility.TempContent(texts), -1, new EditorUtility.SelectMenuItemFunction(this.SelectLightmapUsers), lightmapIndex);
                Event.current.Use();
            }
        }

        private void SelectLightmapUsers(object userData, string[] options, int selected)
        {
            int num = (int) userData;
            ArrayList list = new ArrayList();
            MeshRenderer[] rendererArray = UnityEngine.Object.FindObjectsOfType(typeof(MeshRenderer)) as MeshRenderer[];
            foreach (MeshRenderer renderer in rendererArray)
            {
                if ((renderer != null) && (renderer.lightmapIndex == num))
                {
                    list.Add(renderer.gameObject);
                }
            }
            Terrain[] terrainArray = UnityEngine.Object.FindObjectsOfType(typeof(Terrain)) as Terrain[];
            foreach (Terrain terrain in terrainArray)
            {
                if ((terrain != null) && (terrain.lightmapIndex == num))
                {
                    list.Add(terrain.gameObject);
                }
            }
            Selection.objects = list.ToArray(typeof(UnityEngine.Object)) as UnityEngine.Object[];
        }

        public void UpdateLightmapSelection()
        {
            MeshRenderer renderer;
            Terrain terrain = null;
            if ((Selection.activeGameObject == null) || (((renderer = Selection.activeGameObject.GetComponent<MeshRenderer>()) == null) && ((terrain = Selection.activeGameObject.GetComponent<Terrain>()) == null)))
            {
                this.m_SelectedLightmap = -1;
            }
            else
            {
                this.m_SelectedLightmap = (renderer == null) ? terrain.lightmapIndex : renderer.lightmapIndex;
            }
        }

        private class Styles
        {
            public GUIContent LightingDataAsset = EditorGUIUtility.TextContent("Lighting Data Asset|A different LightingData.asset can be assigned here. These assets are generated by baking a scene in the OnDemand mode.");
            public GUIContent LightProbes = EditorGUIUtility.TextContent("Light Probes|A different LightProbes.asset can be assigned here. These assets are generated by baking a scene containing light probes.");
            public GUIContent MapsArraySize = EditorGUIUtility.TextContent("Array Size|The length of the array of lightmaps.");
            public GUIStyle selectedLightmapHighlight = "LightmapEditorSelectedHighlight";
        }
    }
}

