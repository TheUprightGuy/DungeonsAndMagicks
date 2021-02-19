using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDistributor : MonoBehaviour
{
    public List<GameObject> EnemyTypePrefabs;

    public LevelTemplateUtilities LTU;
    
    [Header("Density")]
    public uint EnemiesInLevel = 10;

    [Header("Squads")]
    public uint MinPossibleSquadSize = 1;
    public uint MaxPossibleSquadSize = 4;
    public float SquadTightness = 5.0f;


    private void Awake()
    {
        Distribute();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Distribute()
    {
        int i = 0;
        while ( i < EnemiesInLevel)
        {
            int SquadSize = (int)Random.Range(MinPossibleSquadSize, MaxPossibleSquadSize);

            Vector3 centrePoint = LTU.RandPointWithinLevelBounds();
            int j = 0;
            while( j < SquadSize)
            {
                Vector2 randOffset = Random.insideUnitCircle * SquadTightness;

                Vector3 pos = new Vector3(centrePoint.x + randOffset.x, 0.0f, centrePoint.z + randOffset.y);

                Vector3 rayOrigin = pos;
                rayOrigin.y += 10.0f;

                int layerMask = ~LayerMask.GetMask("NavMesh");
                if (LTU.CheckPointWithinLevelBounds(pos) &&//Within the bounds
                    !Physics.Raycast(rayOrigin, Vector3.down, 10.0f, layerMask)) //HitSomethingother than floor
                {
                    int enemyIndex = Random.Range(0, EnemyTypePrefabs.Count - 1);
                    Instantiate(EnemyTypePrefabs[enemyIndex], pos, Quaternion.identity, this.transform);
                    j++;
                    i++;
                }
            }
        }
    }
}
