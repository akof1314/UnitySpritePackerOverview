using UnityEngine;
using UnityEditor.IMGUI.Controls;

namespace SpritePackerOverview
{
    public class SpritePackerOverviewTreeView : TreeView
    {
        private SpritePackerOverviewUtil m_PackerOverviewUtil;
        private SpritePackerOverviewTreeModel m_TreeModel;
        private int kPageCapacity = 100000;

        public SpritePackerOverviewTreeView(TreeViewState state, MultiColumnHeader multiColumnHeader) : base(state, multiColumnHeader)
        {
            rowHeight = 20f;
            showAlternatingRowBackgrounds = true;
            showBorder = true;
            multiColumnHeader.height = 23f;
            m_PackerOverviewUtil = new SpritePackerOverviewUtil();
        }

        public void SetModel(SpritePackerOverviewTreeModel model)
        {
            m_TreeModel = model;
        }

        protected override TreeViewItem BuildRoot()
        {
            TreeViewItem root = new TreeViewItem { id = 99999999, depth = -1 };
            var atlasNames = m_TreeModel.GetAtlasNames();
            for (var i = 0; i < atlasNames.Length; i++)
            {
                var atlasName = atlasNames[i];
                var pagesCount = m_TreeModel.GetAtlasPagesCount(i);
                for (int j = 0; j < pagesCount; j++)
                {
                    root.AddChild(new TreeViewItem(i * kPageCapacity + j, -1, atlasName));
                }
            }

            return root;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            for (int i = 0; i < args.GetNumVisibleColumns(); ++i)
                CellGUI(args.GetCellRect(i), args.item, args.GetColumn(i), ref args);
        }

        protected void CellGUI(Rect cellRect, TreeViewItem item, int column, ref RowGUIArgs args)
        {
            int pagesIndex = item.id % kPageCapacity;
            int atlasIndex = item.id / kPageCapacity;
            var info = m_TreeModel.GetAtlasPagesInfo(atlasIndex, pagesIndex);

            switch (column)
            {
                case 0:
                    {
                        Rect position = cellRect;
                        position.width = 16f;
                        position.height = 16f;
                        position.y += 2f;
                        position.x += 2f;
                        Texture iconForItem = info.texture;
                        if (iconForItem)
                        {
                            GUI.DrawTexture(position, iconForItem, ScaleMode.ScaleToFit);
                        }

                        cellRect.xMin += 20f;
                        DefaultGUI.Label(cellRect, item.displayName, args.selected, args.focused);
                    }
                    break;
                case 1:
                    DefaultGUI.Label(cellRect, string.Format("Page {0}", pagesIndex + 1), args.selected, args.focused);
                    break;
                case 2:
                    DefaultGUI.Label(cellRect, info.size, args.selected, args.focused);
                    break;
                case 3:
                    DefaultGUI.Label(cellRect, info.format, args.selected, args.focused);
                    break;
            }
        }

        protected override void DoubleClickedItem(int id)
        {
            int pagesIndex = id % kPageCapacity;
            int atlasIndex = id / kPageCapacity;

            m_PackerOverviewUtil.selectedAtlas = atlasIndex;
            m_PackerOverviewUtil.selectedPage = pagesIndex;
            m_PackerOverviewUtil.Repaint();
        }
    }
}