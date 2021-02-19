using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTemplateUtilities : MonoBehaviour
{
    public bool FetchTemplateFromLevelGen = true;
    public LevelTemplate Template;

    private void Start()
    {
        if (FetchTemplateFromLevelGen || Template == null)
        {
            Template = GetComponent<LevelTemplateGenerator>().Template;
        }
    }


    /// <summary>
    /// Flatens <paramref name="_point"/> and checks if the x,z is within the bounds of the tiles in the template 
    /// </summary>
    /// <param name="_point"></param>
    /// <returns>True if the point is within the level</returns>
    public bool CheckPointWithinLevelBounds(Vector3 _point)
    {
        float baseSize = Template.TileBaseSize;
        bool returnVal = false;
        foreach (var item in Template.LevelLayout)
        {
            Vector2 size = new Vector2(baseSize, baseSize);
            Vector2 centrePoint = new Vector2(item.PosIndex.x * (baseSize), item.PosIndex.y * (baseSize));
            Rect CollisionRect = new Rect(centrePoint - (size / 2), size);

            Vector2 pointFlat = new Vector2(_point.x, _point.z);

            returnVal = CollisionRect.Contains(pointFlat);

            if (returnVal) {break;}
        }

        
        return returnVal;
    }


    public Vector3 RandPointWithinLevelBounds()
    {
        Vector3 final = Vector3.zero;

        int randIndex = (int)Random.Range(0, Template.LevelLayout.Count - 1);
        final = RandPointInTile(randIndex);

        return final;
    }

    public Vector3 RandPointInTile(int _tileIndex)
    {
        Vector3 final = Vector3.zero;
        TileInfo tile = Template.LevelLayout[_tileIndex];
        float baseSize = Template.TileBaseSize;
        Vector2 size = new Vector2(baseSize, baseSize);

        Vector2 centrePoint = new Vector2(tile.PosIndex.x * (baseSize), tile.PosIndex.y * (baseSize));
        Vector2 botLeft = centrePoint - (size / 2);
        Vector2 topRight = centrePoint + (size / 2);

        final = new Vector3(Random.Range(botLeft.x, topRight.x), 0, Random.Range(topRight.y, botLeft.y));
        return final;
    }

    private void OnDrawGizmos()
    {
      
    }
}
