using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ColorLutUtility
{       
    /// <summary> 
    /// Creates a Texture3D from a Texture2D with the same content but rearranged to have dimensions: height, height, height. 
    /// NOTE: format of <paramref name="tex2d"/> needs to be: (width * depth, height), where width, depth and height should have the same dimensions.
    /// </summary>
    public static Texture3D CreateLut3D(Texture2D tex2d)
    {
        int dim = tex2d.height;
        Color[] colors2D = tex2d.GetPixels();
        Color[] colors3D = new Color[dim * dim * dim];
        
        for (int i = 0; i < dim; i++)
        {
            for (int j = 0; j < dim; j++)
            {
                for (int k = 0; k < dim; k++)
                {
                    int index3d = Map3dTo1d(i, j, k, dim, dim, dim);
                    colors3D[index3d] = colors2D[k * dim + i + j * dim * dim];
                }
            }
        }

        Texture3D tex3D = new Texture3D(dim, dim, dim, TextureFormat.RGBA32, false)
        {
            //important otherwise artifacts appear:
            filterMode = FilterMode.Bilinear,
            //important otherwise artifacts appear:
            wrapMode = TextureWrapMode.Clamp
        };

        tex3D.SetPixels(colors3D);
        tex3D.Apply();
        return tex3D;
    }

#if UNITY_EDITOR
    /// <summary> 
    /// Creates a lut Texture2D as an asset in the AssetDatabase. <paramref name="assetName"/> should neither contain the asset path, nor a file extension.
    /// NOTE: Only available in editor. 
    /// </summary>
    public static void CreateDefaultLut2dAsset(string assetName, int dim, bool swapGreenBlue)
    {   
        byte[] pngData = CreateDefaultLut2d(dim, true).EncodeToPNG();
        string path = string.Concat("Assets", Path.DirectorySeparatorChar, assetName, ".png");
        File.WriteAllBytes(path, pngData);
        UnityEditor.AssetDatabase.Refresh();
    }
#endif

    /// <summary> Creates a default lut (mapping colors to themselves) as a Texture2D with dimensions: (dim * dim, dim). </summary>
    public static Texture2D CreateDefaultLut2d(int dim, bool swapGreenBlue)
    {
        int width = dim;
        int height = dim;
        int depth = dim;

        Color[] colors = new Color[width * height * depth];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < dim; z++)
                {
                    float red = (float)x / (width - 1);
                    float green = (float)y / (height - 1);
                    float blue = (float)z / (depth - 1);

                    if(swapGreenBlue)
                    {
                        float tmp = green;
                        green = blue;
                        blue = tmp;
                    }

                    colors[Map3dTo1d(x, y, z, width, height, depth)] = new Color(red, green, blue, 1.0f);
                }
            }
        }

        Texture2D tex = new Texture2D(width * depth, height, TextureFormat.ARGB32, false)
        {
            anisoLevel = 0
        };

        tex.SetPixels(colors);
        tex.Apply();
        return tex;
    }

    public static int Map3dTo1d(Vector3Int pos, Vector3Int size)
    {
        return pos.x + (pos.y * size.x) + (pos.z * size.x * size.y);
    }

    public static int Map2dTo1d(Vector2Int pos, Vector2Int size)
    {
        return pos.x + (pos.y * size.x);
    }

    public static int Map3dTo1d(int x, int y, int z, int width, int height, int depth)
    {
        return x + (y * width) + (z * width * height);
    }
}
