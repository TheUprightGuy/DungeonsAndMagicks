using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class LevelTemplateGenerator : MonoBehaviour
{
    public LevelTemplate Template = null;
    private float TileSize = 20.0f;

    
    // Start is called before the first frame update
    void Awake()
    {
        if (Template == null)
        {
            Debug.Log("No template found");
            return;
        }
        if (Template.LevelLayout.Count == 0)
        {
            Debug.Log("No tile layout found");
            return;
        }
        if (Template.LevelTiles.Count == 0)
        {
            Debug.Log("No tile prefabs found");
            return;
        }

        foreach (TileInfo item in Template.LevelLayout)
        {
            SpawnTile(item);
        }

        TileSize = Template.TileBaseSize;
        GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnTile(TileInfo tileInfo)
    {
        
        //Get type of tile needed-
        //TunnelFenced
        //CornerFenced
        //EndCapFenced
        //EdgeFenced
        //NoFenced

        //Get Direction to place forward in

        TileFencedType TypeInfo = TileFencedType.NONE;
        Vector3 dirInfo = Vector3.zero;
        //If left and right available, type is tunnel and forward is Vector3.forward
        if (!tileInfo.EastOpen && !tileInfo.WestOpen && tileInfo.NorthOpen && tileInfo.SouthOpen){ TypeInfo = TileFencedType.TUNNEL; dirInfo = Vector3.forward; }
        //If forward and back available, type is tunnel and forward is Vector3.left
        else if (tileInfo.EastOpen && tileInfo.WestOpen && !tileInfo.NorthOpen && !tileInfo.SouthOpen) { TypeInfo = TileFencedType.TUNNEL; dirInfo = Vector3.left; }


        //If forward and left avilable, type is corner and forward is Vector3.back
        else if(!tileInfo.EastOpen && tileInfo.WestOpen && tileInfo.NorthOpen && !tileInfo.SouthOpen) { TypeInfo = TileFencedType.CORNER; dirInfo = Vector3.forward; }
        //If forward and right avilable, type is corner and forward is Vector3.left
        else if(tileInfo.EastOpen && !tileInfo.WestOpen && tileInfo.NorthOpen && !tileInfo.SouthOpen) { TypeInfo = TileFencedType.CORNER; dirInfo = Vector3.right; }
        //If back and left avilable, type is corner and forward is Vector3.right
        else if(tileInfo.EastOpen && !tileInfo.WestOpen && !tileInfo.NorthOpen && tileInfo.SouthOpen) { TypeInfo = TileFencedType.CORNER; dirInfo = Vector3.back; }
        //If back and right avilable, type is corner and forward is Vector3.forward
        else if(!tileInfo.EastOpen && tileInfo.WestOpen && !tileInfo.NorthOpen && tileInfo.SouthOpen) { TypeInfo = TileFencedType.CORNER; dirInfo = Vector3.left; }

        //If only foward available, type is EndCapFenced and forward is Vector.back
        else if(tileInfo.EastOpen && tileInfo.WestOpen && !tileInfo.NorthOpen && tileInfo.SouthOpen) { TypeInfo = TileFencedType.ENDCAP; dirInfo = Vector3.back; }
        //If only left available, type is EndCapFenced and forward is Vector.right
        else if(!tileInfo.EastOpen && tileInfo.WestOpen && tileInfo.NorthOpen && tileInfo.SouthOpen) { TypeInfo = TileFencedType.ENDCAP; dirInfo = Vector3.right; }
        //If only back available, type is EndCapFenced and forward is Vector.forward
        else if(tileInfo.EastOpen && tileInfo.WestOpen && tileInfo.NorthOpen && !tileInfo.SouthOpen) { TypeInfo = TileFencedType.ENDCAP; dirInfo = Vector3.forward; }
        //If only right available, type is EndCapFenced and forward is Vector.left
        else if(tileInfo.EastOpen && !tileInfo.WestOpen && tileInfo.NorthOpen && tileInfo.SouthOpen) { TypeInfo = TileFencedType.ENDCAP; dirInfo = Vector3.left; }

        //If only foward UNavailable, type is EDGE and forward is Vector.forward
        else if(!tileInfo.EastOpen && !tileInfo.WestOpen && tileInfo.NorthOpen && !tileInfo.SouthOpen) { TypeInfo = TileFencedType.EDGE; dirInfo = Vector3.forward; }
        //If only left UNavailable, type is EDGE and forward is Vector.left
        else if(!tileInfo.EastOpen && tileInfo.WestOpen && !tileInfo.NorthOpen && !tileInfo.SouthOpen) { TypeInfo = TileFencedType.EDGE; dirInfo = Vector3.left; }
        //If only back UNavailable, type is EDGE and forward is Vector.back
        else if(!tileInfo.EastOpen && !tileInfo.WestOpen && !tileInfo.NorthOpen && tileInfo.SouthOpen) { TypeInfo = TileFencedType.EDGE; dirInfo = Vector3.back; }
        //If only right UNavailable, type is EDGE and forward is Vector.right
        else if(tileInfo.EastOpen && !tileInfo.WestOpen && !tileInfo.NorthOpen && !tileInfo.SouthOpen) { TypeInfo = TileFencedType.EDGE; dirInfo = Vector3.right; }

        //If none available, type is None and forward is forward
        else if(tileInfo.EastOpen && tileInfo.WestOpen && tileInfo.NorthOpen && tileInfo.SouthOpen) { TypeInfo = TileFencedType.NONE; dirInfo = Vector3.forward; }

        List<GameObject> possibleTiles = new List<GameObject>();
        foreach (GameObject item in Template.LevelTiles)
        {
            if (item.GetComponent<TilePrefabInfo>().FencingType == TypeInfo)
            {
                possibleTiles.Add(item);
            }
        }

        int iRandIndex = Random.Range(0, possibleTiles.Count);

        GameObject newTile = Instantiate(possibleTiles[iRandIndex]);
        newTile.transform.position = new Vector3(tileInfo.PosIndex.x * TileSize, 0.0f, tileInfo.PosIndex.y * TileSize);
        newTile.transform.forward = dirInfo;
        newTile.GetComponent<TilePrefabInfo>().TileLayoutIndex = tileInfo.PosIndex;
        newTile.transform.parent = transform;
    }
}
