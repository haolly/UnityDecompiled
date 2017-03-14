﻿namespace UnityEditor.TestTools.TestRunner.GUI
{
    using NUnit.Framework.Interfaces;
    using System;
    using UnityEditor.TestTools.TestRunner;
    using UnityEngine;
    using UnityEngine.TestTools.TestRunner;
    using UnityEngine.TestTools.TestRunner.GUI;

    internal class WindowResultUpdater : ScriptableObject, TestRunnerListener
    {
        public void RunFinished(ITestResult testResults)
        {
        }

        public void RunStarted(ITest testsToRun)
        {
        }

        public void TestFinished(ITestResult test)
        {
            if (TestRunnerWindow.s_Instance != null)
            {
                TestRunnerResult result = new TestRunnerResult(test);
                TestRunnerWindow.s_Instance.m_SelectedTestTypes.UpdateResult(result);
                TestRunnerWindow.s_Instance.m_SelectedTestTypes.Repaint();
                TestRunnerWindow.s_Instance.Repaint();
            }
        }

        public void TestStarted(ITest testName)
        {
        }
    }
}
