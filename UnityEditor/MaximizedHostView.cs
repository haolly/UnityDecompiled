﻿namespace UnityEditor
{
    using System;
    using UnityEngine;

    internal class MaximizedHostView : HostView
    {
        protected override void AddDefaultItemsToMenu(GenericMenu menu, EditorWindow view)
        {
            if (menu.GetItemCount() != 0)
            {
                menu.AddSeparator("");
            }
            menu.AddItem(EditorGUIUtility.TextContent("Maximize"), !(base.parent is SplitView), new GenericMenu.MenuFunction2(this.Unmaximize), view);
            menu.AddDisabledItem(EditorGUIUtility.TextContent("Close Tab"));
            menu.AddSeparator("");
            Type[] paneTypes = base.GetPaneTypes();
            GUIContent content = EditorGUIUtility.TextContent("Add Tab");
            foreach (Type type in paneTypes)
            {
                if (type != null)
                {
                    GUIContent content2 = new GUIContent(EditorWindow.GetLocalizedTitleContentFromType(type));
                    content2.text = content.text + "/" + content2.text;
                    menu.AddDisabledItem(content2);
                }
            }
        }

        protected override RectOffset GetBorderSize()
        {
            base.m_BorderSize.left = 0;
            base.m_BorderSize.right = 0;
            base.m_BorderSize.top = 0x11;
            base.m_BorderSize.bottom = 4;
            return base.m_BorderSize;
        }

        public void OnGUI()
        {
            base.ClearBackground();
            EditorGUIUtility.ResetGUIState();
            Rect rect = new Rect(-2f, 0f, base.position.width + 4f, base.position.height);
            base.background = "dockarea";
            rect = base.background.margin.Remove(rect);
            Rect position = new Rect(rect.x + 1f, rect.y, rect.width - 2f, 17f);
            if (Event.current.type == EventType.Repaint)
            {
                base.background.Draw(rect, GUIContent.none, false, false, false, false);
                "dragTab".Draw(position, base.actualView.titleContent, false, false, true, base.hasFocus);
            }
            if ((Event.current.type == EventType.ContextClick) && position.Contains(Event.current.mousePosition))
            {
                base.PopupGenericMenu(base.actualView, new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 0f, 0f));
            }
            base.ShowGenericMenu();
            if (base.actualView != null)
            {
                base.actualView.m_Pos = base.borderSize.Remove(base.screenPosition);
            }
            base.InvokeOnGUI(rect);
        }

        private void Unmaximize(object userData)
        {
            EditorWindow win = (EditorWindow) userData;
            WindowLayout.Unmaximize(win);
        }
    }
}

