using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [HideInInspector] public GetMouseInWorld mousePos;
    [HideInInspector] public CharacterMovement movement;

    [Header("Player Base Stats")]
    public float movementSpeed;
    [Header("Buffs")]
    public List<Buff> buffs;
    public float movementMod;

    #region Singleton
    public static CharacterStats instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one Character Stats exists!");
            Destroy(this.gameObject);
        }
        instance = this;
        mousePos = GetComponent<GetMouseInWorld>();
        movement = GetComponent<CharacterMovement>();
    }
    #endregion Singleton
    private void Start()
    {
        movementSpeed = movement.PlayerSpeed;
    }

    private void Update()
    {
        // Update Buffs & Debuffs
        for (int i = buffs.Count - 1; i >= 0; i--)
        {
            if (buffs[i].UpdateCount(Time.deltaTime))
            {
                CallbackHandler.instance.RemoveBuffFromUI(buffs[i]);
                buffs.Remove(buffs[i]);
            }
        }
    }
    
    public void AddBuff(Buff _buff)
    {
        // Prune same type of buffs
        for (int i = buffs.Count - 1; i >= 0; i--)
        {
            if (buffs[i].buffType == _buff.buffType)
            {
                CallbackHandler.instance.RemoveBuffFromUI(buffs[i]);
                buffs.Remove(buffs[i]);
            }
        }
        // Apply new buff
        buffs.Add(_buff);
        CallbackHandler.instance.AddBuffToUI(_buff);

        CheckBuffs();
    }

    public void CheckBuffs()
    {
        CheckMSBuffs();
    }

    public void CheckMSBuffs()
    {
        movementMod = 1.0f;
        // Check for MS Buff
        for (int i = buffs.Count - 1; i >= 0; i--)
        {
            if (buffs[i].buffType == BuffType.Movement)
            {
                movementMod = buffs[i].movementModifiers.moveIncrease;
            }
        }
        movement.PlayerSpeed = movementSpeed * movementMod;
    }
}
