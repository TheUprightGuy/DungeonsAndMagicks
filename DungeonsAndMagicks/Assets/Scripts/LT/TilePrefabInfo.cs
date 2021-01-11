using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileFencedType
{
    NONE,
    TUNNEL,
    CORNER,
    ENDCAP,
    EDGE
}
public class TilePrefabInfo : MonoBehaviour
{
    public TileFencedType FencingType;

    public Vector2Int TileLayoutIndex;
}
