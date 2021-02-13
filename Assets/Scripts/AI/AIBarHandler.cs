using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBarHandler : MonoBehaviour
{
    public bool DisableBarOnFullHealth = true;
    
    public GameObject BarPrefab;
    
    public float speed;
    public Vector3 offSet;

    
    private List<GameObject> BarObjects = new List<GameObject>();

    Camera main;
    // Start is called before the first frame update
    void Start()
    {
        main = Camera.main;            
    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (BarObjects.Count > AIController.Instance.AgentTransforms.Count)
        {
            int ATCount = AIController.Instance.AgentTransforms.Count;
            int BOCount = BarObjects.Count;
            for (int i = ATCount; i < BOCount; i++)
            {
                Destroy(BarObjects[i]);
            }

            BarObjects.RemoveRange(ATCount, BOCount - ATCount);
        }


        for (int i = 0; i < AIController.Instance.AgentTransforms.Count; i++)
        {
            AIEnemyStats AIStats = AIController.Instance.AgentTransforms[i].GetComponent<AIEnemyStats>();
            
            if (i+1 > BarObjects.Count)
            {
                BarObjects.Add(InstantiateNewBar());
            }
            
            if (AIStats.MaxHealth == AIStats.Health && DisableBarOnFullHealth)
            {
                BarObjects[i].SetActive(false);
                continue;
            }

            BarObjects[i].SetActive(true);

            Vector3 screenPos = main.WorldToScreenPoint(AIController.Instance.AgentTransforms[i].position + offSet);
            screenPos.z = 0.0f;
            BarObjects[i].GetComponent<RectTransform>().position = screenPos;// Vector3.MoveTowards(BarObjects[i].GetComponent<RectTransform>().position, screenPos , step);


            Vector3 newScale = BarObjects[i].transform.GetChild(0).GetComponent<RectTransform>().localScale;
            newScale.x = AIStats.Health / AIStats.MaxHealth;
            BarObjects[i].transform.GetChild(0).GetComponent<RectTransform>().localScale = newScale;
        }

    }

    GameObject InstantiateNewBar()
    {
        GameObject newBar = Instantiate(BarPrefab, transform);
        return newBar;
    }
}
