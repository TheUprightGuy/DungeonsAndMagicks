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
    public bool PointWithinLevelBounds(Vector3 _point)
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

    

    private void OnDrawGizmos()
    {
      
    }
}
