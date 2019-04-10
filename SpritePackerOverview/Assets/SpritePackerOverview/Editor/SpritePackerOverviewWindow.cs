using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SpritePackerOverview
{
    public class SpritePackerOverviewWindow : EditorWindow
    {
        [MenuItem("Window/Sprite Packer Overview")]
        static void ShowWindow()
        {
            GetWindow<SpritePackerOverviewWindow>();
        }

        private class OverviewWindowStyle
        {
            public static readonly GUIContent packLabel = new GUIContent("Pack");
            public static readonly GUIContent repackLabel = new GUIContent("Repack");
            public static readonly GUIContent viewAtlasLabel = new GUIContent("View Atlas:");
            public static readonly GUIContent windowTitle = new GUIContent("Sprite Packer Overview");
            public static readonly GUIContent pageContentLabel = new GUIContent("Page {0}");

            public static readonly GUIContent packingDisabledLabel =
                new GUIContent("Legacy sprite packing is disabled. Enable it in Edit > Project Settings > Editor.");
        }

        private SearchField m_SearchField;
        [SerializeField] private TreeViewState m_TreeViewState;
        [SerializeField] private MultiColumnHeaderState m_MultiColumnHeaderState;
        private SpritePackerOverviewTreeView m_TreeView;
        private SpritePackerOverviewTreeModel m_TreeModel;

        private void Awake()
        {
            minSize = new Vector2(400f, 256f);
            titleContent = OverviewWindowStyle.windowTitle;
        }

        private bool ValidateIsPackingEnabled()
        {
            if (EditorSettings.spritePackerMode != SpritePackerMode.BuildTimeOnly
                && EditorSettings.spritePackerMode != SpritePackerMode.AlwaysOn)
            {
                EditorGUILayout.BeginVertical();
                GUILayout.Label(OverviewWindowStyle.packingDisabledLabel);
                EditorGUILayout.EndVertical();
                return false;
            }

            return true;
        }

        private void OnGUI()
        {
            if (!ValidateIsPackingEnabled())
            {
                return;
            }

            InitIfNeeded();
            DrawToolbar();
            DrawTreeView();
        }

        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            m_TreeView.searchString = m_SearchField.OnToolbarGUI(m_TreeView.searchString);
            EditorGUILayout.EndHorizontal();
        }

        private void DrawTreeView()
        {
            Rect rect = GUILayoutUtility.GetRect(0, 100000, 0, 100000);
            m_TreeView.OnGUI(rect);
        }

        private void InitIfNeeded()
        {
            if (m_SearchField != null)
            {
                return;
            }

            bool firstInit = m_MultiColumnHeaderState == null;
            var headerState = CreateMultiColumnHeader();
            if (MultiColumnHeaderState.CanOverwriteSerializedFields(m_MultiColumnHeaderState, headerState))
            {
                MultiColumnHeaderState.OverwriteSerializedFields(m_MultiColumnHeaderState, headerState);
            }

            m_MultiColumnHeaderState = headerState;

            var multiColumnHeader = new MultiColumnHeader(headerState);
            if (firstInit)
            {
                multiColumnHeader.ResizeToFit();
            }

            if (m_TreeViewState == null)
            {
                m_TreeViewState = new TreeViewState();
            }

            m_TreeModel = new SpritePackerOverviewTreeModel();
            m_TreeView = new SpritePackerOverviewTreeView(m_TreeViewState, multiColumnHeader);
            m_TreeView.SetModel(m_TreeModel);

            m_SearchField = new SearchField();
            m_SearchField.downOrUpArrowKeyPressed += m_TreeView.SetFocusAndEnsureSelectedItem;
            Refresh();
        }

        private void Refresh()
        {
            m_TreeModel.Reload();
            m_TreeView.Reload();
        }

        private MultiColumnHeaderState CreateMultiColumnHeader()
        {
            var columns = new[]
            {
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Atlas Name"),
                    headerTextAlignment = TextAlignment.Left,
                    canSort = false,
                    sortingArrowAlignment = TextAlignment.Right,
                    width = 280,
                    minWidth = 150,
                    autoResize = false,
                    allowToggleVisibility = false
                },
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Page"),
                    headerTextAlignment = TextAlignment.Left,
                    canSort = false,
                    width = 100,
                    minWidth = 70,
                    maxWidth = 120,
                    autoResize = false,
                    allowToggleVisibility = true
                },
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Size"),
                    headerTextAlignment = TextAlignment.Left,
                    canSort = false,
                    width = 100,
                    minWidth = 80,
                    autoResize = false,
                    allowToggleVisibility = true
                },
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Format"),
                    headerTextAlignment = TextAlignment.Left,
                    canSort = false,
                    width = 350,
                    minWidth = 100,
                    autoResize = false,
                    allowToggleVisibility = true
                }
            };

            return new MultiColumnHeaderState(columns);
        }
    }
}