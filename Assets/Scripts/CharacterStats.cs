using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [HideInInspector] public GetMouseInWorld mousePos;
    [HideInInspector] public CharacterMovement movement;

    Animator animator;

    [Header("Player Base Stats")]
    public float movementSpeed;
    public float castSpeed = 1.0f;
    [Header("Buffs")]
    public List<Buff> buffs;
    public float movementMod;

    #region Singleton
    public static CharacterStats instance;
    public PlayerStateInfo playerInfo;
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
        animator = GetComponentInChildren<Animator>();
    }
    #endregion Singleton

    public bool Control()
    {
        return (playerInfo.Control());
    }

    public void SetControl(bool _toggle)
    {
        playerInfo.SetControl(_toggle);
    }

    private void Start()
    {
        movementSpeed = movement.PlayerSpeed;
        CallbackHandler.instance.setCastSpeed += SetCastSpeed;
    }
    private void OnDestroy()
    {
        CallbackHandler.instance.setCastSpeed -= SetCastSpeed;
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
                CheckBuffs();
            }
        }

        UpdateAnimations();
    }

    public void UpdateAnimations()
    {
        animator.SetBool("Moving", (movement.currentMovMag > 0));
        animator.SetFloat("Forward", movement.backwards);
        animator.SetFloat("Side", movement.strafing);
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

    public void SetCastSpeed(float _speed)
    {
        animator.SetFloat("CastSpeed", _speed);
    }
}
