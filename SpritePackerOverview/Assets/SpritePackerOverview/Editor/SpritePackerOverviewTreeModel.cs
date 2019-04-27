using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Sprites;

namespace SpritePackerOverview
{
    public class SpritePackerOverviewTreeModel
    {
        public class AtlasInfo
        {
            public Texture texture;
            public string size;
            public string format;
        }

        private string[] m_AtlasNames;
        private List<List<AtlasInfo>> m_AtlasInfos = new List<List<AtlasInfo>>();

        public void Reload()
        {
            RefreshAtlasNameList();
        }

        public bool IsEmpty()
        {
            return m_AtlasNames == null || m_AtlasNames.Length == 0;
        }

        public string[] GetAtlasNames()
        {
            return m_AtlasNames;
        }

        public int GetAtlasPagesCount(int atlasIndex)
        {
            if (atlasIndex >= m_AtlasInfos.Count)
            {
                return 0;
            }

            return m_AtlasInfos[atlasIndex].Count;
        }

        public AtlasInfo GetAtlasPagesInfo(int atlasIndex, int pageIndex)
        {
            return m_AtlasInfos[atlasIndex][pageIndex];
        }

        private void RefreshAtlasNameList()
        {
            m_AtlasNames = Packer.atlasNames;
            m_AtlasInfos.Clear();

            for (var i = 0; i < m_AtlasNames.Length; i++)
            {
                var atlasName = m_AtlasNames[i];
                List<AtlasInfo> infos = new List<AtlasInfo>();
                Texture2D[] textures = Packer.GetTexturesForAtlas(atlasName);
                foreach (var texture in textures)
                {
                    AtlasInfo info = new AtlasInfo
                    {
                        texture = texture,
                        format = texture.format.ToString(),
                        size = string.Format("{0}x{1}", texture.width, texture.height)
                    };
                    infos.Add(info);
                }
                m_AtlasInfos.Add(infos);
            }
        }
    }
}