using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    #region SetupIDs
    private void Awake()
    {
        AbilityUI[] temp = GetComponentsInChildren<AbilityUI>();
        for (int i = 0; i < temp.Length; i++)
        {
            temp[i].abilityID = i;
        }
    }
    #endregion SetupIDs
}
