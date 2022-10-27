using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Color = UnityEngine.Color;

[ExecuteAlways]
public class LitTile : MonoBehaviour
{
    private GameObject _tileObject;
    public static List<PlayerInLitTile> playerList = new List<PlayerInLitTile>();
    public Vector2 tileCount = new Vector2(3,3);
    public Vector2 tileSize;
    [Range(0,0.05f)]public float colorTemporalReduce = 0.05f;
    private Vector3 _tileCenter;
    private Texture2D _emissionTex;
    private Renderer _renderer;
    private Bounds _bounds;
    private Material _material;
    
    private static readonly int EmissionMap = Shader.PropertyToID("_EmissionMap");
    
    private void OnEnable()
    {
        _tileObject = gameObject;
        _tileCenter = _tileObject.transform.position;
        _renderer = _tileObject.GetComponent<Renderer>();
        if(!_renderer) 
            return;
        
        _bounds = _renderer.bounds;
        tileSize.x = _bounds.size.x;
        tileSize.y = _bounds.size.z;
        
        if(tileCount.x < 1 )
            tileCount.x = 1;
        if(tileCount.y < 1)
            tileCount.y = 1;
        
        _emissionTex = new Texture2D((int)tileCount.x,(int)tileCount.y);
        _emissionTex.wrapMode = TextureWrapMode.Clamp;
        _emissionTex.filterMode = FilterMode.Point;
        _material = _renderer.sharedMaterial;
        
        for (int j = 0; j < tileCount.x; j++)
        {
            for (int i = 0; i < tileCount.y; i++)
            {
                Color c = Color.black;
                _emissionTex.SetPixel(i,j,c);
            }
        }
        
        if (!_material)
            return;
        
        _material.SetTexture(EmissionMap,_emissionTex);
    }

    private void OnDisable()
    {
        if (Application.isPlaying)
        {
            Destroy(_emissionTex);
        }
        else
        {
            DestroyImmediate(_emissionTex);
        }
    }
    
    void Update()
    {
        if (playerList.Count < 1)
            return;

        for (int j = 0; j < tileCount.x; j++)
        {
            for (int i = 0; i < tileCount.y; i++)
            {
                Color c = _emissionTex.GetPixel(i, j);
                _emissionTex.SetPixel(i,j,c*(1-colorTemporalReduce));
            }
        }
        
        foreach (var player in playerList)
        {
            Vector3 pos = player.transform.position;
            Vector2 postion = new Vector2(pos.x,pos.z);
            postion.x = Remap(postion.x, _tileCenter.x + tileSize.x * 0.5f, _tileCenter.x - tileSize.x * 0.5f, 0, 1);
            postion.y = Remap(postion.y, _tileCenter.z + tileSize.y * 0.5f, _tileCenter.z - tileSize.y * 0.5f, 0, 1);
            postion.x *= tileCount.x;
            postion.y *= tileCount.y;
            postion.x = Mathf.Floor(postion.x);
            postion.y = Mathf.Floor(postion.y);
            
            _emissionTex.SetPixel((int)postion.x, (int)postion.y, player.color);
            // Debug.Log(postion);
        }
        
        _emissionTex.Apply();
    }

    float Remap(float input, float inMin, float inMax, float outMin, float outMax)
    {
        float o = outMin + (input - inMin) * (outMax - outMin) / (inMax - inMin);
        return o;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        var b = _bounds;
        Vector3 size = b.size;
        size.y += 0.2f;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(b.center,size);
    }
    
#endif
    
}
