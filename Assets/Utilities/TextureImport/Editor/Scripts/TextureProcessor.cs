using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;


namespace CustomTextureImport
{
    public sealed class TextureProcessor : AssetPostprocessor
    {
        private static TextureProcessorSettings _settings;

        private TextureImportSettings _currentImportSetting;

        private int _waitUpdates = 0;
        private bool _firstImport;
        private bool _isAllowedImport;

        private void OnPreprocessTexture()
        {
            if (_settings == null)
            {
                _settings = FindSettings();

                if (_settings == null)
                    return;
            }        

            _isAllowedImport = _settings.IsAllowedFolder(assetPath);

            if (!_isAllowedImport)
                return;

            TextureImporter textureImporter = assetImporter as TextureImporter;
            _currentImportSetting = _settings.GetImporter(assetPath, textureImporter);

            if (_currentImportSetting == null)
            {
                _isAllowedImport = false;
                return;
            }
            
            _firstImport = _currentImportSetting.Import(assetPath, textureImporter);
        }


        private void OnPostprocessTexture(Texture2D texture)
        {
            if (!_isAllowedImport)
                return;

            // this is used to log after some updates, because the texture here is not yet an Asset, hence it can't be used as a context in a log.
            EditorApplication.update += OnEditorUpdate;
        }

        private void OnEditorUpdate()
        {
            _waitUpdates++;

            if (_waitUpdates >= 25)
            {
                EditorApplication.update -= OnEditorUpdate;

                var t = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);

                if(_firstImport)
                    Debug.Log("Imported Texture at: " + assetPath + " with " + _currentImportSetting.name, t);
                else
                    Debug.Log("Texture has changed at: " + assetPath, t);
            }
        }

        private TextureProcessorSettings FindSettings()
        {
            var typeName = typeof(TextureProcessorSettings).Name;

            var paths = AssetDatabase.FindAssets("t: " + typeName);
            if (paths.Length <= 0)
            {
                Debug.LogWarning("Could not find instance of " + typeName + ". Please create one.");
                return null;
            }

            else if (paths.Length > 1)
            {
                Debug.LogWarning("Found more than one instance of " + typeName + ". This is not allowed.");
                return null;
            }

            return AssetDatabase.LoadAssetAtPath<TextureProcessorSettings>(AssetDatabase.GUIDToAssetPath(paths[0]));
        }
    }
}