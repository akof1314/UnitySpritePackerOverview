using System;
using System.Reflection;
using UnityEditor;

namespace SpritePackerOverview
{
    public class SpritePackerOverviewUtil
    {
        private Assembly m_Assembly;
        private Type m_TypePackerWindow;
        private FieldInfo m_FieldInfoSelectedAtlas;
        private FieldInfo m_FieldInfoSelectedPage;
        private EditorWindow m_FirstPackerWindow;
        
        private Assembly assembly
        {
            get
            {
                if (m_Assembly == null)
                {
                    m_Assembly = Assembly.GetAssembly(typeof(EditorGUIUtility));
                }
                return m_Assembly;
            }
        }

        private Type packerWindowType
        {
            get
            {
                if (m_TypePackerWindow == null)
                {
                    m_TypePackerWindow = assembly.GetType("UnityEditor.Sprites.PackerWindow");
                }
                return m_TypePackerWindow;
            }
        }

        private EditorWindow firstPackerWindow
        {
            get
            {
                if (m_FirstPackerWindow == null)
                {
                    m_FirstPackerWindow = EditorWindow.GetWindow(packerWindowType, false, null, false);
                }
                return m_FirstPackerWindow;
            }
        }

        private FieldInfo selectedAtlasFieldInfo
        {
            get
            {
                if (m_FieldInfoSelectedAtlas == null)
                {
                    m_FieldInfoSelectedAtlas = packerWindowType.GetField("m_SelectedAtlas", BindingFlags.Instance | BindingFlags.NonPublic);
                }
                return m_FieldInfoSelectedAtlas;
            }
        }

        public int selectedAtlas
        {
            get { return (int)selectedAtlasFieldInfo.GetValue(firstPackerWindow); }
            set { selectedAtlasFieldInfo.SetValue(firstPackerWindow, value);}
        }

        private FieldInfo selectedPageFieldInfo
        {
            get
            {
                if (m_FieldInfoSelectedPage == null)
                {
                    m_FieldInfoSelectedPage = packerWindowType.GetField("m_SelectedPage", BindingFlags.Instance | BindingFlags.NonPublic);
                }
                return m_FieldInfoSelectedPage;
            }
        }

        public int selectedPage
        {
            get { return (int)selectedPageFieldInfo.GetValue(firstPackerWindow); }
            set { selectedPageFieldInfo.SetValue(firstPackerWindow, value); }
        }

        public void Repaint()
        {
            firstPackerWindow.Repaint();
        }
    }
}