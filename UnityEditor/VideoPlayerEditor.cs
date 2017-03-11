﻿namespace UnityEditor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEditor.AnimatedValues;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.Video;

    [CustomEditor(typeof(VideoPlayer)), CanEditMultipleObjects]
    internal class VideoPlayerEditor : Editor
    {
        [CompilerGenerated]
        private static Func<string, GUIContent> <>f__am$cache0;
        [CompilerGenerated]
        private static EntryGenerator <>f__mg$cache0;
        [CompilerGenerated]
        private static EntryGenerator <>f__mg$cache1;
        private SerializedProperty m_AspectRatio;
        private SerializedProperty m_AudioOutputMode;
        private ushort m_AudioTrackCountCached = 0;
        private List<AudioTrackInfo> m_AudioTrackInfos;
        private SerializedProperty m_ControlledAudioTrackCount;
        private GUIContent m_ControlledAudioTrackCountContent;
        private SerializedProperty m_DataSource;
        private readonly AnimBool m_DataSourceIsClip = new AnimBool();
        private SerializedProperty m_DirectAudioMutes;
        private SerializedProperty m_DirectAudioVolumes;
        private SerializedProperty m_EnabledAudioTracks;
        private SerializedProperty m_Looping;
        private GUIContent[] m_MaterialPropertyPopupContent;
        private int m_MaterialPropertyPopupContentHash;
        private int m_MaterialPropertyPopupInvalidSelections;
        private int m_MaterialPropertyPopupSelection;
        private string m_MultiMaterialInfo = null;
        private SerializedProperty m_PlaybackSpeed;
        private SerializedProperty m_PlayOnAwake;
        private SerializedProperty m_RenderMode;
        private readonly AnimBool m_ShowAspectRatio = new AnimBool();
        private readonly AnimBool m_ShowAudioControls = new AnimBool();
        private readonly AnimBool m_ShowMaterialProperty = new AnimBool();
        private readonly AnimBool m_ShowRenderer = new AnimBool();
        private readonly AnimBool m_ShowRenderTexture = new AnimBool();
        private readonly AnimBool m_ShowTargetCamera = new AnimBool();
        private SerializedProperty m_TargetAudioSources;
        private SerializedProperty m_TargetCamera;
        private SerializedProperty m_TargetCameraAlpha;
        private SerializedProperty m_TargetMaterialProperty;
        private SerializedProperty m_TargetMaterialRenderer;
        private SerializedProperty m_TargetTexture;
        private SerializedProperty m_Url;
        private SerializedProperty m_VideoClip;
        private SerializedProperty m_WaitForFirstFrame;
        private static Styles s_Styles;

        private static GUIContent[] BuildPopupEntries(UnityEngine.Object[] objects, EntryGenerator func, out int selection, out int invalidSelections)
        {
            selection = -1;
            invalidSelections = 0;
            List<string> first = null;
            foreach (UnityEngine.Object obj2 in objects)
            {
                int num2;
                bool flag;
                List<string> second = func(obj2, objects.Count<UnityEngine.Object>() > 1, out num2, out flag);
                if (second != null)
                {
                    if (flag)
                    {
                        invalidSelections++;
                    }
                    List<string> list3 = (first != null) ? new List<string>(first.Intersect<string>(second)) : second;
                    selection = (first != null) ? ((((selection >= 0) && (num2 >= 0)) && (first[selection] == second[num2])) ? list3.IndexOf(first[selection]) : -1) : num2;
                    first = list3;
                }
            }
            if (first == null)
            {
                first = new List<string>();
            }
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = x => new GUIContent(x);
            }
            return Enumerable.Select<string, GUIContent>(first, <>f__am$cache0).ToArray<GUIContent>();
        }

        private void DisplayMultiMaterialInformation(bool refreshInfo)
        {
            if (refreshInfo || (this.m_MultiMaterialInfo == null))
            {
                this.m_MultiMaterialInfo = this.GenerateMultiMaterialinformation();
            }
            if (!string.IsNullOrEmpty(this.m_MultiMaterialInfo))
            {
                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                GUILayout.Space(EditorGUIUtility.labelWidth);
                EditorGUILayout.HelpBox(this.m_MultiMaterialInfo, MessageType.Info);
                GUILayout.EndHorizontal();
            }
        }

        private string GenerateMultiMaterialinformation()
        {
            if (base.targets.Count<UnityEngine.Object>() > 1)
            {
                return "";
            }
            VideoPlayer target = base.target as VideoPlayer;
            if (target == null)
            {
                return "";
            }
            Renderer targetRenderer = GetTargetRenderer(target);
            if (targetRenderer == null)
            {
                return "";
            }
            Material[] sharedMaterials = targetRenderer.sharedMaterials;
            if ((sharedMaterials == null) || (sharedMaterials.Count<Material>() <= 1))
            {
                return "";
            }
            List<string> source = new List<string>();
            foreach (Material material in sharedMaterials)
            {
                if (material != null)
                {
                    int propertyIdx = 0;
                    int propertyCount = ShaderUtil.GetPropertyCount(material.shader);
                    while (propertyIdx < propertyCount)
                    {
                        if ((ShaderUtil.GetPropertyType(material.shader, propertyIdx) == ShaderUtil.ShaderPropertyType.TexEnv) && (ShaderUtil.GetPropertyName(material.shader, propertyIdx) == this.m_TargetMaterialProperty.stringValue))
                        {
                            source.Add(material.name);
                            break;
                        }
                        propertyIdx++;
                    }
                }
            }
            if (source.Count<string>() == sharedMaterials.Count<Material>())
            {
                return s_Styles.texPropInAllMaterialsHelp;
            }
            return (string.Format(s_Styles.texPropInSomeMaterialsHelp, source.Count<string>(), sharedMaterials.Count<Material>()) + ": " + string.Join(", ", source.ToArray()));
        }

        private GUIContent GetAudioTrackEnabledContent(ushort trackIdx)
        {
            while (this.m_AudioTrackInfos.Count <= trackIdx)
            {
                this.m_AudioTrackInfos.Add(new AudioTrackInfo());
            }
            AudioTrackInfo info = this.m_AudioTrackInfos[trackIdx];
            VideoPlayer target = null;
            if (!base.serializedObject.isEditingMultipleObjects)
            {
                target = (VideoPlayer) base.target;
            }
            string str = (target == null) ? "" : target.GetAudioLanguageCode(trackIdx);
            ushort num = (target == null) ? ((ushort) 0) : target.GetAudioChannelCount(trackIdx);
            if (((str != info.language) || (num != info.channelCount)) || (info.content == null))
            {
                string str2 = "";
                if (str.Length > 0)
                {
                    str2 = str2 + str;
                }
                if (num > 0)
                {
                    if (str2.Length > 0)
                    {
                        str2 = str2 + ", ";
                    }
                    str2 = str2 + num.ToString() + " ch";
                }
                if (str2.Length > 0)
                {
                    str2 = " [" + str2 + "]";
                }
                info.content = EditorGUIUtility.TextContent("Track " + trackIdx + str2);
                info.content.tooltip = s_Styles.enableDecodingTooltip;
            }
            return info.content;
        }

        private static List<string> GetMaterialPropertyNames(UnityEngine.Object obj, bool multiSelect, out int selection, out bool invalidSelection)
        {
            selection = -1;
            invalidSelection = true;
            List<string> source = new List<string>();
            VideoPlayer vp = obj as VideoPlayer;
            if (vp != null)
            {
                Renderer targetRenderer = GetTargetRenderer(vp);
                if (targetRenderer == null)
                {
                    return source;
                }
                foreach (Material material in targetRenderer.sharedMaterials)
                {
                    if (material != null)
                    {
                        int propertyIdx = 0;
                        int propertyCount = ShaderUtil.GetPropertyCount(material.shader);
                        while (propertyIdx < propertyCount)
                        {
                            if (ShaderUtil.GetPropertyType(material.shader, propertyIdx) == ShaderUtil.ShaderPropertyType.TexEnv)
                            {
                                string propertyName = ShaderUtil.GetPropertyName(material.shader, propertyIdx);
                                if (!source.Contains(propertyName))
                                {
                                    source.Add(propertyName);
                                }
                            }
                            propertyIdx++;
                        }
                        selection = source.IndexOf(vp.targetMaterialProperty);
                        invalidSelection = (selection < 0) && (source.Count<string>() > 0);
                        if (invalidSelection && !multiSelect)
                        {
                            selection = source.Count<string>();
                            source.Add(vp.targetMaterialProperty);
                        }
                    }
                }
            }
            return source;
        }

        private static int GetMaterialPropertyPopupHash(UnityEngine.Object[] objects)
        {
            int num = 0;
            foreach (VideoPlayer player in objects)
            {
                if (player != null)
                {
                    Renderer targetRenderer = GetTargetRenderer(player);
                    if (targetRenderer != null)
                    {
                        num ^= player.targetMaterialProperty.GetHashCode();
                        foreach (Material material in targetRenderer.sharedMaterials)
                        {
                            if (material != null)
                            {
                                num ^= material.name.GetHashCode();
                                int propertyIdx = 0;
                                int propertyCount = ShaderUtil.GetPropertyCount(material.shader);
                                while (propertyIdx < propertyCount)
                                {
                                    if (ShaderUtil.GetPropertyType(material.shader, propertyIdx) == ShaderUtil.ShaderPropertyType.TexEnv)
                                    {
                                        num ^= ShaderUtil.GetPropertyName(material.shader, propertyIdx).GetHashCode();
                                    }
                                    propertyIdx++;
                                }
                            }
                        }
                    }
                }
            }
            return num;
        }

        private static Renderer GetTargetRenderer(VideoPlayer vp)
        {
            Renderer targetMaterialRenderer = vp.targetMaterialRenderer;
            if (targetMaterialRenderer != null)
            {
                return targetMaterialRenderer;
            }
            return vp.gameObject.GetComponent<Renderer>();
        }

        private void HandleAudio()
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(this.m_AudioOutputMode, s_Styles.audioOutputModeContent, new GUILayoutOption[0]);
            this.m_ShowAudioControls.target = this.m_AudioOutputMode.intValue != 0;
            if (EditorGUILayout.BeginFadeGroup(this.m_ShowAudioControls.faded))
            {
                if (base.serializedObject.isEditingMultipleObjects)
                {
                    EditorGUILayout.HelpBox(s_Styles.audioControlsNotEditableHelp, MessageType.Warning, false);
                }
                else if (this.m_AudioOutputMode.hasMultipleDifferentValues)
                {
                    EditorGUILayout.HelpBox(s_Styles.selectUniformAudioOutputModeHelp, MessageType.Warning, false);
                }
                else
                {
                    ushort intValue = (ushort) this.m_ControlledAudioTrackCount.intValue;
                    this.HandleControlledAudioTrackCount();
                    if (this.m_ControlledAudioTrackCount.hasMultipleDifferentValues)
                    {
                        EditorGUILayout.HelpBox(s_Styles.selectUniformAudioTracksHelp, MessageType.Warning, false);
                    }
                    else
                    {
                        VideoAudioOutputMode mode = (VideoAudioOutputMode) this.m_AudioOutputMode.intValue;
                        ushort num2 = (ushort) Math.Min(Math.Min((ushort) this.m_ControlledAudioTrackCount.intValue, intValue), this.m_EnabledAudioTracks.arraySize);
                        for (ushort i = 0; i < num2; i = (ushort) (i + 1))
                        {
                            EditorGUILayout.PropertyField(this.m_EnabledAudioTracks.GetArrayElementAtIndex(i), this.GetAudioTrackEnabledContent(i), new GUILayoutOption[0]);
                            EditorGUI.indentLevel++;
                            switch (mode)
                            {
                                case VideoAudioOutputMode.AudioSource:
                                    EditorGUILayout.PropertyField(this.m_TargetAudioSources.GetArrayElementAtIndex(i), s_Styles.audioSourceContent, new GUILayoutOption[0]);
                                    break;

                                case VideoAudioOutputMode.Direct:
                                    EditorGUILayout.PropertyField(this.m_DirectAudioMutes.GetArrayElementAtIndex(i), s_Styles.muteLabel, new GUILayoutOption[0]);
                                    EditorGUILayout.Slider(this.m_DirectAudioVolumes.GetArrayElementAtIndex(i), 0f, 1f, s_Styles.volumeLabel, new GUILayoutOption[0]);
                                    break;
                            }
                            EditorGUI.indentLevel--;
                        }
                    }
                }
            }
            EditorGUILayout.EndFadeGroup();
        }

        private void HandleControlledAudioTrackCount()
        {
            if (!this.m_DataSourceIsClip.value && !this.m_DataSource.hasMultipleDifferentValues)
            {
                GUIContent controlledAudioTrackCountContent;
                VideoPlayer target = (VideoPlayer) base.target;
                ushort num = !base.serializedObject.isEditingMultipleObjects ? target.audioTrackCount : ((ushort) 0);
                if (num == 0)
                {
                    controlledAudioTrackCountContent = s_Styles.controlledAudioTrackCountContent;
                }
                else
                {
                    if (num != this.m_AudioTrackCountCached)
                    {
                        this.m_AudioTrackCountCached = num;
                        this.m_ControlledAudioTrackCountContent = EditorGUIUtility.TextContent(string.Concat(new object[] { s_Styles.controlledAudioTrackCountContent.text, " [", num, " found]" }));
                        this.m_ControlledAudioTrackCountContent.tooltip = s_Styles.controlledAudioTrackCountContent.tooltip;
                    }
                    controlledAudioTrackCountContent = this.m_ControlledAudioTrackCountContent;
                }
                EditorGUILayout.PropertyField(this.m_ControlledAudioTrackCount, controlledAudioTrackCountContent, new GUILayoutOption[0]);
            }
        }

        private void HandleDataSourceField()
        {
            this.m_DataSourceIsClip.target = this.m_DataSource.intValue == 0;
            if (this.m_DataSource.hasMultipleDifferentValues)
            {
                EditorGUILayout.HelpBox(s_Styles.selectUniformVideoSourceHelp, MessageType.Warning, false);
            }
            else if (EditorGUILayout.BeginFadeGroup(this.m_DataSourceIsClip.faded))
            {
                EditorGUILayout.PropertyField(this.m_VideoClip, s_Styles.videoClipContent, new GUILayoutOption[0]);
            }
            else
            {
                EditorGUILayout.PropertyField(this.m_Url, s_Styles.urlContent, new GUILayoutOption[0]);
                Rect position = EditorGUILayout.GetControlRect(true, 16f, new GUILayoutOption[0]);
                position.xMin += EditorGUIUtility.labelWidth;
                position.xMax = (position.xMin + GUI.skin.label.CalcSize(s_Styles.browseContent).x) + 10f;
                if (EditorGUI.DropdownButton(position, s_Styles.browseContent, FocusType.Passive, GUISkin.current.button))
                {
                    string[] filters = new string[] { "Movie files", "dv,mp4,mpg,mpeg,m4v,ogv,vp8,webm", "All files", "*" };
                    string str = EditorUtility.OpenFilePanelWithFilters(s_Styles.selectMovieFile, "", filters);
                    if (!string.IsNullOrEmpty(str))
                    {
                        this.m_Url.stringValue = "file://" + str;
                    }
                }
            }
            EditorGUILayout.EndFadeGroup();
        }

        private static void HandlePopup(GUIContent content, SerializedProperty property, GUIContent[] entries, int selection)
        {
            Rect totalPosition = EditorGUILayout.GetControlRect(true, 16f, new GUILayoutOption[0]);
            GUIContent label = EditorGUI.BeginProperty(totalPosition, content, property);
            EditorGUI.BeginChangeCheck();
            EditorGUI.BeginDisabledGroup(entries.Count<GUIContent>() == 0);
            selection = EditorGUI.Popup(totalPosition, label, selection, entries);
            EditorGUI.EndDisabledGroup();
            if (EditorGUI.EndChangeCheck())
            {
                property.stringValue = entries[selection].text;
            }
            EditorGUI.EndProperty();
        }

        private void HandleTargetField(VideoRenderMode currentRenderMode)
        {
            this.m_ShowRenderTexture.target = currentRenderMode == VideoRenderMode.RenderTexture;
            if (EditorGUILayout.BeginFadeGroup(this.m_ShowRenderTexture.faded))
            {
                EditorGUILayout.PropertyField(this.m_TargetTexture, s_Styles.textureContent, new GUILayoutOption[0]);
            }
            EditorGUILayout.EndFadeGroup();
            this.m_ShowTargetCamera.target = (currentRenderMode == VideoRenderMode.CameraFarPlane) || (currentRenderMode == VideoRenderMode.CameraNearPlane);
            if (EditorGUILayout.BeginFadeGroup(this.m_ShowTargetCamera.faded))
            {
                EditorGUILayout.PropertyField(this.m_TargetCamera, s_Styles.cameraContent, new GUILayoutOption[0]);
                EditorGUILayout.Slider(this.m_TargetCameraAlpha, 0f, 1f, s_Styles.alphaContent, new GUILayoutOption[0]);
            }
            EditorGUILayout.EndFadeGroup();
            this.m_ShowRenderer.target = currentRenderMode == VideoRenderMode.MaterialOverride;
            if (EditorGUILayout.BeginFadeGroup(this.m_ShowRenderer.faded))
            {
                bool flag = base.targets.Count<UnityEngine.Object>() > 1;
                if (flag)
                {
                    EditorGUILayout.PropertyField(this.m_TargetMaterialRenderer, s_Styles.materialRendererContent, new GUILayoutOption[0]);
                }
                else
                {
                    Rect totalPosition = EditorGUILayout.GetControlRect(true, 16f, new GUILayoutOption[0]);
                    GUIContent label = EditorGUI.BeginProperty(totalPosition, s_Styles.materialRendererContent, this.m_TargetMaterialRenderer);
                    EditorGUI.BeginChangeCheck();
                    UnityEngine.Object obj2 = EditorGUI.ObjectField(totalPosition, label, GetTargetRenderer((VideoPlayer) base.target), typeof(Renderer), true);
                    if (EditorGUI.EndChangeCheck())
                    {
                        this.m_TargetMaterialRenderer.objectReferenceValue = obj2;
                    }
                    EditorGUI.EndProperty();
                }
                int materialPropertyPopupHash = GetMaterialPropertyPopupHash(base.targets);
                if (this.m_MaterialPropertyPopupContentHash != materialPropertyPopupHash)
                {
                    if (<>f__mg$cache1 == null)
                    {
                        <>f__mg$cache1 = new EntryGenerator(VideoPlayerEditor.GetMaterialPropertyNames);
                    }
                    this.m_MaterialPropertyPopupContent = BuildPopupEntries(base.targets, <>f__mg$cache1, out this.m_MaterialPropertyPopupSelection, out this.m_MaterialPropertyPopupInvalidSelections);
                }
                HandlePopup(s_Styles.materialPropertyContent, this.m_TargetMaterialProperty, this.m_MaterialPropertyPopupContent, this.m_MaterialPropertyPopupSelection);
                if ((this.m_MaterialPropertyPopupInvalidSelections > 0) || (this.m_MaterialPropertyPopupContent.Length == 0))
                {
                    GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                    GUILayout.Space(EditorGUIUtility.labelWidth);
                    if (this.m_MaterialPropertyPopupContent.Length == 0)
                    {
                        if (!flag)
                        {
                            EditorGUILayout.HelpBox(s_Styles.rendererMaterialsHaveNoTexPropsHelp, MessageType.Warning);
                        }
                        else
                        {
                            EditorGUILayout.HelpBox(s_Styles.someRendererMaterialsHaveNoTexPropsHelp, MessageType.Warning);
                        }
                    }
                    else if (!flag)
                    {
                        EditorGUILayout.HelpBox(s_Styles.invalidTexPropSelectionHelp, MessageType.Warning);
                    }
                    else if (this.m_MaterialPropertyPopupInvalidSelections == 1)
                    {
                        EditorGUILayout.HelpBox(s_Styles.oneInvalidTexPropSelectionHelp, MessageType.Warning);
                    }
                    else
                    {
                        EditorGUILayout.HelpBox(string.Format(s_Styles.someInvalidTexPropSelectionsHelp, this.m_MaterialPropertyPopupInvalidSelections), MessageType.Warning);
                    }
                    GUILayout.EndHorizontal();
                }
                else
                {
                    this.DisplayMultiMaterialInformation(this.m_MaterialPropertyPopupContentHash != materialPropertyPopupHash);
                }
                this.m_MaterialPropertyPopupContentHash = materialPropertyPopupHash;
            }
            EditorGUILayout.EndFadeGroup();
            this.m_ShowAspectRatio.target = (currentRenderMode != VideoRenderMode.MaterialOverride) && (currentRenderMode != VideoRenderMode.APIOnly);
            if (EditorGUILayout.BeginFadeGroup(this.m_ShowAspectRatio.faded))
            {
                EditorGUILayout.PropertyField(this.m_AspectRatio, s_Styles.aspectRatioLabel, new GUILayoutOption[0]);
            }
            EditorGUILayout.EndFadeGroup();
        }

        private void OnDisable()
        {
            this.m_ShowRenderTexture.valueChanged.RemoveListener(new UnityAction(this.Repaint));
            this.m_ShowTargetCamera.valueChanged.RemoveListener(new UnityAction(this.Repaint));
            this.m_ShowRenderer.valueChanged.RemoveListener(new UnityAction(this.Repaint));
            this.m_ShowMaterialProperty.valueChanged.RemoveListener(new UnityAction(this.Repaint));
            this.m_DataSourceIsClip.valueChanged.RemoveListener(new UnityAction(this.Repaint));
            this.m_ShowAspectRatio.valueChanged.RemoveListener(new UnityAction(this.Repaint));
            this.m_ShowAudioControls.valueChanged.RemoveListener(new UnityAction(this.Repaint));
        }

        private void OnEnable()
        {
            this.m_ShowRenderTexture.valueChanged.AddListener(new UnityAction(this.Repaint));
            this.m_ShowTargetCamera.valueChanged.AddListener(new UnityAction(this.Repaint));
            this.m_ShowRenderer.valueChanged.AddListener(new UnityAction(this.Repaint));
            this.m_ShowMaterialProperty.valueChanged.AddListener(new UnityAction(this.Repaint));
            this.m_DataSourceIsClip.valueChanged.AddListener(new UnityAction(this.Repaint));
            this.m_ShowAspectRatio.valueChanged.AddListener(new UnityAction(this.Repaint));
            this.m_ShowAudioControls.valueChanged.AddListener(new UnityAction(this.Repaint));
            this.m_DataSource = base.serializedObject.FindProperty("m_DataSource");
            this.m_VideoClip = base.serializedObject.FindProperty("m_VideoClip");
            this.m_Url = base.serializedObject.FindProperty("m_Url");
            this.m_PlayOnAwake = base.serializedObject.FindProperty("m_PlayOnAwake");
            this.m_WaitForFirstFrame = base.serializedObject.FindProperty("m_WaitForFirstFrame");
            this.m_Looping = base.serializedObject.FindProperty("m_Looping");
            this.m_PlaybackSpeed = base.serializedObject.FindProperty("m_PlaybackSpeed");
            this.m_RenderMode = base.serializedObject.FindProperty("m_RenderMode");
            this.m_TargetTexture = base.serializedObject.FindProperty("m_TargetTexture");
            this.m_TargetCamera = base.serializedObject.FindProperty("m_TargetCamera");
            this.m_TargetMaterialRenderer = base.serializedObject.FindProperty("m_TargetMaterialRenderer");
            this.m_TargetMaterialProperty = base.serializedObject.FindProperty("m_TargetMaterialProperty");
            this.m_AspectRatio = base.serializedObject.FindProperty("m_AspectRatio");
            this.m_TargetCameraAlpha = base.serializedObject.FindProperty("m_TargetCameraAlpha");
            this.m_AudioOutputMode = base.serializedObject.FindProperty("m_AudioOutputMode");
            this.m_ControlledAudioTrackCount = base.serializedObject.FindProperty("m_ControlledAudioTrackCount");
            this.m_EnabledAudioTracks = base.serializedObject.FindProperty("m_EnabledAudioTracks");
            this.m_TargetAudioSources = base.serializedObject.FindProperty("m_TargetAudioSources");
            this.m_DirectAudioVolumes = base.serializedObject.FindProperty("m_DirectAudioVolumes");
            this.m_DirectAudioMutes = base.serializedObject.FindProperty("m_DirectAudioMutes");
            this.m_ShowRenderTexture.value = this.m_RenderMode.intValue == 2;
            this.m_ShowTargetCamera.value = (this.m_RenderMode.intValue == 0) || (this.m_RenderMode.intValue == 1);
            this.m_ShowRenderer.value = this.m_RenderMode.intValue == 3;
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new EntryGenerator(VideoPlayerEditor.GetMaterialPropertyNames);
            }
            this.m_MaterialPropertyPopupContent = BuildPopupEntries(base.targets, <>f__mg$cache0, out this.m_MaterialPropertyPopupSelection, out this.m_MaterialPropertyPopupInvalidSelections);
            this.m_MaterialPropertyPopupContentHash = GetMaterialPropertyPopupHash(base.targets);
            this.m_ShowMaterialProperty.value = (base.targets.Count<UnityEngine.Object>() > 1) || ((this.m_MaterialPropertyPopupSelection >= 0) && (this.m_MaterialPropertyPopupContent.Length > 0));
            this.m_DataSourceIsClip.value = this.m_DataSource.intValue == 0;
            this.m_ShowAspectRatio.value = (this.m_RenderMode.intValue != 3) && (this.m_RenderMode.intValue != 4);
            this.m_ShowAudioControls.value = this.m_AudioOutputMode.intValue != 0;
            VideoPlayer target = base.target as VideoPlayer;
            target.prepareCompleted += new VideoPlayer.EventHandler(this.PrepareCompleted);
            this.m_AudioTrackInfos = new List<AudioTrackInfo>();
        }

        public override void OnInspectorGUI()
        {
            if (s_Styles == null)
            {
                s_Styles = new Styles();
            }
            base.serializedObject.Update();
            EditorGUILayout.PropertyField(this.m_DataSource, s_Styles.dataSourceContent, new GUILayoutOption[0]);
            this.HandleDataSourceField();
            EditorGUILayout.PropertyField(this.m_PlayOnAwake, s_Styles.playOnAwakeContent, new GUILayoutOption[0]);
            EditorGUILayout.PropertyField(this.m_WaitForFirstFrame, s_Styles.waitForFirstFrameContent, new GUILayoutOption[0]);
            EditorGUILayout.PropertyField(this.m_Looping, s_Styles.loopContent, new GUILayoutOption[0]);
            EditorGUILayout.Slider(this.m_PlaybackSpeed, 0f, 10f, s_Styles.playbackSpeedContent, new GUILayoutOption[0]);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(this.m_RenderMode, s_Styles.renderModeContent, new GUILayoutOption[0]);
            if (this.m_RenderMode.hasMultipleDifferentValues)
            {
                EditorGUILayout.HelpBox(s_Styles.selectUniformVideoRenderModeHelp, MessageType.Warning, false);
            }
            else
            {
                VideoRenderMode intValue = (VideoRenderMode) this.m_RenderMode.intValue;
                this.HandleTargetField(intValue);
            }
            this.HandleAudio();
            base.serializedObject.ApplyModifiedProperties();
        }

        private void PrepareCompleted(VideoPlayer vp)
        {
            base.Repaint();
        }

        internal class AudioTrackInfo
        {
            public ushort channelCount = 0;
            public GUIContent content;
            public string language = "";
        }

        private delegate List<string> EntryGenerator(UnityEngine.Object obj, bool multiSelect, out int selection, out bool invalidSelection);

        private class Styles
        {
            public GUIContent alphaContent = EditorGUIUtility.TextContent("Alpha|A value less than 1.0 will reveal the content behind the video.");
            public GUIContent aspectRatioLabel = EditorGUIUtility.TextContent("Aspect Ratio");
            public string audioControlsNotEditableHelp = "Audio controls not editable when using muliple selection.";
            public GUIContent audioOutputModeContent = EditorGUIUtility.TextContent("Audio Output Mode|Where the audio in the movie will be output.");
            public GUIContent audioSourceContent = EditorGUIUtility.TextContent("Audio Source|AudioSource component tha will receive this track's audio samples.");
            public GUIContent browseContent = EditorGUIUtility.TextContent("Browse...|Click to set a file:// URL.  http:// URLs have to be written or copy-pasted manually.");
            public GUIContent cameraContent = EditorGUIUtility.TextContent("Camera|Camera where the images will be drawn, behind (Back Plane) or in front of (Front Plane) of the scene.");
            public GUIContent controlledAudioTrackCountContent = EditorGUIUtility.TextContent("Controlled Tracks|How many audio tracks will the player control.  The actual number of tracks is only known during playback when the source is a URL.");
            public GUIContent dataSourceContent = EditorGUIUtility.TextContent("Source|Type of source the movie will be read from.");
            public string enableDecodingTooltip = "Enable decoding for this track.  Only effective when not playing.  When playing from a URL, track details are shown only while playing back.";
            public string invalidTexPropSelectionHelp = "Invalid texture property selection.";
            public GUIContent loopContent = EditorGUIUtility.TextContent("Loop|Start playback at the beginning when end is reached.");
            public GUIContent materialPropertyContent = EditorGUIUtility.TextContent("Material Property|Texture property of the current Material that will receive the images.");
            public GUIContent materialRendererContent = EditorGUIUtility.TextContent("Renderer|Renderer that will receive the images. Defaults to the first renderer on the game object.");
            public GUIContent muteLabel = EditorGUIUtility.TextContent("Mute");
            public string oneInvalidTexPropSelectionHelp = "1 selected object has an invalid texture property selection.";
            public GUIContent playbackSpeedContent = EditorGUIUtility.TextContent("Playback Speed|Increase or decrease the playback speed. 1.0 is the normal speed.");
            public GUIContent playOnAwakeContent = EditorGUIUtility.TextContent("Play On Awake|Start playback as soon as the game is started.");
            public string rendererMaterialsHaveNoTexPropsHelp = "Renderer materials have no texture properties.";
            public GUIContent renderModeContent = EditorGUIUtility.TextContent("Render Mode|Type of object on which the played images will be drawn.");
            public string selectMovieFile = "Select movie file.";
            public string selectUniformAudioOutputModeHelp = "Select a uniform audio target before audio settings can be edited.";
            public string selectUniformAudioTracksHelp = "Only sources with the same number of audio tracks can be edited during multi-selection.";
            public string selectUniformVideoRenderModeHelp = "Select a uniform video render mode type before a target camera, render texture or material parameter can be selected.";
            public string selectUniformVideoSourceHelp = "Select a uniform video source type before a video clip or URL can be selected.";
            public string someInvalidTexPropSelectionsHelp = "{0} selected objects have invalid texture property selections.";
            public string someRendererMaterialsHaveNoTexPropsHelp = "Some selected renderers have materials with no texture properties.";
            public string texPropInAllMaterialsHelp = "Texture property appears in all renderer materials.";
            public string texPropInSomeMaterialsHelp = "Texture property appears in {0} out of {1} renderer materials.";
            public GUIContent textureContent = EditorGUIUtility.TextContent("Target Texture|RenderTexture where the images will be drawn.  RenderTextures can be created under the Assets folder and the used on other objects.");
            public GUIContent urlContent = EditorGUIUtility.TextContent("URL|URLs can be http:// or file://. File URLs can be relative [file://] or absolute [file:///].  For file URLs, the prefix is optional.");
            public GUIContent videoClipContent = EditorGUIUtility.TextContent("Video Clip|VideoClips can be imported using the asset pipeline.");
            public GUIContent volumeLabel = EditorGUIUtility.TextContent("Volume");
            public GUIContent waitForFirstFrameContent = EditorGUIUtility.TextContent("Wait For First Frame|Wait for first frame to be ready before starting playback. When on, player time will only start increasing when the first image is ready.  When off, the first few frames may be skipped while clip preparation is ongoing.");
        }
    }
}

