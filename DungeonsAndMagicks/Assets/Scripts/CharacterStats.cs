using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [HideInInspector] public GetMouseInWorld mousePos;
    [HideInInspector] public CharacterMovement movement;

    public float movementSpeed;
    public float movementMod;
    public float movementDuration;

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
        // TEMP - WILL REPLACE WITH BUFF SYSTEM
        if (movementDuration > 0)
        {
            movementDuration -= Time.deltaTime;
        }
        else
        {
            ResetMovementSpeed();
        }
    }

    public void UpdateMovementSpeed(float _speed, float _duration)
    {
        // temp
        movementMod = _speed;
        movementDuration = _duration;
        movement.PlayerSpeed = movementSpeed * movementMod;
    }

    public void ResetMovementSpeed()
    {
        // temp
        movement.PlayerSpeed = movementSpeed;
        movementMod = 1;
    }
}
