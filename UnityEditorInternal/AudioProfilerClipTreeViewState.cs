﻿namespace UnityEditorInternal
{
    using System;
    using UnityEditor.IMGUI.Controls;
    using UnityEngine;

    internal class AudioProfilerClipTreeViewState : TreeViewState
    {
        [SerializeField]
        public int prevSelectedColumn = 1;
        [SerializeField]
        public int selectedColumn = 2;
        [SerializeField]
        public bool sortByDescendingOrder = true;

        public void SetSelectedColumn(int index)
        {
            if (index != this.selectedColumn)
            {
                this.prevSelectedColumn = this.selectedColumn;
            }
            else
            {
                this.sortByDescendingOrder = !this.sortByDescendingOrder;
            }
            this.selectedColumn = index;
        }
    }
}

