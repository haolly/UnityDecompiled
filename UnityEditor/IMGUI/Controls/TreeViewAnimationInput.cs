﻿namespace UnityEditor.IMGUI.Controls
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEditor;
    using UnityEngine;

    internal class TreeViewAnimationInput
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        private double <animationDuration>k__BackingField;
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        private int <endRow>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <expanding>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TreeViewItem <item>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Rect <rowsRect>k__BackingField;
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        private int <startRow>k__BackingField;
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        private Rect <startRowRect>k__BackingField;
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        private double <startTime>k__BackingField;
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        private TreeViewController <treeView>k__BackingField;
        public Action<TreeViewAnimationInput> animationEnded;

        public void FireAnimationEndedEvent()
        {
            if (this.animationEnded != null)
            {
                this.animationEnded(this);
            }
        }

        public override string ToString()
        {
            object[] objArray1 = new object[] { "Input: startRow ", this.startRow, " endRow ", this.endRow, " rowsRect ", this.rowsRect, " startTime ", this.startTime, " anitmationDuration", this.animationDuration, " ", this.expanding, " ", this.item.displayName };
            return string.Concat(objArray1);
        }

        public double animationDuration { get; set; }

        public double elapsedTime
        {
            get => 
                (EditorApplication.timeSinceStartup - this.startTime);
            set
            {
                this.startTime = EditorApplication.timeSinceStartup - value;
            }
        }

        public float elapsedTimeNormalized =>
            Mathf.Clamp01(((float) this.elapsedTime) / ((float) this.animationDuration));

        public int endRow { get; set; }

        public bool expanding { get; set; }

        public TreeViewItem item { get; set; }

        public Rect rowsRect { get; set; }

        public int startRow { get; set; }

        public Rect startRowRect { get; set; }

        public double startTime { get; set; }

        public TreeViewController treeView { get; set; }
    }
}

