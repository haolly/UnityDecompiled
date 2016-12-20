﻿namespace UnityEditor
{
    using System;
    using System.Collections.Generic;
    using UnityEditorInternal;
    using UnityEngine;
    using UnityEngine.Profiling;
    using UnityEngine.Scripting;

    [EditorWindowTitle(title="Profiler", useTypeNameAsIconName=true)]
    internal class ProfilerWindow : EditorWindow, IProfilerWindowController
    {
        private const float kBaseIndent = 8f;
        private const float kColumnSize = 80f;
        private const int kFirst = -999999;
        private const float kFoldoutSize = 14f;
        private const float kIndentPx = 16f;
        private const int kLast = 0xf423f;
        private const float kNameColumnSize = 350f;
        private const string kProfilerColumnSettings = "VisibleProfilerColumnsV2";
        private const string kProfilerDetailColumnSettings = "VisibleProfilerDetailColumns";
        private const string kProfilerEnabledSessionKey = "ProfilerEnabled";
        private const string kProfilerGPUColumnSettings = "VisibleProfilerGPUColumns";
        private const string kProfilerGPUDetailColumnSettings = "VisibleProfilerGPUDetailColumns";
        private const string kProfilerVisibleGraphsSettings = "VisibleProfilerGraphs";
        private const float kRowHeight = 16f;
        private const string kSearchControlName = "ProfilerSearchField";
        private const float kSmallMargin = 4f;
        private string m_ActiveNativePlatformSupportModule;
        private AttachProfilerUI m_AttachProfilerUI;
        [SerializeField]
        private AudioProfilerClipTreeViewState m_AudioProfilerClipTreeViewState;
        private AudioProfilerClipView m_AudioProfilerClipView;
        private AudioProfilerClipViewBackend m_AudioProfilerClipViewBackend;
        private AudioProfilerDSPView m_AudioProfilerDSPView;
        [SerializeField]
        private AudioProfilerGroupTreeViewState m_AudioProfilerGroupTreeViewState;
        private AudioProfilerGroupView m_AudioProfilerGroupView;
        private AudioProfilerGroupViewBackend m_AudioProfilerGroupViewBackend;
        private float m_ChartMaxClamp;
        private float[] m_ChartOldMax;
        private ProfilerChart[] m_Charts;
        private ProfilerHierarchyGUI m_CPUDetailHierarchyGUI;
        private ProfilerHierarchyGUI m_CPUHierarchyGUI;
        private ProfilerTimelineGUI m_CPUTimelineGUI;
        private ProfilerArea m_CurrentArea;
        private int m_CurrentFrame;
        [SerializeField]
        private float m_DSPGraphZoomFactor;
        private bool m_FocusSearchField = false;
        private bool m_GatherObjectReferences;
        private ProfilerHierarchyGUI m_GPUDetailHierarchyGUI;
        private ProfilerHierarchyGUI m_GPUHierarchyGUI;
        private Vector2 m_GraphPos;
        [SerializeField]
        private bool m_HighlightAudibleDSPChains;
        private int m_LastAudioProfilerFrame;
        private int m_LastFrameFromTick;
        private MemoryTreeListClickable m_MemoryListView;
        private SplitterState m_NetworkSplit;
        private Vector2[] m_PaneScroll;
        private Vector2 m_PaneScroll_AudioChannels;
        private Vector2 m_PaneScroll_AudioClips;
        private Vector2 m_PaneScroll_AudioDSP;
        private int m_PrevLastFrame;
        private static List<ProfilerWindow> m_ProfilerWindows = new List<ProfilerWindow>();
        [SerializeField]
        private bool m_Recording;
        private MemoryTreeList m_ReferenceListView;
        private string m_SearchString = "";
        private ProfilerMemoryRecordMode m_SelectedMemRecordMode;
        private ProfilerAudioView m_ShowDetailedAudioPane;
        private ProfilerMemoryView m_ShowDetailedMemoryPane;
        [SerializeField]
        private bool m_ShowInactiveDSPChains;
        [SerializeField]
        private bool m_TimelineViewDetail;
        private SplitterState m_VertSplit;
        private SplitterState m_ViewSplit;
        private ProfilerViewType m_ViewType;
        private static Styles ms_Styles;
        private bool[] msgFoldouts;
        private string[] msgNames;
        private readonly char s_CheckMark;
        private static readonly int s_HashControlID = "ProfilerSearchField".GetHashCode();

        public ProfilerWindow()
        {
            float[] relativeSizes = new float[] { 50f, 50f };
            int[] minSizes = new int[] { 50, 50 };
            this.m_VertSplit = new SplitterState(relativeSizes, minSizes, null);
            float[] singleArray2 = new float[] { 70f, 30f };
            int[] numArray2 = new int[] { 450, 50 };
            this.m_ViewSplit = new SplitterState(singleArray2, numArray2, null);
            float[] singleArray3 = new float[] { 20f, 80f };
            int[] numArray3 = new int[] { 100, 100 };
            this.m_NetworkSplit = new SplitterState(singleArray3, numArray3, null);
            this.m_AttachProfilerUI = new AttachProfilerUI();
            this.m_GraphPos = Vector2.zero;
            this.m_PaneScroll = new Vector2[9];
            this.m_PaneScroll_AudioChannels = Vector2.zero;
            this.m_PaneScroll_AudioDSP = Vector2.zero;
            this.m_PaneScroll_AudioClips = Vector2.zero;
            this.m_ViewType = ProfilerViewType.Hierarchy;
            this.m_CurrentArea = ProfilerArea.CPU;
            this.m_ShowDetailedMemoryPane = ProfilerMemoryView.Simple;
            this.m_ShowDetailedAudioPane = ProfilerAudioView.Stats;
            this.m_ShowInactiveDSPChains = false;
            this.m_HighlightAudibleDSPChains = true;
            this.m_DSPGraphZoomFactor = 1f;
            this.m_CurrentFrame = -1;
            this.m_LastFrameFromTick = -1;
            this.m_PrevLastFrame = -1;
            this.m_LastAudioProfilerFrame = -1;
            this.m_ChartOldMax = new float[] { -1f, -1f };
            this.m_ChartMaxClamp = 70000f;
            this.m_TimelineViewDetail = false;
            this.m_GatherObjectReferences = true;
            this.m_AudioProfilerGroupView = null;
            this.m_AudioProfilerClipView = null;
            this.m_SelectedMemRecordMode = ProfilerMemoryRecordMode.None;
            this.s_CheckMark = '✔';
            this.msgNames = new string[] { "UserMessage", "ObjectDestroy", "ClientRpc", "ObjectSpawn", "Owner", "Command", "LocalPlayerTransform", "SyncEvent", "SyncVars", "SyncList", "ObjectSpawnScene", "NetworkInfo", "SpawnFinished", "ObjectHide", "CRC", "ClientAuthority" };
            this.msgFoldouts = new bool[] { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false };
        }

        private void AddAreaClick(object userData, string[] options, int selected)
        {
            this.m_Charts[selected].active = true;
        }

        private void AudioProfilerToggle(ProfilerCaptureFlags toggleFlag)
        {
            bool flag = (AudioSettings.profilerCaptureFlags & toggleFlag) != 0;
            bool flag2 = GUILayout.Toggle(flag, "Record", EditorStyles.toolbarButton, new GUILayoutOption[0]);
            if (flag != flag2)
            {
                ProfilerDriver.SetAudioCaptureFlags((AudioSettings.profilerCaptureFlags & ~toggleFlag) | (!flag2 ? 0 : toggleFlag));
            }
        }

        private void Awake()
        {
            if (Profiler.supported)
            {
                this.m_Recording = SessionState.GetBool("ProfilerEnabled", true);
                Profiler.enabled = this.m_Recording;
                this.m_SelectedMemRecordMode = ProfilerDriver.memoryRecordMode;
            }
        }

        private void BuildColumns()
        {
            ProfilerColumn[] columnsToShow = new ProfilerColumn[] { ProfilerColumn.FunctionName };
            ProfilerColumn[] columns = new ProfilerColumn[] { ProfilerColumn.ObjectName };
            this.m_CPUHierarchyGUI = new ProfilerHierarchyGUI(this, "VisibleProfilerColumnsV2", columnsToShow, ProfilerColumnNames(columnsToShow), false, ProfilerColumn.TotalTime);
            this.m_CPUTimelineGUI = new ProfilerTimelineGUI(this);
            string text = EditorGUIUtility.TextContent("Object").text;
            string[] columnNames = ProfilerColumnNames(columns);
            columnNames[0] = text;
            this.m_CPUDetailHierarchyGUI = new ProfilerHierarchyGUI(this, "VisibleProfilerDetailColumns", columns, columnNames, true, ProfilerColumn.TotalTime);
            ProfilerColumn[] columnArray3 = new ProfilerColumn[] { ProfilerColumn.FunctionName };
            ProfilerColumn[] columnArray4 = new ProfilerColumn[] { ProfilerColumn.ObjectName };
            this.m_GPUHierarchyGUI = new ProfilerHierarchyGUI(this, "VisibleProfilerGPUColumns", columnArray3, ProfilerColumnNames(columnArray3), false, ProfilerColumn.TotalGPUTime);
            columnNames = ProfilerColumnNames(columnArray4);
            columnNames[0] = text;
            this.m_GPUDetailHierarchyGUI = new ProfilerHierarchyGUI(this, "VisibleProfilerGPUDetailColumns", columnArray4, columnNames, true, ProfilerColumn.TotalGPUTime);
        }

        private void CheckForPlatformModuleChange()
        {
            if (this.m_ActiveNativePlatformSupportModule != EditorUtility.GetActiveNativePlatformSupportModuleName())
            {
                ProfilerDriver.ResetHistory();
                this.Initialize();
                this.m_ActiveNativePlatformSupportModule = EditorUtility.GetActiveNativePlatformSupportModuleName();
            }
        }

        private static bool CheckFrameData(ProfilerProperty property)
        {
            if (!property.frameDataReady)
            {
                GUILayout.Label(ms_Styles.noData, ms_Styles.background, new GUILayoutOption[0]);
                return false;
            }
            return true;
        }

        public void ClearSelectedPropertyPath()
        {
            if (ProfilerDriver.selectedPropertyPath != string.Empty)
            {
                this.m_CPUHierarchyGUI.selectedIndex = -1;
                ProfilerDriver.selectedPropertyPath = string.Empty;
                this.UpdateCharts();
            }
        }

        public ProfilerProperty CreateProperty(bool details)
        {
            ProfilerProperty property = new ProfilerProperty();
            ProfilerColumn profilerSortColumn = (this.m_CurrentArea != ProfilerArea.CPU) ? (!details ? this.m_GPUHierarchyGUI.sortType : this.m_GPUDetailHierarchyGUI.sortType) : (!details ? this.m_CPUHierarchyGUI.sortType : this.m_CPUDetailHierarchyGUI.sortType);
            property.SetRoot(this.GetActiveVisibleFrameIndex(), profilerSortColumn, this.m_ViewType);
            property.onlyShowGPUSamples = this.m_CurrentArea == ProfilerArea.GPU;
            return property;
        }

        private void DrawAudioPane()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
            ProfilerAudioView showDetailedAudioPane = this.m_ShowDetailedAudioPane;
            if (GUILayout.Toggle(showDetailedAudioPane == ProfilerAudioView.Stats, "Stats", EditorStyles.toolbarButton, new GUILayoutOption[0]))
            {
                showDetailedAudioPane = ProfilerAudioView.Stats;
            }
            if (GUILayout.Toggle(showDetailedAudioPane == ProfilerAudioView.Channels, "Channels", EditorStyles.toolbarButton, new GUILayoutOption[0]))
            {
                showDetailedAudioPane = ProfilerAudioView.Channels;
            }
            if (GUILayout.Toggle(showDetailedAudioPane == ProfilerAudioView.Groups, "Groups", EditorStyles.toolbarButton, new GUILayoutOption[0]))
            {
                showDetailedAudioPane = ProfilerAudioView.Groups;
            }
            if (GUILayout.Toggle(showDetailedAudioPane == ProfilerAudioView.ChannelsAndGroups, "Channels and groups", EditorStyles.toolbarButton, new GUILayoutOption[0]))
            {
                showDetailedAudioPane = ProfilerAudioView.ChannelsAndGroups;
            }
            if (Unsupported.IsDeveloperBuild() && GUILayout.Toggle(showDetailedAudioPane == ProfilerAudioView.DSPGraph, "DSP Graph", EditorStyles.toolbarButton, new GUILayoutOption[0]))
            {
                showDetailedAudioPane = ProfilerAudioView.DSPGraph;
            }
            if (Unsupported.IsDeveloperBuild() && GUILayout.Toggle(showDetailedAudioPane == ProfilerAudioView.Clips, "Clips", EditorStyles.toolbarButton, new GUILayoutOption[0]))
            {
                showDetailedAudioPane = ProfilerAudioView.Clips;
            }
            if (showDetailedAudioPane != this.m_ShowDetailedAudioPane)
            {
                this.m_ShowDetailedAudioPane = showDetailedAudioPane;
                this.m_LastAudioProfilerFrame = -1;
            }
            if (this.m_ShowDetailedAudioPane == ProfilerAudioView.Stats)
            {
                GUILayout.Space(5f);
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                this.DrawOverviewText(this.m_CurrentArea);
            }
            else if (this.m_ShowDetailedAudioPane == ProfilerAudioView.DSPGraph)
            {
                GUILayout.Space(5f);
                this.AudioProfilerToggle(ProfilerCaptureFlags.DSPNodes);
                GUILayout.Space(5f);
                this.m_ShowInactiveDSPChains = GUILayout.Toggle(this.m_ShowInactiveDSPChains, "Show inactive", EditorStyles.toolbarButton, new GUILayoutOption[0]);
                if (this.m_ShowInactiveDSPChains)
                {
                    this.m_HighlightAudibleDSPChains = GUILayout.Toggle(this.m_HighlightAudibleDSPChains, "Highlight audible", EditorStyles.toolbarButton, new GUILayoutOption[0]);
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                Rect position = GUILayoutUtility.GetRect(20f, 10000f, (float) 10f, (float) 20000f);
                this.m_PaneScroll_AudioDSP = GUI.BeginScrollView(position, this.m_PaneScroll_AudioDSP, new Rect(0f, 0f, 10000f, 20000f));
                Rect clippingRect = new Rect(this.m_PaneScroll_AudioDSP.x, this.m_PaneScroll_AudioDSP.y, position.width, position.height);
                if (this.m_AudioProfilerDSPView == null)
                {
                    this.m_AudioProfilerDSPView = new AudioProfilerDSPView();
                }
                ProfilerProperty property = this.CreateProperty(false);
                if (CheckFrameData(property))
                {
                    this.m_AudioProfilerDSPView.OnGUI(clippingRect, property, this.m_ShowInactiveDSPChains, this.m_HighlightAudibleDSPChains, ref this.m_DSPGraphZoomFactor, ref this.m_PaneScroll_AudioDSP);
                }
                property.Cleanup();
                GUI.EndScrollView();
                base.Repaint();
            }
            else if (this.m_ShowDetailedAudioPane == ProfilerAudioView.Clips)
            {
                GUILayout.Space(5f);
                this.AudioProfilerToggle(ProfilerCaptureFlags.Clips);
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                Rect rect3 = GUILayoutUtility.GetRect(20f, 20000f, (float) 10f, (float) 10000f);
                Rect rect4 = new Rect(rect3.x, rect3.y, 230f, rect3.height);
                Rect rect = new Rect(rect4.xMax, rect3.y, rect3.width - rect4.width, rect3.height);
                string overviewText = ProfilerDriver.GetOverviewText(this.m_CurrentArea, this.GetActiveVisibleFrameIndex());
                Vector2 vector = EditorStyles.wordWrappedLabel.CalcSize(GUIContent.Temp(overviewText));
                this.m_PaneScroll_AudioClips = GUI.BeginScrollView(rect4, this.m_PaneScroll_AudioClips, new Rect(0f, 0f, vector.x, vector.y));
                GUI.Label(new Rect(3f, 3f, vector.x, vector.y), overviewText, EditorStyles.wordWrappedLabel);
                GUI.EndScrollView();
                EditorGUI.DrawRect(new Rect(rect4.xMax - 1f, rect4.y, 1f, rect4.height), Color.black);
                if (this.m_AudioProfilerClipTreeViewState == null)
                {
                    this.m_AudioProfilerClipTreeViewState = new AudioProfilerClipTreeViewState();
                }
                if (this.m_AudioProfilerClipViewBackend == null)
                {
                    this.m_AudioProfilerClipViewBackend = new AudioProfilerClipViewBackend(this.m_AudioProfilerClipTreeViewState);
                }
                ProfilerProperty property2 = this.CreateProperty(false);
                if (CheckFrameData(property2))
                {
                    if ((this.m_CurrentFrame == -1) || (this.m_LastAudioProfilerFrame != this.m_CurrentFrame))
                    {
                        this.m_LastAudioProfilerFrame = this.m_CurrentFrame;
                        AudioProfilerClipInfo[] audioProfilerClipInfo = property2.GetAudioProfilerClipInfo();
                        if ((audioProfilerClipInfo != null) && (audioProfilerClipInfo.Length > 0))
                        {
                            List<AudioProfilerClipInfoWrapper> data = new List<AudioProfilerClipInfoWrapper>();
                            foreach (AudioProfilerClipInfo info in audioProfilerClipInfo)
                            {
                                data.Add(new AudioProfilerClipInfoWrapper(info, property2.GetAudioProfilerNameByOffset(info.assetNameOffset)));
                            }
                            this.m_AudioProfilerClipViewBackend.SetData(data);
                            if (this.m_AudioProfilerClipView == null)
                            {
                                this.m_AudioProfilerClipView = new AudioProfilerClipView(this, this.m_AudioProfilerClipTreeViewState);
                                this.m_AudioProfilerClipView.Init(rect, this.m_AudioProfilerClipViewBackend);
                            }
                        }
                    }
                    if (this.m_AudioProfilerClipView != null)
                    {
                        this.m_AudioProfilerClipView.OnGUI(rect);
                    }
                }
                property2.Cleanup();
            }
            else
            {
                GUILayout.Space(5f);
                this.AudioProfilerToggle(ProfilerCaptureFlags.Channels);
                GUILayout.Space(5f);
                bool flag = GUILayout.Toggle(AudioUtil.resetAllAudioClipPlayCountsOnPlay, "Reset play count on play", EditorStyles.toolbarButton, new GUILayoutOption[0]);
                if (flag != AudioUtil.resetAllAudioClipPlayCountsOnPlay)
                {
                    AudioUtil.resetAllAudioClipPlayCountsOnPlay = flag;
                }
                if (Unsupported.IsDeveloperBuild())
                {
                    GUILayout.Space(5f);
                    bool @bool = EditorPrefs.GetBool("AudioProfilerShowAllGroups");
                    bool flag3 = GUILayout.Toggle(@bool, "Show all groups (dev-builds only)", EditorStyles.toolbarButton, new GUILayoutOption[0]);
                    if (@bool != flag3)
                    {
                        EditorPrefs.SetBool("AudioProfilerShowAllGroups", flag3);
                    }
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                Rect rect6 = GUILayoutUtility.GetRect(20f, 20000f, (float) 10f, (float) 10000f);
                Rect rect7 = new Rect(rect6.x, rect6.y, 230f, rect6.height);
                Rect rect8 = new Rect(rect7.xMax, rect6.y, rect6.width - rect7.width, rect6.height);
                string t = ProfilerDriver.GetOverviewText(this.m_CurrentArea, this.GetActiveVisibleFrameIndex());
                Vector2 vector2 = EditorStyles.wordWrappedLabel.CalcSize(GUIContent.Temp(t));
                this.m_PaneScroll_AudioChannels = GUI.BeginScrollView(rect7, this.m_PaneScroll_AudioChannels, new Rect(0f, 0f, vector2.x, vector2.y));
                GUI.Label(new Rect(3f, 3f, vector2.x, vector2.y), t, EditorStyles.wordWrappedLabel);
                GUI.EndScrollView();
                EditorGUI.DrawRect(new Rect(rect7.xMax - 1f, rect7.y, 1f, rect7.height), Color.black);
                if (this.m_AudioProfilerGroupTreeViewState == null)
                {
                    this.m_AudioProfilerGroupTreeViewState = new AudioProfilerGroupTreeViewState();
                }
                if (this.m_AudioProfilerGroupViewBackend == null)
                {
                    this.m_AudioProfilerGroupViewBackend = new AudioProfilerGroupViewBackend(this.m_AudioProfilerGroupTreeViewState);
                }
                ProfilerProperty property3 = this.CreateProperty(false);
                if (CheckFrameData(property3))
                {
                    if ((this.m_CurrentFrame == -1) || (this.m_LastAudioProfilerFrame != this.m_CurrentFrame))
                    {
                        this.m_LastAudioProfilerFrame = this.m_CurrentFrame;
                        AudioProfilerGroupInfo[] audioProfilerGroupInfo = property3.GetAudioProfilerGroupInfo();
                        if ((audioProfilerGroupInfo != null) && (audioProfilerGroupInfo.Length > 0))
                        {
                            List<AudioProfilerGroupInfoWrapper> list2 = new List<AudioProfilerGroupInfoWrapper>();
                            foreach (AudioProfilerGroupInfo info2 in audioProfilerGroupInfo)
                            {
                                bool flag4 = (info2.flags & 0x40) != 0;
                                if (((this.m_ShowDetailedAudioPane != ProfilerAudioView.Channels) || !flag4) && ((this.m_ShowDetailedAudioPane != ProfilerAudioView.Groups) || flag4))
                                {
                                    list2.Add(new AudioProfilerGroupInfoWrapper(info2, property3.GetAudioProfilerNameByOffset(info2.assetNameOffset), property3.GetAudioProfilerNameByOffset(info2.objectNameOffset), this.m_ShowDetailedAudioPane == ProfilerAudioView.Channels));
                                }
                            }
                            this.m_AudioProfilerGroupViewBackend.SetData(list2);
                            if (this.m_AudioProfilerGroupView == null)
                            {
                                this.m_AudioProfilerGroupView = new AudioProfilerGroupView(this, this.m_AudioProfilerGroupTreeViewState);
                                this.m_AudioProfilerGroupView.Init(rect8, this.m_AudioProfilerGroupViewBackend);
                            }
                        }
                    }
                    if (this.m_AudioProfilerGroupView != null)
                    {
                        this.m_AudioProfilerGroupView.OnGUI(rect8, this.m_ShowDetailedAudioPane == ProfilerAudioView.Channels);
                    }
                }
                property3.Cleanup();
            }
        }

        private static void DrawBackground(int row, bool selected)
        {
            Rect position = new Rect(1f, 16f * row, GUIClip.visibleRect.width, 16f);
            GUIStyle style = ((row % 2) != 0) ? ms_Styles.entryOdd : ms_Styles.entryEven;
            if (Event.current.type == EventType.Repaint)
            {
                style.Draw(position, GUIContent.none, false, false, selected, false);
            }
        }

        private void DrawCPUOrRenderingPane(ProfilerHierarchyGUI mainPane, ProfilerHierarchyGUI detailPane, ProfilerTimelineGUI timelinePane)
        {
            ProfilerProperty property = this.CreateProperty(false);
            this.DrawCPUOrRenderingToolbar(property);
            if (!CheckFrameData(property))
            {
                property.Cleanup();
            }
            else if ((timelinePane != null) && (this.m_ViewType == ProfilerViewType.Timeline))
            {
                float height = this.m_VertSplit.realSizes[1];
                height -= EditorStyles.toolbar.CalcHeight(GUIContent.none, 10f) + 2f;
                timelinePane.DoGUI(this.GetActiveVisibleFrameIndex(), base.position.width, base.position.height - height, height, this.m_TimelineViewDetail);
                property.Cleanup();
            }
            else
            {
                SplitterGUILayout.BeginHorizontalSplit(this.m_ViewSplit, new GUILayoutOption[0]);
                GUILayout.BeginVertical(new GUILayoutOption[0]);
                bool expandAll = false;
                mainPane.DoGUI(property, this.m_SearchString, expandAll);
                property.Cleanup();
                GUILayout.EndVertical();
                GUILayout.BeginVertical(new GUILayoutOption[0]);
                ProfilerProperty property2 = this.CreateProperty(true);
                ProfilerProperty detailedProperty = mainPane.GetDetailedProperty(property2);
                property2.Cleanup();
                if (detailedProperty != null)
                {
                    detailPane.DoGUI(detailedProperty, string.Empty, expandAll);
                    detailedProperty.Cleanup();
                }
                else
                {
                    DrawEmptyCPUOrRenderingDetailPane();
                }
                GUILayout.EndVertical();
                SplitterGUILayout.EndHorizontalSplit();
            }
        }

        private void DrawCPUOrRenderingToolbar(ProfilerProperty property)
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
            string[] displayedOptions = new string[] { "Hierarchy", "Timeline", "Raw Hierarchy" };
            int[] numArray1 = new int[3];
            numArray1[1] = 1;
            numArray1[2] = 2;
            int[] optionValues = numArray1;
            GUILayoutOption[] options = new GUILayoutOption[] { GUILayout.Width(120f) };
            this.m_ViewType = (ProfilerViewType) EditorGUILayout.IntPopup((int) this.m_ViewType, displayedOptions, optionValues, EditorStyles.toolbarDropDown, options);
            GUILayout.FlexibleSpace();
            GUILayout.Label(string.Format("CPU:{0}ms   GPU:{1}ms", property.frameTime, property.frameGpuTime), EditorStyles.miniLabel, new GUILayoutOption[0]);
            GUI.enabled = ProfilerDriver.GetNextFrameIndex(this.m_CurrentFrame) == -1;
            if (GUILayout.Button(!GUI.enabled ? ms_Styles.noFrameDebugger : ms_Styles.frameDebugger, EditorStyles.toolbarButton, new GUILayoutOption[0]))
            {
                FrameDebuggerWindow.ShowFrameDebuggerWindow().EnableIfNeeded();
            }
            GUI.enabled = true;
            if (ProfilerInstrumentationPopup.InstrumentationEnabled && GUILayout.Button(ms_Styles.profilerInstrumentation, EditorStyles.toolbarDropDown, new GUILayoutOption[0]))
            {
                ProfilerInstrumentationPopup.Show(GUILayoutUtility.topLevel.GetLast());
            }
            GUILayout.FlexibleSpace();
            if (this.m_ViewType == ProfilerViewType.Timeline)
            {
                this.m_TimelineViewDetail = GUILayout.Toggle(this.m_TimelineViewDetail, ms_Styles.timelineHighDetail, EditorStyles.toolbarButton, new GUILayoutOption[0]);
                ms_Styles.memRecord.text = "Mem Record";
                if (this.m_SelectedMemRecordMode != ProfilerMemoryRecordMode.None)
                {
                    string text = ms_Styles.memRecord.text;
                    object[] objArray1 = new object[] { text, " [", this.s_CheckMark, "]" };
                    ms_Styles.memRecord.text = string.Concat(objArray1);
                }
                GUILayoutOption[] optionArray2 = new GUILayoutOption[] { GUILayout.Width(100f) };
                Rect position = GUILayoutUtility.GetRect(ms_Styles.memRecord, EditorStyles.toolbarDropDown, optionArray2);
                if (EditorGUI.ButtonMouseDown(position, ms_Styles.memRecord, FocusType.Passive, EditorStyles.toolbarDropDown))
                {
                    string[] strArray2 = new string[] { "None", "Sample only", "Callstack (fast)", "Callstack (full)" };
                    bool[] enabled = new bool[strArray2.Length];
                    for (int i = 0; i < strArray2.Length; i++)
                    {
                        enabled[i] = true;
                    }
                    int[] selected = new int[] { this.m_SelectedMemRecordMode };
                    EditorUtility.DisplayCustomMenu(position, strArray2, enabled, selected, new EditorUtility.SelectMenuItemFunction(this.MemRecordModeClick), null);
                }
            }
            else
            {
                this.SearchFieldGUI();
            }
            EditorGUILayout.EndHorizontal();
            this.HandleCommandEvents();
        }

        private void DrawDetailedMemoryPane(SplitterState splitter)
        {
            SplitterGUILayout.BeginHorizontalSplit(splitter, new GUILayoutOption[0]);
            this.m_MemoryListView.OnGUI();
            this.m_ReferenceListView.OnGUI();
            SplitterGUILayout.EndHorizontalSplit();
        }

        private static void DrawEmptyCPUOrRenderingDetailPane()
        {
            GUILayout.Box(string.Empty, ms_Styles.header, new GUILayoutOption[0]);
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical(new GUILayoutOption[0]);
            GUILayout.FlexibleSpace();
            GUILayout.Label("Select Line for per-object breakdown", EditorStyles.wordWrappedLabel, new GUILayoutOption[0]);
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private void DrawMainToolbar()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
            GUILayoutOption[] options = new GUILayoutOption[] { GUILayout.Width(120f) };
            Rect position = GUILayoutUtility.GetRect(ms_Styles.addArea, EditorStyles.toolbarDropDown, options);
            if (EditorGUI.ButtonMouseDown(position, ms_Styles.addArea, FocusType.Passive, EditorStyles.toolbarDropDown))
            {
                int length = this.m_Charts.Length;
                string[] strArray = new string[length];
                bool[] enabled = new bool[length];
                for (int i = 0; i < length; i++)
                {
                    strArray[i] = ((ProfilerArea) i).ToString();
                    enabled[i] = !this.m_Charts[i].active;
                }
                EditorUtility.DisplayCustomMenu(position, strArray, enabled, null, new EditorUtility.SelectMenuItemFunction(this.AddAreaClick), null);
            }
            GUILayout.FlexibleSpace();
            bool flag = GUILayout.Toggle(this.m_Recording, ms_Styles.profilerRecord, EditorStyles.toolbarButton, new GUILayoutOption[0]);
            if (flag != this.m_Recording)
            {
                Profiler.enabled = flag;
                this.m_Recording = flag;
                SessionState.SetBool("ProfilerEnabled", flag);
            }
            SetProfileDeepScripts(GUILayout.Toggle(ProfilerDriver.deepProfiling, ms_Styles.deepProfile, EditorStyles.toolbarButton, new GUILayoutOption[0]));
            ProfilerDriver.profileEditor = GUILayout.Toggle(ProfilerDriver.profileEditor, ms_Styles.profileEditor, EditorStyles.toolbarButton, new GUILayoutOption[0]);
            this.m_AttachProfilerUI.OnGUILayout(this);
            if (GUILayout.Button(ms_Styles.clearData, EditorStyles.toolbarButton, new GUILayoutOption[0]))
            {
                ProfilerDriver.ClearAllFrames();
                NetworkDetailStats.m_NetworkOperations.Clear();
            }
            GUILayout.Space(5f);
            GUILayout.FlexibleSpace();
            this.FrameNavigationControls();
            GUILayout.EndHorizontal();
        }

        private void DrawMemoryPane(SplitterState splitter)
        {
            this.DrawMemoryToolbar();
            if (this.m_ShowDetailedMemoryPane == ProfilerMemoryView.Simple)
            {
                this.DrawOverviewText(ProfilerArea.Memory);
            }
            else
            {
                this.DrawDetailedMemoryPane(splitter);
            }
        }

        private void DrawMemoryToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
            GUILayoutOption[] options = new GUILayoutOption[] { GUILayout.Width(70f) };
            this.m_ShowDetailedMemoryPane = (ProfilerMemoryView) EditorGUILayout.EnumPopup(this.m_ShowDetailedMemoryPane, EditorStyles.toolbarDropDown, options);
            GUILayout.Space(5f);
            if (this.m_ShowDetailedMemoryPane == ProfilerMemoryView.Detailed)
            {
                if (GUILayout.Button("Take Sample: " + this.m_AttachProfilerUI.GetConnectedProfiler(), EditorStyles.toolbarButton, new GUILayoutOption[0]))
                {
                    this.RefreshMemoryData();
                }
                this.m_GatherObjectReferences = GUILayout.Toggle(this.m_GatherObjectReferences, ms_Styles.gatherObjectReferences, EditorStyles.toolbarButton, new GUILayoutOption[0]);
                if (this.m_AttachProfilerUI.IsEditor())
                {
                    GUILayout.Label("Memory usage in editor is not as it would be in a player", EditorStyles.toolbarButton, new GUILayoutOption[0]);
                }
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawNetworkOperationsPane()
        {
            SplitterGUILayout.BeginHorizontalSplit(this.m_NetworkSplit, new GUILayoutOption[0]);
            GUILayout.Label(ProfilerDriver.GetOverviewText(this.m_CurrentArea, this.GetActiveVisibleFrameIndex()), EditorStyles.wordWrappedLabel, new GUILayoutOption[0]);
            this.m_PaneScroll[(int) this.m_CurrentArea] = GUILayout.BeginScrollView(this.m_PaneScroll[(int) this.m_CurrentArea], ms_Styles.background);
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
            EditorGUILayout.LabelField("Operation Detail", new GUILayoutOption[0]);
            EditorGUILayout.LabelField("Over 5 Ticks", new GUILayoutOption[0]);
            EditorGUILayout.LabelField("Over 10 Ticks", new GUILayoutOption[0]);
            EditorGUILayout.LabelField("Total", new GUILayoutOption[0]);
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel++;
            for (short i = 0; i < this.msgNames.Length; i = (short) (i + 1))
            {
                if (NetworkDetailStats.m_NetworkOperations.ContainsKey(i))
                {
                    this.msgFoldouts[i] = EditorGUILayout.Foldout(this.msgFoldouts[i], this.msgNames[i] + ":");
                    if (this.msgFoldouts[i])
                    {
                        EditorGUILayout.BeginVertical(new GUILayoutOption[0]);
                        NetworkDetailStats.NetworkOperationDetails details = NetworkDetailStats.m_NetworkOperations[i];
                        EditorGUI.indentLevel++;
                        foreach (string str in details.m_Entries.Keys)
                        {
                            int time = (int) Time.time;
                            NetworkDetailStats.NetworkOperationEntryDetails details2 = details.m_Entries[str];
                            if (details2.m_IncomingTotal > 0)
                            {
                                EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
                                EditorGUILayout.LabelField("IN:" + str, new GUILayoutOption[0]);
                                EditorGUILayout.LabelField(details2.m_IncomingSequence.GetFiveTick(time).ToString(), new GUILayoutOption[0]);
                                EditorGUILayout.LabelField(details2.m_IncomingSequence.GetTenTick(time).ToString(), new GUILayoutOption[0]);
                                EditorGUILayout.LabelField(details2.m_IncomingTotal.ToString(), new GUILayoutOption[0]);
                                EditorGUILayout.EndHorizontal();
                            }
                            if (details2.m_OutgoingTotal > 0)
                            {
                                EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
                                EditorGUILayout.LabelField("OUT:" + str, new GUILayoutOption[0]);
                                EditorGUILayout.LabelField(details2.m_OutgoingSequence.GetFiveTick(time).ToString(), new GUILayoutOption[0]);
                                EditorGUILayout.LabelField(details2.m_OutgoingSequence.GetTenTick(time).ToString(), new GUILayoutOption[0]);
                                EditorGUILayout.LabelField(details2.m_OutgoingTotal.ToString(), new GUILayoutOption[0]);
                                EditorGUILayout.EndHorizontal();
                            }
                        }
                        EditorGUI.indentLevel--;
                        EditorGUILayout.EndVertical();
                    }
                }
            }
            EditorGUI.indentLevel--;
            GUILayout.EndScrollView();
            SplitterGUILayout.EndHorizontalSplit();
        }

        private static void DrawOtherToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawOverviewText(ProfilerArea area)
        {
            this.m_PaneScroll[(int) area] = GUILayout.BeginScrollView(this.m_PaneScroll[(int) area], ms_Styles.background);
            GUILayout.Label(ProfilerDriver.GetOverviewText(area, this.GetActiveVisibleFrameIndex()), EditorStyles.wordWrappedLabel, new GUILayoutOption[0]);
            GUILayout.EndScrollView();
        }

        private void DrawPane(ProfilerArea area)
        {
            DrawOtherToolbar();
            this.DrawOverviewText(area);
        }

        private void FrameNavigationControls()
        {
            if (this.m_CurrentFrame > ProfilerDriver.lastFrameIndex)
            {
                this.SetCurrentFrameDontPause(ProfilerDriver.lastFrameIndex);
            }
            GUILayout.Label(ms_Styles.frame, EditorStyles.miniLabel, new GUILayoutOption[0]);
            GUILayoutOption[] options = new GUILayoutOption[] { GUILayout.Width(100f) };
            GUILayout.Label("   " + this.PickFrameLabel(), EditorStyles.miniLabel, options);
            GUI.enabled = ProfilerDriver.GetPreviousFrameIndex(this.m_CurrentFrame) != -1;
            if (GUILayout.Button(ms_Styles.prevFrame, EditorStyles.toolbarButton, new GUILayoutOption[0]))
            {
                this.PrevFrame();
            }
            GUI.enabled = ProfilerDriver.GetNextFrameIndex(this.m_CurrentFrame) != -1;
            if (GUILayout.Button(ms_Styles.nextFrame, EditorStyles.toolbarButton, new GUILayoutOption[0]))
            {
                this.NextFrame();
            }
            GUI.enabled = true;
            GUILayout.Space(10f);
            if (GUILayout.Button(ms_Styles.currentFrame, EditorStyles.toolbarButton, new GUILayoutOption[0]))
            {
                this.SetCurrentFrame(-1);
                this.m_LastFrameFromTick = ProfilerDriver.lastFrameIndex;
            }
        }

        private static Rect GenerateRect(ref int row, int indent)
        {
            Rect rect = new Rect((indent * 16f) + 8f, ((float) row) * 16f, 0f, 16f) {
                xMax = 350f
            };
            row++;
            return rect;
        }

        public int GetActiveVisibleFrameIndex()
        {
            return ((this.m_CurrentFrame != -1) ? this.m_CurrentFrame : this.m_LastFrameFromTick);
        }

        public string GetSearch()
        {
            return this.m_SearchString;
        }

        private void HandleCommandEvents()
        {
            Event current = Event.current;
            EventType type = current.type;
            switch (type)
            {
                case EventType.ExecuteCommand:
                case EventType.ValidateCommand:
                {
                    bool flag = type == EventType.ExecuteCommand;
                    if (Event.current.commandName == "Find")
                    {
                        if (flag)
                        {
                            this.m_FocusSearchField = true;
                        }
                        current.Use();
                    }
                    break;
                }
            }
        }

        private void Initialize()
        {
            int len = ProfilerDriver.maxHistoryLength - 1;
            this.m_Charts = new ProfilerChart[9];
            Color[] colors = ProfilerColors.colors;
            for (ProfilerArea area = ProfilerArea.CPU; area < ProfilerArea.AreaCount; area += 1)
            {
                float dataScale = 1f;
                Chart.ChartType line = Chart.ChartType.Line;
                string[] graphStatisticsPropertiesForArea = ProfilerDriver.GetGraphStatisticsPropertiesForArea(area);
                int length = graphStatisticsPropertiesForArea.Length;
                switch (area)
                {
                    case ProfilerArea.GPU:
                    case ProfilerArea.CPU:
                        line = Chart.ChartType.StackedFill;
                        dataScale = 0.001f;
                        break;
                }
                ProfilerChart chart = new ProfilerChart(area, line, dataScale, length);
                for (int i = 0; i < length; i++)
                {
                    chart.m_Series[i] = new ChartSeries(graphStatisticsPropertiesForArea[i], len, colors[i % colors.Length]);
                }
                this.m_Charts[(int) area] = chart;
            }
            if (this.m_ReferenceListView == null)
            {
                this.m_ReferenceListView = new MemoryTreeList(this, null);
            }
            if (this.m_MemoryListView == null)
            {
                this.m_MemoryListView = new MemoryTreeListClickable(this, this.m_ReferenceListView);
            }
            this.UpdateCharts();
            this.BuildColumns();
            foreach (ProfilerChart chart2 in this.m_Charts)
            {
                chart2.LoadAndBindSettings();
            }
        }

        public bool IsSearching()
        {
            return (!string.IsNullOrEmpty(this.m_SearchString) && (this.m_SearchString.Length > 0));
        }

        private void MemRecordModeClick(object userData, string[] options, int selected)
        {
            this.m_SelectedMemRecordMode = (ProfilerMemoryRecordMode) selected;
            ProfilerDriver.memoryRecordMode = this.m_SelectedMemRecordMode;
        }

        private void NextFrame()
        {
            int nextFrameIndex = ProfilerDriver.GetNextFrameIndex(this.m_CurrentFrame);
            if (nextFrameIndex != -1)
            {
                this.SetCurrentFrame(nextFrameIndex);
            }
        }

        private void OnDestroy()
        {
            if (Profiler.supported)
            {
                Profiler.enabled = false;
            }
        }

        private void OnDisable()
        {
            m_ProfilerWindows.Remove(this);
        }

        private void OnEnable()
        {
            base.titleContent = base.GetLocalizedTitleContent();
            m_ProfilerWindows.Add(this);
            this.Initialize();
        }

        private void OnFocus()
        {
            if (Profiler.supported)
            {
                Profiler.enabled = this.m_Recording;
            }
        }

        private void OnGUI()
        {
            this.CheckForPlatformModuleChange();
            if (ms_Styles == null)
            {
                ms_Styles = new Styles();
            }
            this.DrawMainToolbar();
            SplitterGUILayout.BeginVerticalSplit(this.m_VertSplit, new GUILayoutOption[0]);
            this.m_GraphPos = EditorGUILayout.BeginScrollView(this.m_GraphPos, ms_Styles.profilerGraphBackground, new GUILayoutOption[0]);
            if (this.m_PrevLastFrame != ProfilerDriver.lastFrameIndex)
            {
                this.UpdateCharts();
                this.m_PrevLastFrame = ProfilerDriver.lastFrameIndex;
            }
            int currentFrame = this.m_CurrentFrame;
            Chart.ChartAction[] actionArray = new Chart.ChartAction[this.m_Charts.Length];
            for (int i = 0; i < this.m_Charts.Length; i++)
            {
                ProfilerChart chart = this.m_Charts[i];
                if (chart.active)
                {
                    currentFrame = chart.DoChartGUI(currentFrame, this.m_CurrentArea, out actionArray[i]);
                }
            }
            bool flag = false;
            if (currentFrame != this.m_CurrentFrame)
            {
                this.SetCurrentFrame(currentFrame);
                flag = true;
            }
            for (int j = 0; j < this.m_Charts.Length; j++)
            {
                ProfilerChart chart2 = this.m_Charts[j];
                if (chart2.active)
                {
                    if (actionArray[j] == Chart.ChartAction.Closed)
                    {
                        if (this.m_CurrentArea == j)
                        {
                            this.m_CurrentArea = ProfilerArea.CPU;
                        }
                        chart2.active = false;
                    }
                    else if (actionArray[j] == Chart.ChartAction.Activated)
                    {
                        this.m_CurrentArea = (ProfilerArea) j;
                        if ((this.m_CurrentArea != ProfilerArea.CPU) && (this.m_CPUHierarchyGUI.selectedIndex != -1))
                        {
                            this.ClearSelectedPropertyPath();
                        }
                        flag = true;
                    }
                }
            }
            if (flag)
            {
                base.Repaint();
                GUIUtility.ExitGUI();
            }
            EditorGUILayout.EndScrollView();
            GUILayout.BeginVertical(new GUILayoutOption[0]);
            switch (this.m_CurrentArea)
            {
                case ProfilerArea.CPU:
                    this.DrawCPUOrRenderingPane(this.m_CPUHierarchyGUI, this.m_CPUDetailHierarchyGUI, this.m_CPUTimelineGUI);
                    break;

                case ProfilerArea.GPU:
                    this.DrawCPUOrRenderingPane(this.m_GPUHierarchyGUI, this.m_GPUDetailHierarchyGUI, null);
                    break;

                case ProfilerArea.Memory:
                    this.DrawMemoryPane(this.m_ViewSplit);
                    break;

                case ProfilerArea.Audio:
                    this.DrawAudioPane();
                    break;

                case ProfilerArea.NetworkMessages:
                    this.DrawPane(this.m_CurrentArea);
                    break;

                case ProfilerArea.NetworkOperations:
                    this.DrawNetworkOperationsPane();
                    break;

                default:
                    this.DrawPane(this.m_CurrentArea);
                    break;
            }
            GUILayout.EndVertical();
            SplitterGUILayout.EndVerticalSplit();
        }

        private void OnLostFocus()
        {
            if (GUIUtility.hotControl != 0)
            {
                for (int i = 0; i < this.m_Charts.Length; i++)
                {
                    ProfilerChart chart = this.m_Charts[i];
                    chart.m_Chart.OnLostFocus();
                }
            }
        }

        private string PickFrameLabel()
        {
            if (this.m_CurrentFrame == -1)
            {
                return "Current";
            }
            return ((this.m_CurrentFrame + 1) + " / " + (ProfilerDriver.lastFrameIndex + 1));
        }

        private void PrevFrame()
        {
            int previousFrameIndex = ProfilerDriver.GetPreviousFrameIndex(this.m_CurrentFrame);
            if (previousFrameIndex != -1)
            {
                this.SetCurrentFrame(previousFrameIndex);
            }
        }

        private static string[] ProfilerColumnNames(ProfilerColumn[] columns)
        {
            string[] names = Enum.GetNames(typeof(ProfilerColumn));
            string[] strArray2 = new string[columns.Length];
            for (int i = 0; i < columns.Length; i++)
            {
                switch (columns[i])
                {
                    case ProfilerColumn.FunctionName:
                        strArray2[i] = LocalizationDatabase.GetLocalizedString("Overview");
                        break;

                    case ProfilerColumn.TotalPercent:
                        strArray2[i] = LocalizationDatabase.GetLocalizedString("Total");
                        break;

                    case ProfilerColumn.SelfPercent:
                        strArray2[i] = LocalizationDatabase.GetLocalizedString("Self");
                        break;

                    case ProfilerColumn.Calls:
                        strArray2[i] = LocalizationDatabase.GetLocalizedString("Calls");
                        break;

                    case ProfilerColumn.GCMemory:
                        strArray2[i] = LocalizationDatabase.GetLocalizedString("GC Alloc");
                        break;

                    case ProfilerColumn.TotalTime:
                        strArray2[i] = LocalizationDatabase.GetLocalizedString("Time ms");
                        break;

                    case ProfilerColumn.SelfTime:
                        strArray2[i] = LocalizationDatabase.GetLocalizedString("Self ms");
                        break;

                    case ProfilerColumn.DrawCalls:
                        strArray2[i] = LocalizationDatabase.GetLocalizedString("DrawCalls");
                        break;

                    case ProfilerColumn.TotalGPUTime:
                        strArray2[i] = LocalizationDatabase.GetLocalizedString("GPU ms");
                        break;

                    case ProfilerColumn.SelfGPUTime:
                        strArray2[i] = LocalizationDatabase.GetLocalizedString("Self ms");
                        break;

                    case ProfilerColumn.TotalGPUPercent:
                        strArray2[i] = LocalizationDatabase.GetLocalizedString("Total");
                        break;

                    case ProfilerColumn.SelfGPUPercent:
                        strArray2[i] = LocalizationDatabase.GetLocalizedString("Self");
                        break;

                    case ProfilerColumn.WarningCount:
                        strArray2[i] = LocalizationDatabase.GetLocalizedString("|Warnings");
                        break;

                    case ProfilerColumn.ObjectName:
                        strArray2[i] = LocalizationDatabase.GetLocalizedString("Name");
                        break;

                    default:
                        strArray2[i] = "ProfilerColumn." + names[(int) columns[i]];
                        break;
                }
            }
            return strArray2;
        }

        private void RefreshMemoryData()
        {
            this.m_MemoryListView.RequiresRefresh = true;
            ProfilerDriver.RequestObjectMemoryInfo(this.m_GatherObjectReferences);
        }

        [RequiredByNativeCode]
        private static void RepaintAllProfilerWindows()
        {
            foreach (ProfilerWindow window in m_ProfilerWindows)
            {
                if (ProfilerDriver.lastFrameIndex != window.m_LastFrameFromTick)
                {
                    window.m_LastFrameFromTick = ProfilerDriver.lastFrameIndex;
                    window.RepaintImmediately();
                }
            }
        }

        internal void SearchFieldGUI()
        {
            Event current = Event.current;
            Rect position = GUILayoutUtility.GetRect(50f, 300f, 16f, 16f, EditorStyles.toolbarSearchField);
            GUI.SetNextControlName("ProfilerSearchField");
            if (this.m_FocusSearchField)
            {
                EditorGUI.FocusTextInControl("ProfilerSearchField");
                if (Event.current.type == EventType.Repaint)
                {
                    this.m_FocusSearchField = false;
                }
            }
            if (((current.type == EventType.KeyDown) && (current.keyCode == KeyCode.Escape)) && (GUI.GetNameOfFocusedControl() == "ProfilerSearchField"))
            {
                this.m_SearchString = "";
            }
            if (((current.type == EventType.KeyDown) && ((current.keyCode == KeyCode.DownArrow) || (current.keyCode == KeyCode.UpArrow))) && (GUI.GetNameOfFocusedControl() == "ProfilerSearchField"))
            {
                this.m_CPUHierarchyGUI.SelectFirstRow();
                this.m_CPUHierarchyGUI.SetKeyboardFocus();
                base.Repaint();
                current.Use();
            }
            bool flag = this.m_CPUHierarchyGUI.selectedIndex != -1;
            EditorGUI.BeginChangeCheck();
            int id = GUIUtility.GetControlID(s_HashControlID, FocusType.Keyboard, base.position);
            this.m_SearchString = EditorGUI.ToolbarSearchField(id, position, this.m_SearchString, false);
            if ((EditorGUI.EndChangeCheck() && (!this.IsSearching() && (GUIUtility.keyboardControl == 0))) && flag)
            {
                this.m_CPUHierarchyGUI.FrameSelection();
            }
        }

        private void SetCurrentFrame(int frame)
        {
            if ((((frame != -1) && Profiler.enabled) && (!ProfilerDriver.profileEditor && (this.m_CurrentFrame != frame))) && EditorApplication.isPlayingOrWillChangePlaymode)
            {
                EditorApplication.isPaused = true;
            }
            if (ProfilerInstrumentationPopup.InstrumentationEnabled)
            {
                ProfilerInstrumentationPopup.UpdateInstrumentableFunctions();
            }
            this.SetCurrentFrameDontPause(frame);
        }

        private void SetCurrentFrameDontPause(int frame)
        {
            this.m_CurrentFrame = frame;
        }

        private static void SetMemoryProfilerInfo(ObjectMemoryInfo[] memoryInfo, int[] referencedIndices)
        {
            foreach (ProfilerWindow window in m_ProfilerWindows)
            {
                if (window.wantsMemoryRefresh)
                {
                    window.m_MemoryListView.SetRoot(MemoryElementDataManager.GetTreeRoot(memoryInfo, referencedIndices));
                }
            }
        }

        private static void SetProfileDeepScripts(bool deep)
        {
            if (ProfilerDriver.deepProfiling != deep)
            {
                bool flag2 = true;
                if (EditorApplication.isPlaying)
                {
                    if (deep)
                    {
                        flag2 = EditorUtility.DisplayDialog("Enable deep script profiling", "Enabling deep profiling requires reloading scripts.", "Reload", "Cancel");
                    }
                    else
                    {
                        flag2 = EditorUtility.DisplayDialog("Disable deep script profiling", "Disabling deep profiling requires reloading all scripts", "Reload", "Cancel");
                    }
                }
                if (flag2)
                {
                    ProfilerDriver.deepProfiling = deep;
                    InternalEditorUtility.RequestScriptReload();
                }
            }
        }

        public void SetSearch(string searchString)
        {
            this.m_SearchString = !string.IsNullOrEmpty(searchString) ? searchString : string.Empty;
        }

        public void SetSelectedPropertyPath(string path)
        {
            if (ProfilerDriver.selectedPropertyPath != path)
            {
                ProfilerDriver.selectedPropertyPath = path;
                this.UpdateCharts();
            }
        }

        private static void ShowProfilerWindow()
        {
            EditorWindow.GetWindow<ProfilerWindow>(false);
        }

        private static void UpdateChartGrid(float timeMax, ChartData data)
        {
            if (timeMax < 1500f)
            {
                string[] labels = new string[] { "1ms (1000FPS)", "0.25ms (4000FPS)", "0.1ms (10000FPS)" };
                data.SetGrid(new float[] { 1000f, 250f, 100f }, labels);
            }
            else if (timeMax < 10000f)
            {
                string[] textArray2 = new string[] { "8ms (120FPS)", "4ms (250FPS)", "1ms (1000FPS)" };
                data.SetGrid(new float[] { 8333f, 4000f, 1000f }, textArray2);
            }
            else if (timeMax < 30000f)
            {
                string[] textArray3 = new string[] { "16ms (60FPS)", "10ms (100FPS)", "5ms (200FPS)" };
                data.SetGrid(new float[] { 16667f, 10000f, 5000f }, textArray3);
            }
            else if (timeMax < 100000f)
            {
                string[] textArray4 = new string[] { "66ms (15FPS)", "33ms (30FPS)", "16ms (60FPS)" };
                data.SetGrid(new float[] { 66667f, 33333f, 16667f }, textArray4);
            }
            else
            {
                string[] textArray5 = new string[] { "500ms (2FPS)", "200ms (5FPS)", "66ms (15FPS)" };
                data.SetGrid(new float[] { 500000f, 200000f, 66667f }, textArray5);
            }
        }

        private void UpdateCharts()
        {
            int num = ProfilerDriver.maxHistoryLength - 1;
            int b = ProfilerDriver.lastFrameIndex - num;
            int firstSelectableFrame = Mathf.Max(ProfilerDriver.firstFrameIndex, b);
            foreach (ProfilerChart chart in this.m_Charts)
            {
                float num5 = 1f;
                float[] scale = new float[chart.m_Series.Length];
                for (int i = 0; i < chart.m_Series.Length; i++)
                {
                    float num8;
                    float num9;
                    ProfilerDriver.GetStatisticsValues(ProfilerDriver.GetStatisticsIdentifier(chart.m_Series[i].identifierName), b, chart.m_DataScale, chart.m_Series[i].data, out num8);
                    num8 = Mathf.Max(num8, 0.0001f);
                    if (num8 > num5)
                    {
                        num5 = num8;
                    }
                    if (chart.m_Type == Chart.ChartType.Line)
                    {
                        num9 = 1f / (num8 * (1.05f + (i * 0.05f)));
                    }
                    else
                    {
                        num9 = 1f / num8;
                    }
                    scale[i] = num9;
                }
                if (chart.m_Type == Chart.ChartType.Line)
                {
                    chart.m_Data.AssignScale(scale);
                }
                if ((chart.m_Area == ProfilerArea.NetworkMessages) || (chart.m_Area == ProfilerArea.NetworkOperations))
                {
                    for (int j = 0; j < chart.m_Series.Length; j++)
                    {
                        scale[j] = 0.9f / num5;
                    }
                    chart.m_Data.AssignScale(scale);
                    chart.m_Data.maxValue = num5;
                }
                chart.m_Data.Assign(chart.m_Series, b, firstSelectableFrame);
            }
            bool flag = (ProfilerDriver.selectedPropertyPath != string.Empty) && (this.m_CurrentArea == ProfilerArea.CPU);
            ProfilerChart chart2 = this.m_Charts[0];
            if (flag)
            {
                chart2.m_Data.hasOverlay = true;
                foreach (ChartSeries series in chart2.m_Series)
                {
                    float num13;
                    int statisticsIdentifier = ProfilerDriver.GetStatisticsIdentifier("Selected" + series.identifierName);
                    series.CreateOverlayData();
                    ProfilerDriver.GetStatisticsValues(statisticsIdentifier, b, chart2.m_DataScale, series.overlayData, out num13);
                }
            }
            else
            {
                chart2.m_Data.hasOverlay = false;
            }
            for (ProfilerArea area = ProfilerArea.CPU; area <= ProfilerArea.GPU; area += 1)
            {
                ProfilerChart chart3 = this.m_Charts[(int) area];
                float num14 = 0f;
                float num15 = 0f;
                for (int k = 0; k < num; k++)
                {
                    float num17 = 0f;
                    for (int m = 0; m < chart3.m_Series.Length; m++)
                    {
                        if (chart3.m_Series[m].enabled)
                        {
                            num17 += chart3.m_Series[m].data[k];
                        }
                    }
                    if (num17 > num14)
                    {
                        num14 = num17;
                    }
                    if ((num17 > num15) && ((k + b) >= (firstSelectableFrame + 1)))
                    {
                        num15 = num17;
                    }
                }
                if (num15 != 0f)
                {
                    num14 = num15;
                }
                num14 = Math.Min(num14, this.m_ChartMaxClamp);
                if (this.m_ChartOldMax[(int) area] > 0f)
                {
                    num14 = Mathf.Lerp(this.m_ChartOldMax[(int) area], num14, 0.4f);
                }
                this.m_ChartOldMax[(int) area] = num14;
                float[] singleArray1 = new float[] { 1f / num14 };
                chart3.m_Data.AssignScale(singleArray1);
                UpdateChartGrid(num14, chart3.m_Data);
            }
            string str2 = null;
            if (!ProfilerDriver.isGPUProfilerSupported)
            {
                str2 = "GPU profiling is not supported by the graphics card driver. Please update to a newer version if available.";
                if (Application.platform == RuntimePlatform.OSXEditor)
                {
                    if (!ProfilerDriver.isGPUProfilerSupportedByOS)
                    {
                        str2 = "GPU profiling requires Mac OS X 10.7 (Lion) and a capable video card. GPU profiling is currently not supported on mobile.";
                    }
                    else
                    {
                        str2 = "GPU profiling is not supported by the graphics card driver (or it was disabled because of driver bugs).";
                    }
                }
            }
            this.m_Charts[1].m_Chart.m_NotSupportedWarning = str2;
        }

        private bool wantsMemoryRefresh
        {
            get
            {
                return this.m_MemoryListView.RequiresRefresh;
            }
        }

        internal class Styles
        {
            public GUIContent addArea = EditorGUIUtility.TextContent("Add Profiler|Add a profiler area");
            public GUIStyle background = "OL Box";
            public GUIContent clearData = EditorGUIUtility.TextContent("Clear");
            public GUIContent currentFrame = EditorGUIUtility.TextContent("Current|Go to current frame");
            public GUIContent deepProfile = EditorGUIUtility.TextContent("Deep Profile|Instrument all mono calls to investigate scripts");
            public GUIStyle entryEven = "OL EntryBackEven";
            public GUIStyle entryOdd = "OL EntryBackOdd";
            public GUIContent frame = EditorGUIUtility.TextContent("Frame: ");
            public GUIContent frameDebugger = EditorGUIUtility.TextContent("Frame Debugger|Open Frame Debugger");
            public GUIContent gatherObjectReferences = EditorGUIUtility.TextContent("Gather object references|Collect reference information to see where objects are referenced from. Disable this to save memory");
            public GUIStyle header = "OL title";
            public GUIStyle label = "OL label";
            public GUIContent memRecord = EditorGUIUtility.TextContent("Mem Record|Record activity in the native memory system");
            public GUIContent nextFrame = EditorGUIUtility.IconContent("Profiler.NextFrame", "|Go one frame forwards");
            public GUIContent noData = EditorGUIUtility.TextContent("No frame data available");
            public GUIContent noFrameDebugger = EditorGUIUtility.TextContent("Frame Debugger|Open Frame Debugger (Current frame needs to be selected)");
            public GUIContent prevFrame = EditorGUIUtility.IconContent("Profiler.PrevFrame", "|Go back one frame");
            public GUIContent profileEditor = EditorGUIUtility.TextContent("Profile Editor|Enable profiling of the editor");
            public GUIStyle profilerGraphBackground = "ProfilerScrollviewBackground";
            public GUIContent profilerInstrumentation = EditorGUIUtility.TextContent("Instrumentation|Add Profiler Instrumentation to selected functions");
            public GUIContent profilerRecord = EditorGUIUtility.TextContentWithIcon("Record|Record profiling information", "Profiler.Record");
            public GUIContent[] reasons = GetLocalizedReasons();
            public GUIContent timelineHighDetail = EditorGUIUtility.TextContent("High Detail|Guaranteed to show all samples and memory callstacks");

            public Styles()
            {
                this.profilerGraphBackground.overflow.left = -170;
            }

            internal static GUIContent[] GetLocalizedReasons()
            {
                GUIContent[] contentArray = new GUIContent[11];
                contentArray[0] = EditorGUIUtility.TextContent("Scene object (Unloaded by loading a new scene or destroying it)");
                contentArray[1] = EditorGUIUtility.TextContent("Builtin Resource (Never unloaded)");
                contentArray[2] = EditorGUIUtility.TextContent("Object is marked Don't Save. (Must be explicitly destroyed or it will leak)");
                contentArray[3] = EditorGUIUtility.TextContent("Asset is dirty and must be saved first (Editor only)");
                contentArray[5] = EditorGUIUtility.TextContent("Asset type created from code or stored in the scene, referenced from native code.");
                contentArray[6] = EditorGUIUtility.TextContent("Asset type created from code or stored in the scene, referenced from scripts and native code.");
                contentArray[8] = EditorGUIUtility.TextContent("Asset referenced from native code.");
                contentArray[9] = EditorGUIUtility.TextContent("Asset referenced from scripts and native code.");
                contentArray[10] = EditorGUIUtility.TextContent("Not Applicable");
                return contentArray;
            }
        }
    }
}

