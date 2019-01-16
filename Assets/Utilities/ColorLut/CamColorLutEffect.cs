using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class CamColorLutEffect : MonoBehaviour
{
    [SerializeField]
    private Material _material;

    [SerializeField]
    private Texture2D _lutTexture2D;

    [SerializeField]
    private Texture3D _lutTexture3D;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_lutTexture3D == null && _lutTexture2D)
            _lutTexture3D = ColorLutUtility.CreateLut3D(_lutTexture2D);

        if (_material && _lutTexture3D)
        {   
            _material.SetTexture("_ColorLut", _lutTexture3D);
            Graphics.Blit(source, destination, _material);
        }

        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
