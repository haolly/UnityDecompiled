﻿namespace UnityEditor
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal class ShapeEditorSelection : IEnumerable<int>, IEnumerable
    {
        [CompilerGenerated]
        private static Func<int, int> <>f__am$cache0;
        private const float k_MinSelectionSize = 6f;
        private readonly int k_RectSelectionID = GUIUtility.GetPermanentControlID();
        private bool m_RectSelecting;
        private HashSet<int> m_SelectedPoints = new HashSet<int>();
        private Vector2 m_SelectMousePoint;
        private Vector2 m_SelectStartPoint;
        private ShapeEditor m_ShapeEditor;

        public ShapeEditorSelection(ShapeEditor owner)
        {
            this.m_ShapeEditor = owner;
        }

        public void Clear()
        {
            this.m_SelectedPoints.Clear();
            if (this.m_ShapeEditor != null)
            {
                this.m_ShapeEditor.activePoint = -1;
            }
        }

        public bool Contains(int i) => 
            this.m_SelectedPoints.Contains(i);

        public void DeleteSelection()
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = x => x;
            }
            IOrderedEnumerable<int> enumerable = Enumerable.OrderByDescending<int, int>(this.m_SelectedPoints, <>f__am$cache0);
            foreach (int num in enumerable)
            {
                this.m_ShapeEditor.RemovePointAt(num);
            }
            if (this.m_ShapeEditor.activePoint >= this.m_ShapeEditor.GetPointsCount())
            {
                this.m_ShapeEditor.activePoint = this.m_ShapeEditor.GetPointsCount() - 1;
            }
            this.m_SelectedPoints.Clear();
        }

        public IEnumerator<int> GetEnumerator() => 
            this.m_SelectedPoints.GetEnumerator();

        public void MoveSelection(Vector3 delta)
        {
            if (delta.sqrMagnitude >= float.Epsilon)
            {
                foreach (int num in this.m_SelectedPoints)
                {
                    this.m_ShapeEditor.SetPointPosition(num, this.m_ShapeEditor.GetPointPosition(num) + delta);
                }
            }
        }

        public void OnGUI()
        {
            ShapeEditor.SelectionType normal;
            Event current = Event.current;
            Handles.BeginGUI();
            Vector2 mousePosition = current.mousePosition;
            int controlID = this.k_RectSelectionID;
            switch (current.GetTypeForControl(controlID))
            {
                case EventType.MouseDown:
                    if ((HandleUtility.nearestControl == controlID) && (current.button == 0))
                    {
                        GUIUtility.hotControl = controlID;
                        this.m_SelectStartPoint = mousePosition;
                    }
                    goto Label_025C;

                case EventType.MouseUp:
                    if ((GUIUtility.hotControl != controlID) || (current.button != 0))
                    {
                        goto Label_025C;
                    }
                    GUIUtility.hotControl = 0;
                    GUIUtility.keyboardControl = 0;
                    if (!this.m_RectSelecting)
                    {
                        this.m_SelectedPoints.Clear();
                        this.m_ShapeEditor.activePoint = -1;
                        this.m_ShapeEditor.Repaint();
                        current.Use();
                        goto Label_025C;
                    }
                    this.m_SelectMousePoint = new Vector2(mousePosition.x, mousePosition.y);
                    normal = ShapeEditor.SelectionType.Normal;
                    if (!Event.current.control)
                    {
                        if (Event.current.shift)
                        {
                            normal = ShapeEditor.SelectionType.Additive;
                        }
                        break;
                    }
                    normal = ShapeEditor.SelectionType.Subtractive;
                    break;

                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == controlID)
                    {
                        if (!this.m_RectSelecting)
                        {
                            Vector2 vector2 = mousePosition - this.m_SelectStartPoint;
                            if (vector2.magnitude > 6f)
                            {
                                this.m_RectSelecting = true;
                            }
                        }
                        if (this.m_RectSelecting)
                        {
                            this.m_SelectMousePoint = new Vector2(mousePosition.x, mousePosition.y);
                            ShapeEditor.SelectionType type = ShapeEditor.SelectionType.Normal;
                            if (Event.current.control)
                            {
                                type = ShapeEditor.SelectionType.Subtractive;
                            }
                            else if (Event.current.shift)
                            {
                                type = ShapeEditor.SelectionType.Additive;
                            }
                            this.RectSelect(EditorGUIExt.FromToRect(this.m_SelectMousePoint, this.m_SelectStartPoint), type);
                        }
                        current.Use();
                    }
                    goto Label_025C;

                case EventType.Repaint:
                    if ((GUIUtility.hotControl == controlID) && this.m_RectSelecting)
                    {
                        EditorStyles.selectionRect.Draw(EditorGUIExt.FromToRect(this.m_SelectStartPoint, this.m_SelectMousePoint), GUIContent.none, false, false, false, false);
                    }
                    goto Label_025C;

                case EventType.Layout:
                    if (!Tools.viewToolActive)
                    {
                        HandleUtility.AddDefaultControl(controlID);
                    }
                    goto Label_025C;

                default:
                    goto Label_025C;
            }
            this.RectSelect(EditorGUIExt.FromToRect(this.m_SelectMousePoint, this.m_SelectStartPoint), normal);
            this.m_RectSelecting = false;
            current.Use();
        Label_025C:
            Handles.EndGUI();
        }

        public void RectSelect(Rect rect, ShapeEditor.SelectionType type)
        {
            if (type == ShapeEditor.SelectionType.Normal)
            {
                this.m_SelectedPoints.Clear();
                this.m_ShapeEditor.activePoint = -1;
                type = ShapeEditor.SelectionType.Additive;
            }
            for (int i = 0; i < this.m_ShapeEditor.GetPointsCount(); i++)
            {
                Vector2 point = this.m_ShapeEditor.LocalToScreen(this.m_ShapeEditor.GetPointPosition(i));
                if (rect.Contains(point))
                {
                    this.SelectPoint(i, type);
                }
            }
            this.m_ShapeEditor.Repaint();
        }

        public void SelectPoint(int i, ShapeEditor.SelectionType type)
        {
            switch (type)
            {
                case ShapeEditor.SelectionType.Normal:
                    this.m_SelectedPoints.Clear();
                    this.m_ShapeEditor.activePoint = i;
                    this.m_SelectedPoints.Add(i);
                    break;

                case ShapeEditor.SelectionType.Additive:
                    this.m_ShapeEditor.activePoint = i;
                    this.m_SelectedPoints.Add(i);
                    break;

                case ShapeEditor.SelectionType.Subtractive:
                    this.m_ShapeEditor.activePoint = (i <= 0) ? 0 : (i - 1);
                    this.m_SelectedPoints.Remove(i);
                    break;

                default:
                    this.m_ShapeEditor.activePoint = i;
                    break;
            }
            this.m_ShapeEditor.Repaint();
        }

        IEnumerator IEnumerable.GetEnumerator() => 
            this.GetEnumerator();

        public int Count =>
            this.m_SelectedPoints.Count;

        public HashSet<int> indices =>
            this.m_SelectedPoints;

        public bool isSelecting =>
            (GUIUtility.hotControl == this.k_RectSelectionID);
    }
}

