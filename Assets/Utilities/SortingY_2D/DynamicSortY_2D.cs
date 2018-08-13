using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace Sorting2D
{
    [ExecuteInEditMode]
    public class DynamicSortY_2D : ASortY_2D
    {
        private void Start()
        {
            UpdateZ();
        }

        private void LateUpdate()
        {
            UpdateZ();
        }
    }
}