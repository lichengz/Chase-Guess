using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class PlayerInLitTile : MonoBehaviour
{
    public Color color;
    private void OnEnable()
    {
        LitTile.playerList.Add(this);
    }

    private void OnDisable()
    {
        LitTile.playerList.Remove(this);
    }
}
