﻿namespace UnityEditor
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEditor.IMGUI.Controls;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    internal class GameObjectTreeViewItem : TreeViewItem
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        private bool <isSceneHeader>k__BackingField;
        private int m_ColorCode;
        private UnityEngine.Object m_ObjectPPTR;
        private bool m_ShouldDisplay;
        private Scene m_UnityScene;

        public GameObjectTreeViewItem(int id, int depth, TreeViewItem parent, string displayName) : base(id, depth, parent, displayName)
        {
        }

        public virtual int colorCode
        {
            get => 
                this.m_ColorCode;
            set
            {
                this.m_ColorCode = value;
            }
        }

        public override string displayName
        {
            get
            {
                if (string.IsNullOrEmpty(base.displayName))
                {
                    if (this.m_ObjectPPTR != null)
                    {
                        this.displayName = this.objectPPTR.name;
                    }
                    else
                    {
                        this.displayName = "deleted gameobject";
                    }
                }
                return base.displayName;
            }
            set
            {
                base.displayName = value;
            }
        }

        public bool isSceneHeader { get; set; }

        public virtual UnityEngine.Object objectPPTR
        {
            get => 
                this.m_ObjectPPTR;
            set
            {
                this.m_ObjectPPTR = value;
            }
        }

        public Scene scene
        {
            get => 
                this.m_UnityScene;
            set
            {
                this.m_UnityScene = value;
            }
        }

        public virtual bool shouldDisplay
        {
            get => 
                this.m_ShouldDisplay;
            set
            {
                this.m_ShouldDisplay = value;
            }
        }
    }
}

