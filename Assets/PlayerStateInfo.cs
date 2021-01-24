using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStateInfo", menuName = "Data/PlayerStateInfo", order = 1)]
public class PlayerStateInfo : ScriptableObject
{
    public bool control;
    public bool alive;

    public bool Control()
    {
        return (alive && control);
    }

    public void SetControl(bool _toggle)
    {
        control = _toggle;
    }
}
