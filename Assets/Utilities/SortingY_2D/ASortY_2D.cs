using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Sorting2D
{
    public abstract class ASortY_2D : MonoBehaviour
    {
        /// <summary> The y position gets multiplied with this value to have more granular sorting. </summary>
        protected const float POS_SCALE = -25.0f;

        [SerializeField]
        private Transform _myPivot;
        [SerializeField]
        private float _sortOffset = 0.0f;
        [SerializeField]
        private SortingGroup _sortingGroup;
        [SerializeField]
        private List<Renderer> _renderers;
        private int _oldOrder = int.MaxValue;

        private void Reset()
        {
            if (!_myPivot)
                _myPivot = transform;

            _renderers = new List<Renderer>(GetComponents<Renderer>());

            _sortingGroup = GetComponent<SortingGroup>();
        }

        public void SetPivot(Transform t)
        {
            _myPivot = t;
            _oldOrder = int.MaxValue;
            UpdateZ();
        }
        
        protected void UpdateZ()
        {
            if (!_myPivot)
            {
                Debug.LogWarning("Can't update the z ordering of '" + gameObject.name + "' because it has no pivot!", gameObject);
                return;
            }

            int curOrder = (int)((_myPivot.position.y + _sortOffset) * POS_SCALE);

            if (curOrder != _oldOrder)
            {
                for (int i = 0; i < _renderers.Count; i++)
                    _renderers[i].sortingOrder = curOrder;

                if (_sortingGroup)
                    _sortingGroup.sortingOrder = curOrder;

                _oldOrder = curOrder;
            }
        }
    }
}