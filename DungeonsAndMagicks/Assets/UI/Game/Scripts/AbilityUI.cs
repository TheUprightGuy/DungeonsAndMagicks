using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUI : MonoBehaviour
{
    [HideInInspector] public int abilityID;
    [HideInInspector] public Ability ability;
    ModList modList;
    public GameObject modPrefab;

    #region Setup
    private void Awake()
    {
        modList = GetComponentInChildren<ModList>();
    }
    #endregion Setup
    #region Callbacks
    private void Start()
    {
        CallbackHandler.instance.setActiveAbility += SetActiveAbility;
        CallbackHandler.instance.setAbilityReference += SetAbilityReference;
        CallbackHandler.instance.updateAbilityModList += UpdateMods;
    }

    private void OnDestroy()
    {
        CallbackHandler.instance.setActiveAbility -= SetActiveAbility;
        CallbackHandler.instance.setAbilityReference -= SetAbilityReference;
        CallbackHandler.instance.updateAbilityModList -= UpdateMods;
    }
    #endregion Callbacks

    public void SetAbilityReference(int _id, Ability _ability)
    {
        if (abilityID == _id)
        {
            ability = _ability;
        }
    }

    public void SetActiveAbility(int _id)
    {
        this.gameObject.transform.localScale = (abilityID == _id) ? new Vector3(1.2f, 1.2f, 1.0f) : new Vector3(1.0f, 1.0f, 1.0f);      
    }

    public void UpdateMods(Ability _ability)
    {
        if (ability == _ability)
        {
            modList.ClearIcons();
            foreach (Mod n in ability.modsApplied)
            {
                modList.CreateMod(n.icon, modPrefab);
            }
        }
    }
}
