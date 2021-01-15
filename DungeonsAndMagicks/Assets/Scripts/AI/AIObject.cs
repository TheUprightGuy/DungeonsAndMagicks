using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIObject : MonoBehaviour
{
    public AIType thisType;

    private AIAgent thisAgent;


    // Start is called before the first frame update
    void Start()
    {
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length > 1)
        {
            Debug.LogError("AIObject Start failed, more than one player present");
            return;
        }

        if (players.Length < 1)
        {
            Debug.LogError("AIObject Start failed, no players present");
            return;
        }

        thisAgent = AIController.Instance.RequestAgent(thisType, this.transform, players[0].transform);
    }

    // Update is called once per frame
    void Update()
    {
        thisAgent.Act();
    }
}
