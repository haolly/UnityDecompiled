﻿namespace UnityEditor.TreeViewExamples
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEditor.IMGUI.Controls;

    internal class TestDataSource : TreeViewDataSource
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        private int <itemCounter>k__BackingField;
        private BackendData m_Backend;

        public TestDataSource(TreeViewController treeView, BackendData data) : base(treeView)
        {
            this.m_Backend = data;
            this.FetchData();
        }

        private void AddChildrenRecursive(BackendData.Foo source, TreeViewItem dest)
        {
            if (source.hasChildren)
            {
                dest.children = new List<TreeViewItem>(source.children.Count);
                for (int i = 0; i < source.children.Count; i++)
                {
                    BackendData.Foo foo = source.children[i];
                    dest.children.Add(new FooTreeViewItem(foo.id, dest.depth + 1, dest, foo.name, foo));
                    this.itemCounter++;
                    this.AddChildrenRecursive(foo, dest.children[i]);
                }
            }
        }

        public override bool CanBeParent(TreeViewItem item) => 
            true;

        public override void FetchData()
        {
            this.itemCounter = 1;
            base.m_RootItem = new FooTreeViewItem(this.m_Backend.root.id, 0, null, this.m_Backend.root.name, this.m_Backend.root);
            this.AddChildrenRecursive(this.m_Backend.root, base.m_RootItem);
            base.m_NeedRefreshRows = true;
        }

        public int itemCounter { get; private set; }
    }
}

