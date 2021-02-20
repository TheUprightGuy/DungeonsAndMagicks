using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    #region Singleton
    public static GameplayUI instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one GameplayUI Exists!");
            Destroy(gameObject);
        }

        instance = this;
        abilityReferences = GetComponentsInChildren<AbilityUI>();
        for (int i = 0; i < abilityReferences.Length; i++)
        {
            abilityReferences[i].abilityID = i;
        }
    }
    #endregion Singleton
    #region Callbacks
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
    #endregion Callbacks

    [HideInInspector] public AbilityUI[] abilityReferences;

    public void ShowAbility(int _i)
    {
        abilityReferences[_i].gameObject.SetActive(true);
    }

    public void HideAbility(int _i)
    {
        abilityReferences[_i].gameObject.SetActive(false);
    }
}
