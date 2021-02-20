using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element
{
    None = 0,
    Fire,
    Ice,
    Lightning
}

public enum AbilityType
{
    None = 0,
    Projectile,
    Buff,
    Movement
}


public abstract class Ability : ScriptableObject
{
    [HideInInspector] public List<Mod> modsApplied;
    new public string name;
    public AbilityType type;
    public Sprite icon;
    public RingMenu modRing;
    public List<Buff> buffs;
    [HideInInspector] public List<Buff> startingBuffs;
    public int sockets;

    public abstract void Use(Transform _user);
    public abstract void AddMods(Mod _element);

    // Called on Equip
    public abstract void StartUp();
    // Called on End Run / Press Q
    public abstract void ResetMods();

    // Called Through Startup - Sets Elements to Start Elements
    public abstract void ResetStartingModifiers();

    #region Coroutines
    private CoroutineSurrogate ___routiner;
    protected CoroutineSurrogate Routiner => ___routiner != null ? ___routiner : ___routiner = GetCoroutineSurrogate();
    protected Coroutine StartCoroutine(IEnumerator routine)
    {
        return Routiner.StartCoroutine(routine);
    }
    protected void StopCoroutine(Coroutine routine)
    {
        if (routine == null)
        {
            return;
        }

        Routiner.StopCoroutine(routine);
    }
    private CoroutineSurrogate GetCoroutineSurrogate()
    {
        CoroutineSurrogate routiner = new GameObject(nameof(CoroutineSurrogate))
            .AddComponent<CoroutineSurrogate>();
        DontDestroyOnLoad(routiner);
        return routiner;
    }
    #endregion Coroutines
}
public class CoroutineSurrogate : MonoBehaviour
{

}