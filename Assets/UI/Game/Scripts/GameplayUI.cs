using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    public AbilityUI[] abilityReferences;

    #region SetupIDs
    private void Awake()
    {
        abilityReferences = GetComponentsInChildren<AbilityUI>();
        for (int i = 0; i < abilityReferences.Length; i++)
        {
            abilityReferences[i].abilityID = i;
        }
    }
    #endregion SetupIDs

    private void Start()
    {
        CallbackHandler.instance.showAbility += ShowAbility;
        CallbackHandler.instance.hideAbility += HideAbility;
    }

    private void OnDestroy()
    {
        CallbackHandler.instance.showAbility -= ShowAbility;
        CallbackHandler.instance.hideAbility -= HideAbility;
    }

    public void ShowAbility(int _i)
    {
        abilityReferences[_i].gameObject.SetActive(true);
    }

    public void HideAbility(int _i)
    {
        abilityReferences[_i].gameObject.SetActive(false);
    }
}
