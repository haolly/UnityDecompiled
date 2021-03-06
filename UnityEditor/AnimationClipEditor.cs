﻿namespace UnityEditor
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEditor.Animations;
    using UnityEditorInternal;
    using UnityEngine;

    [CustomEditor(typeof(AnimationClip))]
    internal class AnimationClipEditor : Editor
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <takeIndex>k__BackingField;
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        private string[] <takeNames>k__BackingField;
        private const int kHeight = 2;
        private const int kPose = 0;
        private const int kPosition = 3;
        private const int kRotation = 1;
        private const int kSamplesPerSecond = 60;
        private float m_AdditivePoseFrame = 0f;
        private AvatarPreview m_AvatarPreview = null;
        private AnimationClip m_Clip = null;
        private AnimationClipInfoProperties m_ClipInfo = null;
        private UnityEditor.Animations.AnimatorController m_Controller = null;
        private bool m_DirtyQualityCurves = false;
        private float m_DraggingAdditivePoseFrame = 0f;
        private bool m_DraggingRange = false;
        private bool m_DraggingRangeBegin = false;
        private bool m_DraggingRangeEnd = false;
        private float m_DraggingStartFrame = 0f;
        private float m_DraggingStopFrame = 0f;
        private EventManipulationHandler m_EventManipulationHandler;
        private TimeArea m_EventTimeArea;
        private float m_InitialClipLength = 0f;
        private bool m_LoopBlend = false;
        private bool m_LoopBlendOrientation = false;
        private bool m_LoopBlendPositionXZ = false;
        private bool m_LoopBlendPositionY = false;
        private bool m_LoopTime = false;
        private AvatarMask m_Mask = null;
        private bool m_NeedsToGenerateClipInfo = false;
        private Vector2[][][] m_QualityCurves = new Vector2[4][][];
        private static bool m_ShowCurves = false;
        private static bool m_ShowEvents = false;
        private float m_StartFrame = 0f;
        private AnimatorState m_State;
        private AnimatorStateMachine m_StateMachine;
        private float m_StopFrame = 1f;
        private TimeArea m_TimeArea;
        public static float s_EventTimelineMax = 1.05f;
        private static int s_LoopMeterHint = s_LoopMeterStr.GetHashCode();
        private static string s_LoopMeterStr = "LoopMeter";
        private static int s_LoopOrientationMeterHint = s_LoopOrientationMeterStr.GetHashCode();
        private static string s_LoopOrientationMeterStr = "LoopOrientationMeter";
        private static int s_LoopPositionXZMeterHint = s_LoopPositionXZMeterStr.GetHashCode();
        private static string s_LoopPositionXZMeterStr = "LoopPostionXZMeter";
        private static int s_LoopPositionYMeterHint = s_LoopPositionYMeterStr.GetHashCode();
        private static string s_LoopPositionYMeterStr = "LoopPostionYMeter";

        private void AnimationClipGUI()
        {
            if (this.m_ClipInfo != null)
            {
                float firstFrame = this.m_ClipInfo.firstFrame;
                float lastFrame = this.m_ClipInfo.lastFrame;
                float additivePoseframe = 0f;
                bool changedStart = false;
                bool changedStop = false;
                bool changedAdditivePoseframe = false;
                this.ClipRangeGUI(ref firstFrame, ref lastFrame, out changedStart, out changedStop, false, ref additivePoseframe, out changedAdditivePoseframe);
                if (changedStart)
                {
                    this.m_ClipInfo.firstFrame = firstFrame;
                }
                if (changedStop)
                {
                    this.m_ClipInfo.lastFrame = lastFrame;
                }
                this.m_AvatarPreview.timeControl.startTime = firstFrame / this.m_Clip.frameRate;
                this.m_AvatarPreview.timeControl.stopTime = lastFrame / this.m_Clip.frameRate;
            }
            else
            {
                this.m_AvatarPreview.timeControl.startTime = 0f;
                this.m_AvatarPreview.timeControl.stopTime = this.m_Clip.length;
            }
            EditorGUIUtility.labelWidth = 0f;
            EditorGUIUtility.fieldWidth = 0f;
            if (this.m_ClipInfo != null)
            {
                this.m_ClipInfo.loop = EditorGUILayout.Toggle("Add Loop Frame", this.m_ClipInfo.loop, new GUILayoutOption[0]);
            }
            EditorGUI.BeginChangeCheck();
            int num4 = (this.m_ClipInfo == null) ? ((int) this.m_Clip.wrapMode) : this.m_ClipInfo.wrapMode;
            num4 = (int) ((WrapModeFixed) EditorGUILayout.EnumPopup("Wrap Mode", (WrapModeFixed) num4, new GUILayoutOption[0]));
            if (EditorGUI.EndChangeCheck())
            {
                if (this.m_ClipInfo != null)
                {
                    this.m_ClipInfo.wrapMode = num4;
                }
                else
                {
                    this.m_Clip.wrapMode = (WrapMode) num4;
                }
            }
        }

        private void CalculateQualityCurves()
        {
            for (int i = 0; i < 4; i++)
            {
                this.m_QualityCurves[i] = new Vector2[2][];
            }
            for (int j = 0; j < 2; j++)
            {
                float num3 = Mathf.Clamp(this.m_ClipInfo.firstFrame / this.m_Clip.frameRate, this.m_Clip.startTime, this.m_Clip.stopTime);
                float num4 = Mathf.Clamp(this.m_ClipInfo.lastFrame / this.m_Clip.frameRate, this.m_Clip.startTime, this.m_Clip.stopTime);
                float num5 = (j != 0) ? num3 : num4;
                float num6 = (j != 0) ? num3 : 0f;
                float num7 = (j != 0) ? this.m_Clip.stopTime : num4;
                int num8 = Mathf.FloorToInt(num6 * 60f);
                int num9 = Mathf.CeilToInt(num7 * 60f);
                this.m_QualityCurves[0][j] = new Vector2[(num9 - num8) + 1];
                this.m_QualityCurves[1][j] = new Vector2[(num9 - num8) + 1];
                this.m_QualityCurves[2][j] = new Vector2[(num9 - num8) + 1];
                this.m_QualityCurves[3][j] = new Vector2[(num9 - num8) + 1];
                QualityCurvesTime time = new QualityCurvesTime {
                    fixedTime = num5,
                    variableEndStart = num6,
                    variableEndEnd = num7,
                    q = j
                };
                MuscleClipEditorUtilities.CalculateQualityCurves(this.m_Clip, time, this.m_QualityCurves[0][j], this.m_QualityCurves[1][j], this.m_QualityCurves[2][j], this.m_QualityCurves[3][j]);
            }
            this.m_DirtyQualityCurves = false;
        }

        public void ClipRangeGUI(ref float startFrame, ref float stopFrame, out bool changedStart, out bool changedStop, bool showAdditivePoseFrame, ref float additivePoseframe, out bool changedAdditivePoseframe)
        {
            changedStart = false;
            changedStop = false;
            changedAdditivePoseframe = false;
            this.m_DraggingRangeBegin = false;
            this.m_DraggingRangeEnd = false;
            bool disabled = ((((startFrame + 0.01f) < (this.m_Clip.startTime * this.m_Clip.frameRate)) || ((startFrame - 0.01f) > (this.m_Clip.stopTime * this.m_Clip.frameRate))) || ((stopFrame + 0.01f) < (this.m_Clip.startTime * this.m_Clip.frameRate))) || ((stopFrame - 0.01f) > (this.m_Clip.stopTime * this.m_Clip.frameRate));
            bool flag2 = false;
            if (disabled)
            {
                GUILayout.BeginHorizontal(EditorStyles.helpBox, new GUILayoutOption[0]);
                GUILayout.Label("The clip range is outside of the range of the source take.", EditorStyles.wordWrappedMiniLabel, new GUILayoutOption[0]);
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical(new GUILayoutOption[0]);
                GUILayout.Space(5f);
                if (GUILayout.Button("Clamp Range", new GUILayoutOption[0]))
                {
                    flag2 = true;
                }
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
            Rect position = GUILayoutUtility.GetRect((float) 10f, (float) 33f);
            GUI.Label(position, "", "TE Toolbar");
            if (Event.current.type == EventType.Repaint)
            {
                this.m_TimeArea.rect = position;
            }
            this.m_TimeArea.BeginViewGUI();
            this.m_TimeArea.EndViewGUI();
            position.height -= 15f;
            int controlID = GUIUtility.GetControlID(0x2fb605, FocusType.Passive);
            int id = GUIUtility.GetControlID(0x2fb605, FocusType.Passive);
            int num3 = GUIUtility.GetControlID(0x2fb605, FocusType.Passive);
            GUI.BeginGroup(new Rect(position.x + 1f, position.y + 1f, position.width - 2f, position.height - 2f));
            float num4 = -1f;
            position.y = num4;
            position.x = num4;
            float x = this.m_TimeArea.FrameToPixel(startFrame, this.m_Clip.frameRate, position);
            float num6 = this.m_TimeArea.FrameToPixel(stopFrame, this.m_Clip.frameRate, position);
            GUI.Label(new Rect(x, position.y, num6 - x, position.height), "", EditorStyles.selectionRect);
            this.m_TimeArea.TimeRuler(position, this.m_Clip.frameRate);
            float num7 = this.m_TimeArea.TimeToPixel(this.m_AvatarPreview.timeControl.currentTime, position) - 0.5f;
            Handles.color = new Color(1f, 0f, 0f, 0.5f);
            Handles.DrawLine((Vector3) new Vector2(num7, position.yMin), (Vector3) new Vector2(num7, position.yMax));
            Handles.DrawLine((Vector3) new Vector2(num7 + 1f, position.yMin), (Vector3) new Vector2(num7 + 1f, position.yMax));
            Handles.color = Color.white;
            using (new EditorGUI.DisabledScope(disabled))
            {
                float time = startFrame / this.m_Clip.frameRate;
                TimeArea.TimeRulerDragMode mode = this.m_TimeArea.BrowseRuler(position, controlID, ref time, 0f, false, "TL InPoint");
                if (mode == TimeArea.TimeRulerDragMode.Cancel)
                {
                    startFrame = this.m_DraggingStartFrame;
                }
                else if (mode != TimeArea.TimeRulerDragMode.None)
                {
                    startFrame = time * this.m_Clip.frameRate;
                    startFrame = MathUtils.RoundBasedOnMinimumDifference(startFrame, (this.m_TimeArea.PixelDeltaToTime(position) * this.m_Clip.frameRate) * 10f);
                    changedStart = true;
                }
                float num9 = stopFrame / this.m_Clip.frameRate;
                TimeArea.TimeRulerDragMode mode2 = this.m_TimeArea.BrowseRuler(position, id, ref num9, 0f, false, "TL OutPoint");
                if (mode2 == TimeArea.TimeRulerDragMode.Cancel)
                {
                    stopFrame = this.m_DraggingStopFrame;
                }
                else if (mode2 != TimeArea.TimeRulerDragMode.None)
                {
                    stopFrame = num9 * this.m_Clip.frameRate;
                    stopFrame = MathUtils.RoundBasedOnMinimumDifference(stopFrame, (this.m_TimeArea.PixelDeltaToTime(position) * this.m_Clip.frameRate) * 10f);
                    changedStop = true;
                }
                if (showAdditivePoseFrame)
                {
                    float num10 = additivePoseframe / this.m_Clip.frameRate;
                    TimeArea.TimeRulerDragMode mode3 = this.m_TimeArea.BrowseRuler(position, num3, ref num10, 0f, false, "TL playhead");
                    if (mode3 == TimeArea.TimeRulerDragMode.Cancel)
                    {
                        additivePoseframe = this.m_DraggingAdditivePoseFrame;
                    }
                    else if (mode3 != TimeArea.TimeRulerDragMode.None)
                    {
                        additivePoseframe = num10 * this.m_Clip.frameRate;
                        additivePoseframe = MathUtils.RoundBasedOnMinimumDifference(additivePoseframe, (this.m_TimeArea.PixelDeltaToTime(position) * this.m_Clip.frameRate) * 10f);
                        changedAdditivePoseframe = true;
                    }
                }
            }
            if (GUIUtility.hotControl == controlID)
            {
                changedStart = true;
            }
            if (GUIUtility.hotControl == id)
            {
                changedStop = true;
            }
            if (GUIUtility.hotControl == num3)
            {
                changedAdditivePoseframe = true;
            }
            GUI.EndGroup();
            using (new EditorGUI.DisabledScope(disabled))
            {
                EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
                EditorGUI.BeginChangeCheck();
                startFrame = EditorGUILayout.FloatField(Styles.StartFrame, Mathf.Round(startFrame * 1000f) / 1000f, new GUILayoutOption[0]);
                if (EditorGUI.EndChangeCheck())
                {
                    changedStart = true;
                }
                GUILayout.FlexibleSpace();
                EditorGUI.BeginChangeCheck();
                stopFrame = EditorGUILayout.FloatField(Styles.EndFrame, Mathf.Round(stopFrame * 1000f) / 1000f, new GUILayoutOption[0]);
                if (EditorGUI.EndChangeCheck())
                {
                    changedStop = true;
                }
                EditorGUILayout.EndHorizontal();
            }
            changedStart |= flag2;
            changedStop |= flag2;
            if (changedStart)
            {
                startFrame = Mathf.Clamp(startFrame, this.m_Clip.startTime * this.m_Clip.frameRate, Mathf.Clamp(stopFrame, this.m_Clip.startTime * this.m_Clip.frameRate, stopFrame));
            }
            if (changedStop)
            {
                stopFrame = Mathf.Clamp(stopFrame, startFrame, this.m_Clip.stopTime * this.m_Clip.frameRate);
            }
            if (changedAdditivePoseframe)
            {
                additivePoseframe = Mathf.Clamp(additivePoseframe, this.m_Clip.startTime * this.m_Clip.frameRate, this.m_Clip.stopTime * this.m_Clip.frameRate);
            }
            if ((changedStart || changedStop) || changedAdditivePoseframe)
            {
                if (!this.m_DraggingRange)
                {
                    this.m_DraggingRangeBegin = true;
                }
                this.m_DraggingRange = true;
            }
            else if ((this.m_DraggingRange && (GUIUtility.hotControl == 0)) && (Event.current.type == EventType.Repaint))
            {
                this.m_DraggingRangeEnd = true;
                this.m_DraggingRange = false;
                this.m_DirtyQualityCurves = true;
                base.Repaint();
            }
            GUILayout.Space(10f);
        }

        private void CurveGUI()
        {
            if (this.m_ClipInfo != null)
            {
                if (this.m_AvatarPreview.timeControl.currentTime == float.NegativeInfinity)
                {
                    this.m_AvatarPreview.timeControl.Update();
                }
                float normalizedTime = this.m_AvatarPreview.timeControl.normalizedTime;
                for (int i = 0; i < this.m_ClipInfo.GetCurveCount(); i++)
                {
                    GUILayout.Space(5f);
                    GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                    GUILayoutOption[] optionArray1 = new GUILayoutOption[] { GUILayout.Width(17f) };
                    if (GUILayout.Button(GUIContent.none, "OL Minus", optionArray1))
                    {
                        this.m_ClipInfo.RemoveCurve(i);
                    }
                    else
                    {
                        float num6;
                        float num7;
                        GUILayoutOption[] optionArray2 = new GUILayoutOption[] { GUILayout.Width(125f) };
                        GUILayout.BeginVertical(optionArray2);
                        string curveName = this.m_ClipInfo.GetCurveName(i);
                        string name = EditorGUILayout.DelayedTextField(curveName, EditorStyles.textField, new GUILayoutOption[0]);
                        if (curveName != name)
                        {
                            this.m_ClipInfo.SetCurveName(i, name);
                        }
                        SerializedProperty curveProperty = this.m_ClipInfo.GetCurveProperty(i);
                        AnimationCurve animationCurveValue = curveProperty.animationCurveValue;
                        int length = animationCurveValue.length;
                        bool disabled = false;
                        int index = length - 1;
                        for (int j = 0; j < length; j++)
                        {
                            if (Mathf.Abs((float) (animationCurveValue.keys[j].time - normalizedTime)) < 0.0001f)
                            {
                                disabled = true;
                                index = j;
                                break;
                            }
                            if (animationCurveValue.keys[j].time > normalizedTime)
                            {
                                index = j;
                                break;
                            }
                        }
                        GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                        if (GUILayout.Button(Styles.PrevKeyContent, new GUILayoutOption[0]) && (index > 0))
                        {
                            index--;
                            this.m_AvatarPreview.timeControl.normalizedTime = animationCurveValue.keys[index].time;
                        }
                        if (GUILayout.Button(Styles.NextKeyContent, new GUILayoutOption[0]))
                        {
                            if (disabled && (index < (length - 1)))
                            {
                                index++;
                            }
                            this.m_AvatarPreview.timeControl.normalizedTime = animationCurveValue.keys[index].time;
                        }
                        using (new EditorGUI.DisabledScope(!disabled))
                        {
                            string kFloatFieldFormatString = EditorGUI.kFloatFieldFormatString;
                            EditorGUI.kFloatFieldFormatString = "n3";
                            num6 = animationCurveValue.Evaluate(normalizedTime);
                            GUILayoutOption[] optionArray3 = new GUILayoutOption[] { GUILayout.Width(60f) };
                            num7 = EditorGUILayout.FloatField(num6, optionArray3);
                            EditorGUI.kFloatFieldFormatString = kFloatFieldFormatString;
                        }
                        bool flag2 = false;
                        if (num6 != num7)
                        {
                            if (disabled)
                            {
                                animationCurveValue.RemoveKey(index);
                            }
                            flag2 = true;
                        }
                        using (new EditorGUI.DisabledScope(disabled))
                        {
                            if (GUILayout.Button(Styles.AddKeyframeContent, new GUILayoutOption[0]))
                            {
                                flag2 = true;
                            }
                        }
                        if (flag2)
                        {
                            Keyframe key = new Keyframe {
                                time = normalizedTime,
                                value = num7,
                                inTangent = 0f,
                                outTangent = 0f
                            };
                            animationCurveValue.AddKey(key);
                            this.m_ClipInfo.SetCurve(i, animationCurveValue);
                            AnimationCurvePreviewCache.ClearCache();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.EndVertical();
                        Rect ranges = new Rect();
                        GUILayoutOption[] optionArray4 = new GUILayoutOption[] { GUILayout.Height(40f) };
                        EditorGUILayout.CurveField(curveProperty, EditorGUI.kCurveColor, ranges, GUIContent.none, optionArray4);
                        Rect lastRect = GUILayoutUtility.GetLastRect();
                        length = animationCurveValue.length;
                        Handles.color = Color.red;
                        Handles.DrawLine(new Vector3(lastRect.x + (normalizedTime * lastRect.width), lastRect.y, 0f), new Vector3(lastRect.x + (normalizedTime * lastRect.width), lastRect.y + lastRect.height, 0f));
                        for (int k = 0; k < length; k++)
                        {
                            float time = animationCurveValue.keys[k].time;
                            Handles.color = Color.white;
                            Handles.DrawLine(new Vector3(lastRect.x + (time * lastRect.width), (lastRect.y + lastRect.height) - 10f, 0f), new Vector3(lastRect.x + (time * lastRect.width), lastRect.y + lastRect.height, 0f));
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                GUILayoutOption[] options = new GUILayoutOption[] { GUILayout.Width(17f) };
                if (GUILayout.Button(GUIContent.none, "OL Plus", options))
                {
                    this.m_ClipInfo.AddCurve();
                }
                GUILayout.EndHorizontal();
            }
        }

        private void DestroyController()
        {
            if ((this.m_AvatarPreview != null) && (this.m_AvatarPreview.Animator != null))
            {
                UnityEditor.Animations.AnimatorController.SetAnimatorController(this.m_AvatarPreview.Animator, null);
            }
            UnityEngine.Object.DestroyImmediate(this.m_Controller);
            UnityEngine.Object.DestroyImmediate(this.m_State);
            this.m_Controller = null;
            this.m_StateMachine = null;
            this.m_State = null;
        }

        internal static void EditWithImporter(AnimationClip clip)
        {
            ModelImporter atPath = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(clip)) as ModelImporter;
            if (atPath != null)
            {
                Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(atPath.assetPath);
                ModelImporterEditor editor = Editor.CreateEditor(atPath) as ModelImporterEditor;
                EditorPrefs.SetInt(editor.GetType().Name + "ActiveEditorIndex", 2);
                int num = 0;
                ModelImporterClipAnimation[] clipAnimations = atPath.clipAnimations;
                for (int i = 0; i < clipAnimations.Length; i++)
                {
                    if (clipAnimations[i].name == clip.name)
                    {
                        num = i;
                    }
                }
                EditorPrefs.SetInt("ModelImporterClipEditor.ActiveClipIndex", num);
            }
        }

        private void EventsGUI()
        {
            if (this.m_ClipInfo != null)
            {
                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                GUILayoutOption[] options = new GUILayoutOption[] { GUILayout.Width(25f) };
                if (GUILayout.Button(Styles.AddEventContent, options))
                {
                    this.m_ClipInfo.AddEvent(Mathf.Clamp01(this.m_AvatarPreview.timeControl.normalizedTime));
                    this.m_EventManipulationHandler.SelectEvent(this.m_ClipInfo.GetEvents(), this.m_ClipInfo.GetEventCount() - 1, this.m_ClipInfo);
                    this.needsToGenerateClipInfo = true;
                }
                Rect position = GUILayoutUtility.GetRect((float) 10f, (float) 33f);
                position.xMin += 5f;
                position.xMax -= 4f;
                GUI.Label(position, "", "TE Toolbar");
                if (Event.current.type == EventType.Repaint)
                {
                    this.m_EventTimeArea.rect = position;
                }
                position.height -= 15f;
                this.m_EventTimeArea.TimeRuler(position, 100f);
                GUI.BeginGroup(new Rect(position.x + 1f, position.y + 1f, position.width - 2f, position.height - 2f));
                Rect rect = new Rect(-1f, -1f, position.width, position.height);
                AnimationEvent[] events = this.m_ClipInfo.GetEvents();
                if (this.m_EventManipulationHandler.HandleEventManipulation(rect, ref events, this.m_ClipInfo))
                {
                    this.m_ClipInfo.SetEvents(events);
                }
                float x = this.m_EventTimeArea.TimeToPixel(this.m_AvatarPreview.timeControl.normalizedTime, rect) - 0.5f;
                Handles.color = new Color(1f, 0f, 0f, 0.5f);
                Handles.DrawLine((Vector3) new Vector2(x, rect.yMin), (Vector3) new Vector2(x, rect.yMax));
                Handles.DrawLine((Vector3) new Vector2(x + 1f, rect.yMin), (Vector3) new Vector2(x + 1f, rect.yMax));
                Handles.color = Color.white;
                GUI.EndGroup();
                GUILayout.EndHorizontal();
                this.m_EventManipulationHandler.Draw(position);
            }
        }

        private float GetClipLength() => 
            ((this.m_ClipInfo?.lastFrame - this.m_ClipInfo.firstFrame) / this.m_Clip.frameRate);

        private string GetStatsText()
        {
            string str = "";
            if ((base.targets.Length == 1) && (base.target as Motion).isHumanMotion)
            {
                str = ((str + "Average Velocity: ") + this.m_Clip.averageSpeed.ToString("0.000") + "\nAverage Angular Y Speed: ") + (((this.m_Clip.averageAngularSpeed * 180f) / 3.141593f)).ToString("0.0") + " deg/s";
            }
            if (this.m_ClipInfo != null)
            {
                return str;
            }
            AnimationClipStats stats = new AnimationClipStats();
            stats.Reset();
            for (int i = 0; i < base.targets.Length; i++)
            {
                AnimationClip clip = base.targets[i] as AnimationClip;
                if (clip != null)
                {
                    AnimationClipStats animationClipStats = AnimationUtility.GetAnimationClipStats(clip);
                    stats.Combine(animationClipStats);
                }
            }
            if (str.Length != 0)
            {
                str = str + '\n';
            }
            float num3 = (((float) stats.constantCurves) / ((float) stats.totalCurves)) * 100f;
            float num4 = (((float) stats.denseCurves) / ((float) stats.totalCurves)) * 100f;
            float num5 = (((float) stats.streamCurves) / ((float) stats.totalCurves)) * 100f;
            return ((str + $"Curves Pos: {stats.positionCurves} Quaternion: {stats.quaternionCurves} Euler: {stats.eulerCurves} Scale: {stats.scaleCurves} Muscles: {stats.muscleCurves} Generic: {stats.genericCurves} PPtr: {stats.pptrCurves}
") + $"Curves Total: {stats.totalCurves}, Constant: {stats.constantCurves} ({num3.ToString("0.0")}%) Dense: {stats.denseCurves} ({num4.ToString("0.0")}%) Stream: {stats.streamCurves} ({num5.ToString("0.0")}%)
" + EditorUtility.FormatBytes(stats.size));
        }

        public override bool HasPreviewGUI()
        {
            this.Init();
            return (this.m_AvatarPreview != null);
        }

        private void Init()
        {
            if (this.m_AvatarPreview == null)
            {
                this.m_AvatarPreview = new AvatarPreview(null, base.target as Motion);
                this.m_AvatarPreview.OnAvatarChangeFunc = new AvatarPreview.OnAvatarChange(this.SetPreviewAvatar);
                this.m_AvatarPreview.fps = Mathf.RoundToInt((base.target as AnimationClip).frameRate);
                this.m_AvatarPreview.ShowIKOnFeetButton = (base.target as Motion).isHumanMotion;
            }
        }

        private void InitController()
        {
            if (!this.m_Clip.legacy && ((this.m_AvatarPreview != null) && (this.m_AvatarPreview.Animator != null)))
            {
                if (this.m_Controller == null)
                {
                    this.m_Controller = new UnityEditor.Animations.AnimatorController();
                    this.m_Controller.pushUndo = false;
                    this.m_Controller.hideFlags = HideFlags.HideAndDontSave;
                    this.m_Controller.AddLayer("preview");
                    this.m_StateMachine = this.m_Controller.layers[0].stateMachine;
                    this.m_StateMachine.pushUndo = false;
                    this.m_StateMachine.hideFlags = HideFlags.HideAndDontSave;
                    if (this.mask != null)
                    {
                        UnityEditor.Animations.AnimatorControllerLayer[] layers = this.m_Controller.layers;
                        layers[0].avatarMask = this.mask;
                        this.m_Controller.layers = layers;
                    }
                }
                if (this.m_State == null)
                {
                    this.m_State = this.m_StateMachine.AddState("preview");
                    this.m_State.pushUndo = false;
                    UnityEditor.Animations.AnimatorControllerLayer[] layerArray2 = this.m_Controller.layers;
                    this.m_State.motion = this.m_Clip;
                    this.m_Controller.layers = layerArray2;
                    this.m_State.iKOnFeet = this.m_AvatarPreview.IKOnFeet;
                    this.m_State.hideFlags = HideFlags.HideAndDontSave;
                }
                UnityEditor.Animations.AnimatorController.SetAnimatorController(this.m_AvatarPreview.Animator, this.m_Controller);
                if (UnityEditor.Animations.AnimatorController.GetEffectiveAnimatorController(this.m_AvatarPreview.Animator) != this.m_Controller)
                {
                    UnityEditor.Animations.AnimatorController.SetAnimatorController(this.m_AvatarPreview.Animator, this.m_Controller);
                }
            }
        }

        private void LoopQualityLampAndCurve(Rect position, float value, int lightMeterHint, bool changedStart, bool changedStop, Vector2[][] curves)
        {
            if (this.m_ClipInfo != null)
            {
                GUIStyle style = new GUIStyle(EditorStyles.miniLabel) {
                    alignment = TextAnchor.MiddleRight
                };
                Rect rect = position;
                rect.xMax -= 20f;
                rect.xMin += EditorGUIUtility.labelWidth;
                GUI.Label(rect, "loop match", style);
                Event current = Event.current;
                int controlID = GUIUtility.GetControlID(lightMeterHint, FocusType.Passive, position);
                if (current.GetTypeForControl(controlID) == EventType.Repaint)
                {
                    Rect rect2 = position;
                    float num2 = (22f - rect2.height) / 2f;
                    rect2.y -= num2;
                    rect2.xMax += num2;
                    rect2.height = 22f;
                    rect2.xMin = rect2.xMax - 22f;
                    if (value < 0.33f)
                    {
                        GUI.DrawTexture(rect2, Styles.RedLightIcon.image);
                    }
                    else if (value < 0.66f)
                    {
                        GUI.DrawTexture(rect2, Styles.OrangeLightIcon.image);
                    }
                    else
                    {
                        GUI.DrawTexture(rect2, Styles.GreenLightIcon.image);
                    }
                    GUI.DrawTexture(rect2, Styles.LightRimIcon.image);
                }
                if (changedStart || changedStop)
                {
                    Rect rect3 = position;
                    rect3.y += rect3.height + 1f;
                    rect3.height = 18f;
                    GUI.color = new Color(0f, 0f, 0f, EditorGUIUtility.isProSkin ? 0.3f : 0.8f);
                    GUI.DrawTexture(rect3, EditorGUIUtility.whiteTexture);
                    rect3 = new RectOffset(-1, -1, -1, -1).Add(rect3);
                    if (!EditorGUIUtility.isProSkin)
                    {
                        GUI.color = new Color(0.3529412f, 0.3529412f, 0.3529412f, 1f);
                    }
                    else
                    {
                        GUI.color = new Color(0.254902f, 0.254902f, 0.254902f, 1f);
                    }
                    GUI.DrawTexture(rect3, EditorGUIUtility.whiteTexture);
                    GUI.color = Color.white;
                    GUI.BeginGroup(rect3);
                    Matrix4x4 drawingToViewMatrix = this.m_TimeArea.drawingToViewMatrix;
                    drawingToViewMatrix.m00 = rect3.width / this.m_TimeArea.shownArea.width;
                    drawingToViewMatrix.m11 = rect3.height - 1f;
                    drawingToViewMatrix.m03 = (-this.m_TimeArea.shownArea.x * rect3.width) / this.m_TimeArea.shownArea.width;
                    drawingToViewMatrix.m13 = 0f;
                    Vector2[] vectorArray = curves[!changedStart ? 1 : 0];
                    Vector3[] points = new Vector3[vectorArray.Length];
                    Color[] colors = new Color[vectorArray.Length];
                    Color color = new Color(1f, 0.3f, 0.3f);
                    Color color2 = new Color(1f, 0.8f, 0f);
                    Color color3 = new Color(0f, 1f, 0f);
                    for (int i = 0; i < points.Length; i++)
                    {
                        points[i] = (Vector3) vectorArray[i];
                        points[i] = drawingToViewMatrix.MultiplyPoint3x4(points[i]);
                        if ((1f - vectorArray[i].y) < 0.33f)
                        {
                            colors[i] = color;
                        }
                        else if ((1f - vectorArray[i].y) < 0.66f)
                        {
                            colors[i] = color2;
                        }
                        else
                        {
                            colors[i] = color3;
                        }
                    }
                    Handles.DrawAAPolyLine(colors, points);
                    GUI.color = new Color(0.3f, 0.6f, 1f);
                    GUI.DrawTexture(new Rect(drawingToViewMatrix.MultiplyPoint3x4(new Vector3((!changedStart ? this.m_StopFrame : this.m_StartFrame) / this.m_Clip.frameRate, 0f, 0f)).x, 0f, 1f, rect3.height), EditorGUIUtility.whiteTexture);
                    GUI.DrawTexture(new Rect(drawingToViewMatrix.MultiplyPoint3x4(new Vector3((!changedStart ? this.m_StartFrame : this.m_StopFrame) / this.m_Clip.frameRate, 0f, 0f)).x, 0f, 1f, rect3.height), EditorGUIUtility.whiteTexture);
                    GUI.color = Color.white;
                    GUI.EndGroup();
                }
            }
        }

        private void LoopToggle(Rect r, GUIContent content, ref bool val)
        {
            if (!this.m_DraggingRange)
            {
                val = EditorGUI.Toggle(r, content, val);
            }
            else
            {
                EditorGUI.LabelField(r, content, GUIContent.none);
                using (new EditorGUI.DisabledScope(true))
                {
                    EditorGUI.Toggle(r, " ", false);
                }
            }
        }

        private void MuscleClipGUI()
        {
            Rect rect2;
            bool changed;
            EditorGUI.BeginChangeCheck();
            this.InitController();
            AnimationClipSettings animationClipSettings = AnimationUtility.GetAnimationClipSettings(this.m_Clip);
            bool isHumanMotion = (base.target as Motion).isHumanMotion;
            bool flag2 = AnimationUtility.HasMotionCurves(this.m_Clip);
            bool flag3 = AnimationUtility.HasRootCurves(this.m_Clip);
            bool flag4 = AnimationUtility.HasGenericRootTransform(this.m_Clip);
            bool flag5 = AnimationUtility.HasMotionFloatCurves(this.m_Clip);
            bool flag6 = flag3 || flag2;
            this.m_StartFrame = !this.m_DraggingRange ? (animationClipSettings.startTime * this.m_Clip.frameRate) : this.m_StartFrame;
            this.m_StopFrame = !this.m_DraggingRange ? (animationClipSettings.stopTime * this.m_Clip.frameRate) : this.m_StopFrame;
            this.m_AdditivePoseFrame = !this.m_DraggingRange ? (animationClipSettings.additiveReferencePoseTime * this.m_Clip.frameRate) : this.m_AdditivePoseFrame;
            bool changedStart = false;
            bool changedStop = false;
            bool changedAdditivePoseframe = false;
            if (this.m_ClipInfo != null)
            {
                if (flag6)
                {
                    if (this.m_DirtyQualityCurves)
                    {
                        this.CalculateQualityCurves();
                    }
                    if ((this.m_QualityCurves[0] == null) && (Event.current.type == EventType.Repaint))
                    {
                        this.m_DirtyQualityCurves = true;
                        base.Repaint();
                    }
                }
                this.ClipRangeGUI(ref this.m_StartFrame, ref this.m_StopFrame, out changedStart, out changedStop, animationClipSettings.hasAdditiveReferencePose, ref this.m_AdditivePoseFrame, out changedAdditivePoseframe);
            }
            float startTime = this.m_StartFrame / this.m_Clip.frameRate;
            float stopTime = this.m_StopFrame / this.m_Clip.frameRate;
            float num3 = this.m_AdditivePoseFrame / this.m_Clip.frameRate;
            if (!this.m_DraggingRange)
            {
                animationClipSettings.startTime = startTime;
                animationClipSettings.stopTime = stopTime;
                animationClipSettings.additiveReferencePoseTime = num3;
            }
            this.m_AvatarPreview.timeControl.startTime = startTime;
            this.m_AvatarPreview.timeControl.stopTime = stopTime;
            if (changedStart)
            {
                this.m_AvatarPreview.timeControl.nextCurrentTime = startTime;
            }
            if (changedStop)
            {
                this.m_AvatarPreview.timeControl.nextCurrentTime = stopTime;
            }
            if (changedAdditivePoseframe)
            {
                this.m_AvatarPreview.timeControl.nextCurrentTime = num3;
            }
            EditorGUIUtility.labelWidth = 0f;
            EditorGUIUtility.fieldWidth = 0f;
            MuscleClipQualityInfo info = MuscleClipEditorUtilities.GetMuscleClipQualityInfo(this.m_Clip, startTime, stopTime);
            Rect controlRect = EditorGUILayout.GetControlRect(new GUILayoutOption[0]);
            this.LoopToggle(controlRect, Styles.LoopTime, ref animationClipSettings.loopTime);
            using (new EditorGUI.DisabledScope(!animationClipSettings.loopTime))
            {
                EditorGUI.indentLevel++;
                rect2 = EditorGUILayout.GetControlRect(new GUILayoutOption[0]);
                this.LoopToggle(rect2, Styles.LoopPose, ref animationClipSettings.loopBlend);
                animationClipSettings.cycleOffset = EditorGUILayout.FloatField(Styles.LoopCycleOffset, animationClipSettings.cycleOffset, new GUILayoutOption[0]);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Space();
            bool flag10 = isHumanMotion && (changedStart || changedStop);
            if (!flag3 || flag2)
            {
                goto Label_06CE;
            }
            GUILayout.Label("Root Transform Rotation", EditorStyles.label, new GUILayoutOption[0]);
            EditorGUI.indentLevel++;
            Rect r = EditorGUILayout.GetControlRect(new GUILayoutOption[0]);
            this.LoopToggle(r, Styles.BakeIntoPoseOrientation, ref animationClipSettings.loopBlendOrientation);
            int selectedIndex = !animationClipSettings.keepOriginalOrientation ? 1 : 0;
            selectedIndex = EditorGUILayout.Popup(!animationClipSettings.loopBlendOrientation ? Styles.BasedUponStartOrientation : Styles.BasedUponOrientation, selectedIndex, !isHumanMotion ? Styles.BasedUponRotationOpt : Styles.BasedUponRotationHumanOpt, new GUILayoutOption[0]);
            animationClipSettings.keepOriginalOrientation = selectedIndex == 0;
            if (flag10)
            {
                EditorGUILayout.GetControlRect(new GUILayoutOption[0]);
            }
            else
            {
                animationClipSettings.orientationOffsetY = EditorGUILayout.FloatField(Styles.OrientationOffsetY, animationClipSettings.orientationOffsetY, new GUILayoutOption[0]);
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
            GUILayout.Label("Root Transform Position (Y)", EditorStyles.label, new GUILayoutOption[0]);
            EditorGUI.indentLevel++;
            Rect rect4 = EditorGUILayout.GetControlRect(new GUILayoutOption[0]);
            this.LoopToggle(rect4, Styles.BakeIntoPosePositionY, ref animationClipSettings.loopBlendPositionY);
            if (isHumanMotion)
            {
                int num5;
                if (animationClipSettings.keepOriginalPositionY)
                {
                    num5 = 0;
                }
                else if (animationClipSettings.heightFromFeet)
                {
                    num5 = 2;
                }
                else
                {
                    num5 = 1;
                }
                switch (EditorGUILayout.Popup(!animationClipSettings.loopBlendPositionY ? Styles.BasedUponPositionY : Styles.BasedUponStartPositionY, num5, Styles.BasedUponPositionYHumanOpt, new GUILayoutOption[0]))
                {
                    case 0:
                        animationClipSettings.keepOriginalPositionY = true;
                        animationClipSettings.heightFromFeet = false;
                        goto Label_0548;

                    case 1:
                        animationClipSettings.keepOriginalPositionY = false;
                        animationClipSettings.heightFromFeet = false;
                        goto Label_0548;
                }
                animationClipSettings.keepOriginalPositionY = false;
                animationClipSettings.heightFromFeet = true;
            }
            else
            {
                int num6 = !animationClipSettings.keepOriginalPositionY ? 1 : 0;
                num6 = EditorGUILayout.Popup(!animationClipSettings.loopBlendPositionY ? Styles.BasedUponPositionY : Styles.BasedUponStartPositionY, num6, Styles.BasedUponPositionYOpt, new GUILayoutOption[0]);
                animationClipSettings.keepOriginalPositionY = num6 == 0;
            }
        Label_0548:
            if (flag10)
            {
                EditorGUILayout.GetControlRect(new GUILayoutOption[0]);
            }
            else
            {
                animationClipSettings.level = EditorGUILayout.FloatField(Styles.PositionOffsetY, animationClipSettings.level, new GUILayoutOption[0]);
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
            GUILayout.Label("Root Transform Position (XZ)", EditorStyles.label, new GUILayoutOption[0]);
            EditorGUI.indentLevel++;
            Rect rect5 = EditorGUILayout.GetControlRect(new GUILayoutOption[0]);
            this.LoopToggle(rect5, Styles.BakeIntoPosePositionXZ, ref animationClipSettings.loopBlendPositionXZ);
            int num7 = !animationClipSettings.keepOriginalPositionXZ ? 1 : 0;
            num7 = EditorGUILayout.Popup(!animationClipSettings.loopBlendPositionXZ ? Styles.BasedUponPositionXZ : Styles.BasedUponStartPositionXZ, num7, !isHumanMotion ? Styles.BasedUponPositionXZOpt : Styles.BasedUponPositionXZHumanOpt, new GUILayoutOption[0]);
            animationClipSettings.keepOriginalPositionXZ = num7 == 0;
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
            if (flag6)
            {
                if (isHumanMotion)
                {
                    this.LoopQualityLampAndCurve(rect2, info.loop, s_LoopMeterHint, changedStart, changedStop, this.m_QualityCurves[0]);
                }
                this.LoopQualityLampAndCurve(r, info.loopOrientation, s_LoopOrientationMeterHint, changedStart, changedStop, this.m_QualityCurves[1]);
                this.LoopQualityLampAndCurve(rect4, info.loopPositionY, s_LoopPositionYMeterHint, changedStart, changedStop, this.m_QualityCurves[2]);
                this.LoopQualityLampAndCurve(rect5, info.loopPositionXZ, s_LoopPositionXZMeterHint, changedStart, changedStop, this.m_QualityCurves[3]);
            }
        Label_06CE:
            if (isHumanMotion)
            {
                if (flag2)
                {
                    this.LoopQualityLampAndCurve(rect2, info.loop, s_LoopMeterHint, changedStart, changedStop, this.m_QualityCurves[0]);
                }
                animationClipSettings.mirror = EditorGUILayout.Toggle(Styles.Mirror, animationClipSettings.mirror, new GUILayoutOption[0]);
            }
            if (this.m_ClipInfo != null)
            {
                animationClipSettings.hasAdditiveReferencePose = EditorGUILayout.Toggle(Styles.HasAdditiveReferencePose, animationClipSettings.hasAdditiveReferencePose, new GUILayoutOption[0]);
                using (new EditorGUI.DisabledScope(!animationClipSettings.hasAdditiveReferencePose))
                {
                    EditorGUI.indentLevel++;
                    this.m_AdditivePoseFrame = EditorGUILayout.FloatField(Styles.AdditiveReferencePoseFrame, this.m_AdditivePoseFrame, new GUILayoutOption[0]);
                    this.m_AdditivePoseFrame = Mathf.Clamp(this.m_AdditivePoseFrame, this.m_Clip.startTime * this.m_Clip.frameRate, this.m_Clip.stopTime * this.m_Clip.frameRate);
                    animationClipSettings.additiveReferencePoseTime = this.m_AdditivePoseFrame / this.m_Clip.frameRate;
                    EditorGUI.indentLevel--;
                }
            }
            if (flag2)
            {
                EditorGUILayout.Space();
                GUILayout.Label(Styles.MotionCurves, EditorStyles.label, new GUILayoutOption[0]);
            }
            if (((this.m_ClipInfo == null) && flag4) && !flag5)
            {
                EditorGUILayout.Space();
                GUILayout.BeginHorizontal(new GUILayoutOption[0]);
                GUILayout.FlexibleSpace();
                if (flag2)
                {
                    if (GUILayout.Button("Remove Root Motion Curves", new GUILayoutOption[0]))
                    {
                        AnimationUtility.SetGenerateMotionCurves(this.m_Clip, false);
                    }
                }
                else if (GUILayout.Button("Generate Root Motion Curves", new GUILayoutOption[0]))
                {
                    AnimationUtility.SetGenerateMotionCurves(this.m_Clip, true);
                }
                GUILayout.EndHorizontal();
            }
            string statsText = this.GetStatsText();
            if (statsText != "")
            {
                GUILayout.Label(statsText, EditorStyles.helpBox, new GUILayoutOption[0]);
            }
            EditorGUILayout.Space();
            if (this.m_ClipInfo != null)
            {
                changed = GUI.changed;
                m_ShowCurves = EditorGUILayout.Foldout(m_ShowCurves, Styles.Curves, true);
                GUI.changed = changed;
                if (m_ShowCurves)
                {
                    this.CurveGUI();
                }
            }
            if (this.m_ClipInfo != null)
            {
                changed = GUI.changed;
                m_ShowEvents = EditorGUILayout.Foldout(m_ShowEvents, "Events", true);
                GUI.changed = changed;
                if (m_ShowEvents)
                {
                    this.EventsGUI();
                }
            }
            if (this.m_DraggingRangeBegin)
            {
                this.m_LoopTime = animationClipSettings.loopTime;
                this.m_LoopBlend = animationClipSettings.loopBlend;
                this.m_LoopBlendOrientation = animationClipSettings.loopBlendOrientation;
                this.m_LoopBlendPositionY = animationClipSettings.loopBlendPositionY;
                this.m_LoopBlendPositionXZ = animationClipSettings.loopBlendPositionXZ;
                animationClipSettings.loopTime = false;
                animationClipSettings.loopBlend = false;
                animationClipSettings.loopBlendOrientation = false;
                animationClipSettings.loopBlendPositionY = false;
                animationClipSettings.loopBlendPositionXZ = false;
                this.m_DraggingStartFrame = animationClipSettings.startTime * this.m_Clip.frameRate;
                this.m_DraggingStopFrame = animationClipSettings.stopTime * this.m_Clip.frameRate;
                this.m_DraggingAdditivePoseFrame = animationClipSettings.additiveReferencePoseTime * this.m_Clip.frameRate;
                animationClipSettings.startTime = 0f;
                animationClipSettings.stopTime = this.m_InitialClipLength;
                AnimationUtility.SetAnimationClipSettingsNoDirty(this.m_Clip, animationClipSettings);
                this.DestroyController();
            }
            if (this.m_DraggingRangeEnd)
            {
                animationClipSettings.loopTime = this.m_LoopTime;
                animationClipSettings.loopBlend = this.m_LoopBlend;
                animationClipSettings.loopBlendOrientation = this.m_LoopBlendOrientation;
                animationClipSettings.loopBlendPositionY = this.m_LoopBlendPositionY;
                animationClipSettings.loopBlendPositionXZ = this.m_LoopBlendPositionXZ;
            }
            if ((EditorGUI.EndChangeCheck() || this.m_DraggingRangeEnd) && !this.m_DraggingRange)
            {
                Undo.RegisterCompleteObjectUndo(this.m_Clip, "Muscle Clip Edit");
                AnimationUtility.SetAnimationClipSettingsNoDirty(this.m_Clip, animationClipSettings);
                EditorUtility.SetDirty(this.m_Clip);
                this.DestroyController();
            }
        }

        internal override void OnAssetStoreInspectorGUI()
        {
            this.OnInspectorGUI();
        }

        private void OnDisable()
        {
            this.DestroyController();
            if (this.m_AvatarPreview != null)
            {
                this.m_AvatarPreview.OnDestroy();
            }
        }

        private void OnEnable()
        {
            this.m_Clip = base.target as AnimationClip;
            this.m_InitialClipLength = this.m_Clip.stopTime - this.m_Clip.startTime;
            if (this.m_TimeArea == null)
            {
                this.m_TimeArea = new TimeArea(true);
                this.m_TimeArea.hRangeLocked = false;
                this.m_TimeArea.vRangeLocked = true;
                this.m_TimeArea.hSlider = true;
                this.m_TimeArea.vSlider = false;
                this.m_TimeArea.hRangeMin = this.m_Clip.startTime;
                this.m_TimeArea.hRangeMax = this.m_Clip.stopTime;
                this.m_TimeArea.margin = 10f;
                this.m_TimeArea.scaleWithWindow = true;
                this.m_TimeArea.SetShownHRangeInsideMargins(this.m_Clip.startTime, this.m_Clip.stopTime);
                this.m_TimeArea.hTicks.SetTickModulosForFrameRate(this.m_Clip.frameRate);
                this.m_TimeArea.ignoreScrollWheelUntilClicked = true;
            }
            if (this.m_EventTimeArea == null)
            {
                this.m_EventTimeArea = new TimeArea(true);
                this.m_EventTimeArea.hRangeLocked = true;
                this.m_EventTimeArea.vRangeLocked = true;
                this.m_EventTimeArea.hSlider = false;
                this.m_EventTimeArea.vSlider = false;
                this.m_EventTimeArea.hRangeMin = 0f;
                this.m_EventTimeArea.hRangeMax = s_EventTimelineMax;
                this.m_EventTimeArea.margin = 10f;
                this.m_EventTimeArea.scaleWithWindow = true;
                this.m_EventTimeArea.SetShownHRangeInsideMargins(0f, s_EventTimelineMax);
                this.m_EventTimeArea.hTicks.SetTickModulosForFrameRate(60f);
                this.m_EventTimeArea.ignoreScrollWheelUntilClicked = true;
            }
            if (this.m_EventManipulationHandler == null)
            {
                this.m_EventManipulationHandler = new EventManipulationHandler(this.m_EventTimeArea);
            }
        }

        internal override void OnHeaderControlsGUI()
        {
            if (((this.m_ClipInfo != null) && (this.takeNames != null)) && (this.takeNames.Length > 1))
            {
                EditorGUIUtility.labelWidth = 80f;
                this.takeIndex = EditorGUILayout.Popup("Source Take", this.takeIndex, this.takeNames, new GUILayoutOption[0]);
            }
            else
            {
                base.OnHeaderControlsGUI();
                ModelImporter atPath = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(base.target)) as ModelImporter;
                if ((atPath != null) && (this.m_ClipInfo == null))
                {
                    GUILayoutOption[] options = new GUILayoutOption[] { GUILayout.ExpandWidth(false) };
                    if (GUILayout.Button("Edit...", EditorStyles.miniButton, options))
                    {
                        EditWithImporter(base.target as AnimationClip);
                    }
                }
            }
        }

        internal override void OnHeaderIconGUI(Rect iconRect)
        {
            Texture2D image = null;
            bool flag = AssetPreview.IsLoadingAssetPreview(base.target.GetInstanceID());
            image = AssetPreview.GetAssetPreview(base.target);
            if (image == null)
            {
                if (flag)
                {
                    base.Repaint();
                }
                image = AssetPreview.GetMiniThumbnail(base.target);
            }
            GUI.DrawTexture(iconRect, image);
        }

        internal override void OnHeaderTitleGUI(Rect titleRect, string header)
        {
            if (this.m_ClipInfo != null)
            {
                this.m_ClipInfo.name = EditorGUI.DelayedTextField(titleRect, this.m_ClipInfo.name, EditorStyles.textField);
            }
            else
            {
                base.OnHeaderTitleGUI(titleRect, header);
            }
        }

        public override void OnInspectorGUI()
        {
            this.Init();
            EditorGUIUtility.labelWidth = 50f;
            EditorGUIUtility.fieldWidth = 30f;
            EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
            using (new EditorGUI.DisabledScope(true))
            {
                GUILayoutOption[] options = new GUILayoutOption[] { GUILayout.Width(46f) };
                GUILayout.Label("Length", EditorStyles.miniLabel, options);
                GUILayout.Label(this.GetClipLength().ToString("0.000"), EditorStyles.miniLabel, new GUILayoutOption[0]);
                GUILayout.FlexibleSpace();
                GUILayout.Label(this.m_Clip.frameRate + " FPS", EditorStyles.miniLabel, new GUILayoutOption[0]);
            }
            EditorGUILayout.EndHorizontal();
            if (!this.m_Clip.legacy)
            {
                this.MuscleClipGUI();
            }
            else
            {
                this.AnimationClipGUI();
            }
        }

        public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
        {
            bool flag = Event.current.type == EventType.Repaint;
            this.InitController();
            if (flag)
            {
                this.m_AvatarPreview.timeControl.Update();
            }
            AnimationClip target = base.target as AnimationClip;
            AnimationClipSettings animationClipSettings = AnimationUtility.GetAnimationClipSettings(target);
            this.m_AvatarPreview.timeControl.loop = true;
            if (flag && (this.m_AvatarPreview.PreviewObject != null))
            {
                if (!target.legacy && (this.m_AvatarPreview.Animator != null))
                {
                    if (this.m_State != null)
                    {
                        this.m_State.iKOnFeet = this.m_AvatarPreview.IKOnFeet;
                    }
                    float normalizedTime = ((animationClipSettings.stopTime - animationClipSettings.startTime) == 0f) ? 0f : ((this.m_AvatarPreview.timeControl.currentTime - animationClipSettings.startTime) / (animationClipSettings.stopTime - animationClipSettings.startTime));
                    this.m_AvatarPreview.Animator.Play(0, 0, normalizedTime);
                    this.m_AvatarPreview.Animator.Update(this.m_AvatarPreview.timeControl.deltaTime);
                }
                else
                {
                    target.SampleAnimation(this.m_AvatarPreview.PreviewObject, this.m_AvatarPreview.timeControl.currentTime);
                }
            }
            this.m_AvatarPreview.DoAvatarPreview(r, background);
        }

        public override void OnPreviewSettings()
        {
            this.m_AvatarPreview.DoPreviewSettings();
        }

        private void SetPreviewAvatar()
        {
            this.DestroyController();
            this.InitController();
        }

        public void ShowRange(AnimationClipInfoProperties info)
        {
            this.UpdateEventsPopupClipInfo(info);
            this.m_ClipInfo = info;
            info.AssignToPreviewClip(this.m_Clip);
        }

        private void UpdateEventsPopupClipInfo(AnimationClipInfoProperties info)
        {
            if (this.m_EventManipulationHandler != null)
            {
                this.m_EventManipulationHandler.UpdateEvent(info);
            }
        }

        public AvatarMask mask
        {
            get => 
                this.m_Mask;
            set
            {
                this.m_Mask = value;
            }
        }

        public bool needsToGenerateClipInfo
        {
            get => 
                this.m_NeedsToGenerateClipInfo;
            set
            {
                this.m_NeedsToGenerateClipInfo = value;
            }
        }

        public int takeIndex { get; set; }

        public string[] takeNames { get; set; }

        private static class Styles
        {
            public static GUIContent AddEventContent = EditorGUIUtility.IconContent("Animation.AddEvent", "|Add Event.");
            public static GUIContent AdditiveReferencePoseFrame = EditorGUIUtility.TextContent("Pose Frame|Pose Frame.");
            public static GUIContent AddKeyframeContent = EditorGUIUtility.IconContent("Animation.AddKeyframe", "|Add Keyframe.");
            public static GUIContent BakeIntoPoseOrientation = EditorGUIUtility.TextContent("Bake Into Pose|Enable to make root rotation be baked into the movement of the bones. Disable to make root rotation be stored as root motion.");
            public static GUIContent BakeIntoPosePositionXZ = EditorGUIUtility.TextContent("Bake Into Pose|Enable to make horizontal root motion be baked into the movement of the bones. Disable to make horizontal root motion be stored as root motion.");
            public static GUIContent BakeIntoPosePositionY = EditorGUIUtility.TextContent("Bake Into Pose|Enable to make vertical root motion be baked into the movement of the bones. Disable to make vertical root motion be stored as root motion.");
            public static GUIContent BasedUponOrientation = EditorGUIUtility.TextContent("Based Upon|What the root rotation is based upon.");
            public static GUIContent BasedUponPositionXZ = EditorGUIUtility.TextContent("Based Upon|What the horizontal root position is based upon.");
            public static GUIContent[] BasedUponPositionXZHumanOpt = new GUIContent[] { EditorGUIUtility.TextContent("Original|Keeps the horizontal position as it is authored in the source file."), EditorGUIUtility.TextContent("Center of Mass|Keeps the center of mass aligned with root transform position.") };
            public static GUIContent[] BasedUponPositionXZOpt = new GUIContent[] { EditorGUIUtility.TextContent("Original|Keeps the horizontal position as it is authored in the source file."), EditorGUIUtility.TextContent("Root Node Position") };
            public static GUIContent BasedUponPositionY = EditorGUIUtility.TextContent("Based Upon|What the vertical root position is based upon.");
            public static GUIContent[] BasedUponPositionYHumanOpt = new GUIContent[] { EditorGUIUtility.TextContent("Original|Keeps the vertical position as it is authored in the source file."), EditorGUIUtility.TextContent("Center of Mass|Keeps the center of mass aligned with root transform position."), EditorGUIUtility.TextContent("Feet|Keeps the feet aligned with the root transform position.") };
            public static GUIContent[] BasedUponPositionYOpt = new GUIContent[] { EditorGUIUtility.TextContent("Original|Keeps the vertical position as it is authored in the source file."), EditorGUIUtility.TextContent("Root Node Position") };
            public static GUIContent[] BasedUponRotationHumanOpt = new GUIContent[] { EditorGUIUtility.TextContent("Original|Keeps the rotation as it is authored in the source file."), EditorGUIUtility.TextContent("Body Orientation|Keeps the upper body pointing forward.") };
            public static GUIContent[] BasedUponRotationOpt = new GUIContent[] { EditorGUIUtility.TextContent("Original|Keeps the rotation as it is authored in the source file."), EditorGUIUtility.TextContent("Root Node Rotation|Keeps the upper body pointing forward.") };
            public static GUIContent BasedUponStartOrientation = EditorGUIUtility.TextContent("Based Upon (at Start)|What the root rotation is based upon.");
            public static GUIContent BasedUponStartPositionXZ = EditorGUIUtility.TextContent("Based Upon (at Start)|What the horizontal root position is based upon.");
            public static GUIContent BasedUponStartPositionY = EditorGUIUtility.TextContent("Based Upon (at Start)|What the vertical root position is based upon.");
            public static GUIContent Curves = EditorGUIUtility.TextContent("Curves|Parameter-related curves.");
            public static GUIContent EndFrame = EditorGUIUtility.TextContent("End|End frame of the clip.");
            public static GUIContent GreenLightIcon = EditorGUIUtility.IconContent("lightMeter/greenLight");
            public static GUIContent HasAdditiveReferencePose = EditorGUIUtility.TextContent("Additive Reference Pose|Enable to define the additive reference pose frame.");
            public static GUIContent LightRimIcon = EditorGUIUtility.IconContent("lightMeter/lightRim");
            public static GUIContent LoopCycleOffset = EditorGUIUtility.TextContent("Cycle Offset|Offset to the cycle of a looping animation, if we want to start it at a different time.");
            public static GUIContent LoopPose = EditorGUIUtility.TextContent("Loop Pose|Enable to make the animation loop seamlessly.");
            public static GUIContent LoopTime = EditorGUIUtility.TextContent("Loop Time|Enable to make the animation plays through and then restarts when the end is reached.");
            public static GUIContent Mirror = EditorGUIUtility.TextContent("Mirror|Mirror left and right in this clip.");
            public static GUIContent MotionCurves = EditorGUIUtility.TextContent("Root Motion is driven by curves");
            public static GUIContent NextKeyContent = EditorGUIUtility.IconContent("Animation.NextKey", "|Go to next key frame.");
            public static GUIContent OrangeLightIcon = EditorGUIUtility.IconContent("lightMeter/orangeLight");
            public static GUIContent OrientationOffsetY = EditorGUIUtility.TextContent("Offset|Offset to the root rotation (in degrees).");
            public static GUIContent PositionOffsetY = EditorGUIUtility.TextContent("Offset|Offset to the vertical root position.");
            public static GUIContent PrevKeyContent = EditorGUIUtility.IconContent("Animation.PrevKey", "|Go to previous key frame.");
            public static GUIContent RedLightIcon = EditorGUIUtility.IconContent("lightMeter/redLight");
            public static GUIContent StartFrame = EditorGUIUtility.TextContent("Start|Start frame of the clip.");
        }
    }
}

