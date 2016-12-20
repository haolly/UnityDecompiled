﻿namespace UnityEditor
{
    using System;
    using UnityEditor.Modules;
    using UnityEngine;

    internal class DisplayUtility
    {
        private static readonly int[] s_DisplayIndices = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };
        private static GUIContent[] s_GenericDisplayNames = new GUIContent[] { EditorGUIUtility.TextContent("Display 1"), EditorGUIUtility.TextContent("Display 2"), EditorGUIUtility.TextContent("Display 3"), EditorGUIUtility.TextContent("Display 4"), EditorGUIUtility.TextContent("Display 5"), EditorGUIUtility.TextContent("Display 6"), EditorGUIUtility.TextContent("Display 7"), EditorGUIUtility.TextContent("Display 8") };

        public static int[] GetDisplayIndices()
        {
            return s_DisplayIndices;
        }

        public static GUIContent[] GetDisplayNames()
        {
            GUIContent[] displayNames = ModuleManager.GetDisplayNames(EditorUserBuildSettings.activeBuildTarget.ToString());
            return ((displayNames == null) ? s_GenericDisplayNames : displayNames);
        }

        public static GUIContent[] GetGenericDisplayNames()
        {
            return s_GenericDisplayNames;
        }
    }
}

