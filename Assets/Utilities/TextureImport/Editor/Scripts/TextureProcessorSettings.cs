using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CustomTextureImport
{
    [CreateAssetMenu(menuName = "Misc/TextureImport/PostProcessorSettings")]
    public class TextureProcessorSettings : ScriptableObject
    {
        [SerializeField]
        private List<TextureImportSettings> _importerSettings;
        [SerializeField]
        private List<DefaultAsset> _foldersToIgnore;

        public bool IsAllowedFolder(string assetPath)
        {
            for (int i = 0; i < _foldersToIgnore.Count; i++)
            {
                var path = AssetDatabase.GetAssetPath(_foldersToIgnore[i]);

                if (assetPath.StartsWith(path))
                    return false;
            }

            return true;
        }

        public TextureImportSettings GetImporter(string assetPath, TextureImporter textureImporter)
        {
            for (int i = _importerSettings.Count - 1; i >= 0; i--)
            {
                if(_importerSettings[i] == null)
                {
                    _importerSettings.RemoveAt(i);
                    continue;
                }

                if (_importerSettings[i].CanImport(assetPath, textureImporter))
                    return _importerSettings[i];
            }

            return null;
        }
    }
}