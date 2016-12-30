﻿namespace UnityEditor.TestTools.TestRunner.GUI
{
    using NUnit.Framework.Interfaces;
    using NUnit.Framework.Internal;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEditor.IMGUI.Controls;
    using UnityEngine.TestTools.TestRunner.GUI;

    internal class TestTreeViewBuilder
    {
        private List<TestRunnerResult> m_OldTestResultList;
        private ITest m_TestListRoot;
        public List<TestRunnerResult> results = new List<TestRunnerResult>();

        public TestTreeViewBuilder(ITest tests, List<TestRunnerResult> oldTestResultResults)
        {
            this.m_OldTestResultList = oldTestResultResults;
            this.m_TestListRoot = tests;
        }

        public TreeViewItem BuildTreeView(TestFilterSettings settings, bool sceneBased, string sceneName)
        {
            TreeViewItem rootItem = new TreeViewItem(0x7fffffff, 0, null, "Invisible Root Item");
            this.ParseTestTree(0, rootItem, this.m_TestListRoot);
            return rootItem;
        }

        private void ParseTestTree(int depth, TreeViewItem rootItem, ITest testElement)
        {
            <ParseTestTree>c__AnonStorey0 storey = new <ParseTestTree>c__AnonStorey0 {
                testElement = testElement
            };
            if ((storey.testElement.RunState != RunState.NotRunnable) && (storey.testElement.RunState != RunState.Skipped))
            {
                if (storey.testElement is TestMethod)
                {
                    TestRunnerResult result = Enumerable.FirstOrDefault<TestRunnerResult>(this.m_OldTestResultList, new Func<TestRunnerResult, bool>(storey.<>m__0));
                    if (result == null)
                    {
                        result = new TestRunnerResult(storey.testElement);
                    }
                    this.results.Add(result);
                    TestLineTreeViewItem child = new TestLineTreeViewItem((Test) storey.testElement, depth, rootItem);
                    rootItem.AddChild(child);
                    child.SetResult(result);
                }
                else
                {
                    TestRunnerResult item = Enumerable.FirstOrDefault<TestRunnerResult>(this.m_OldTestResultList, new Func<TestRunnerResult, bool>(storey.<>m__1));
                    if (item == null)
                    {
                        item = new TestRunnerResult(storey.testElement);
                    }
                    this.results.Add(item);
                    TestGroupTreeViewItem item2 = new TestGroupTreeViewItem((Test) storey.testElement, depth, rootItem);
                    item2.SetResult(item);
                    depth++;
                    foreach (ITest test in storey.testElement.Tests)
                    {
                        this.ParseTestTree(depth, item2, test);
                    }
                    if (item2.hasChildren)
                    {
                        rootItem.AddChild(item2);
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <ParseTestTree>c__AnonStorey0
        {
            internal ITest testElement;

            internal bool <>m__0(TestRunnerResult a) => 
                (a.id == this.testElement.FullName);

            internal bool <>m__1(TestRunnerResult a) => 
                (a.id == this.testElement.FullName);
        }
    }
}
