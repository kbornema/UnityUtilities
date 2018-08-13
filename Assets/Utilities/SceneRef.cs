using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "SceneRef", menuName = "Misc/SceneRef", order = 1)]
public sealed class SceneRef : ScriptableObject
{
    [SerializeField]
    private Object _scene;
    [SerializeField, HideInInspector]
    private string _sceneName;

    private void OnValidate()
    {
#if UNITY_EDITOR

        if(_scene && !(_scene is SceneAsset))
        {
            _scene = null;
            Debug.LogError("Only Scenes can be referenced by " + GetType().Name + "!", this);
        }
#endif
        UpdateString();
    }

    //Also called when building an .exe, thus always up to date:
    private void OnEnable()
    {
        UpdateString();
    }

    private void UpdateString(bool force = false)
    {
        if (!_scene)
            return;

        if (_sceneName != _scene.name || force)
            _sceneName = _scene.name;
    }

    public string GetSceneName()
    {
        return _sceneName;
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(GetSceneName());
    }
}