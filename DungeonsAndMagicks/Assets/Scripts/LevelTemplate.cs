using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct TileInfo
{
    public Vector2Int PosIndex;
    public bool NorthOpen, EastOpen, SouthOpen, WestOpen;
}

[CreateAssetMenu(fileName = "LevelTemplate", menuName = "Levels/Template", order = 1)]
public class LevelTemplate : ScriptableObject
{
    public List<TileInfo> LevelLayout = new List<TileInfo>();
    public List<GameObject> LevelTiles = new List<GameObject>();
}