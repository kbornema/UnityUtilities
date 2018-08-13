//Script originally by Ryan Nielson (http://nielson.io/2016/04/2d-sprite-outlines-in-unity/)
//External outline added by Chris Garcia (thespinforce@gmail.com)

using UnityEngine;

[RequireComponent(typeof(SpriteRenderer)), ExecuteInEditMode]
public class SpriteOutline : MonoBehaviour
{
    [SerializeField]
    private Color _outlineColor = Color.white;
    public Color OutlineColor
    {
        get { return _outlineColor; }
        set { _outlineColor = value; UpdateOutline(); }
    }

    [SerializeField, Range(0, 2)]
    private int _outlineSize = 1;
    public int OutlineSize
    {
        get { return _outlineSize; }
        set { _outlineSize = value; UpdateOutline(); }
    }

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private void Reset()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Application.isEditor)
            UpdateOutline();
    }
#endif

    private void UpdateOutline()
    {
        if (!_spriteRenderer)
        {
            Debug.LogWarning("SpriteRenderer in SpriteOutline is not assigned!", gameObject);
            return;
        }

        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        _spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetColor("_OutlineColor", _outlineColor);
        mpb.SetFloat("_OutlineSize", _outlineSize);
        _spriteRenderer.SetPropertyBlock(mpb);
    }
}