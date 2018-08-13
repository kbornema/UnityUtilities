using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CustomTextureImport
{
    [CreateAssetMenu(menuName = "Misc/TextureImport/ImportSetting")]
    public class TextureImportSettings : ScriptableObject
    {
        [Header("Filter")]
        [SerializeField]
        private string _fileContent;
        [SerializeField]
        private string _fileEnding;

        [Header("Settings")]
        [SerializeField]
        private string _packingTag;
        [SerializeField]
        private float _pixelsPerUnit;
        [SerializeField]
        private TextureImporterType _textureType;
        [SerializeField]
        private bool _mipMap;
        [SerializeField]
        private FilterMode _filterMode;
        [SerializeField]
        private TextureImporterCompression _compression;
        [SerializeField]
        private List<DefaultAsset> _folders;
        [SerializeField]
        private List<DefaultAsset> _foldersToIgnore;

        public bool CanImport(string assetPath, TextureImporter textureImporter)
        {   
            if (!assetPath.EndsWith(_fileEnding, System.StringComparison.OrdinalIgnoreCase))
                return false;

            if(_fileContent.Length > 0 && !assetPath.Contains(_fileContent))
                return false;

            for (int i = _foldersToIgnore.Count - 1; i >= 0; i--)
            {
                if (_foldersToIgnore[i] == null)
                {
                    _foldersToIgnore.RemoveAt(i);
                    continue;
                }

                var folderPath = AssetDatabase.GetAssetPath(_foldersToIgnore[i]);

                if (assetPath.StartsWith(folderPath))
                    return false;
            }

            for (int i = _folders.Count - 1; i >= 0; i--)
            {
                if(_folders[i] == null)
                {
                    _folders.RemoveAt(i);
                    continue;
                }

                var folderPath = AssetDatabase.GetAssetPath(_folders[i]);

                if (assetPath.StartsWith(folderPath))
                    return true;
            }

            return false;
        }

        public bool Import(string assetPath, TextureImporter textureImporter)
        {
            textureImporter.textureType = _textureType;
            textureImporter.spritePackingTag = _packingTag;
            textureImporter.mipmapEnabled = _mipMap;
            textureImporter.filterMode = _filterMode;
            textureImporter.spritePixelsPerUnit = _pixelsPerUnit;
            textureImporter.textureCompression = _compression;

            return true;
        }
    }
}