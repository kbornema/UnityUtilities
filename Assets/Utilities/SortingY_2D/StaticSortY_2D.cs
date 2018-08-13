using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Sorting2D
{
    [ExecuteInEditMode]
    public class StaticSortY_2D : ASortY_2D
    {
        private void Start()
        {
            Update();
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Application.isEditor && !EditorApplication.isPlaying)
                UpdateZ();

            else
                SortOnce();
#else
            SortOnce();
#endif
        }

        private void SortOnce()
        {
            UpdateZ();
            enabled = false;
        }
    }
}