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
    // temp
    [HideInInspector] public List<Mod> modsApplied;
    [HideInInspector] public List<RingMenu> startRings;

    public AbilityType type;

    public List<RingMenu> modRings;
    public List<Buff> buffs;

    public abstract void StartUp();

    public abstract void Use(Transform _user);
    public abstract void AddMods(Mod _element);

    public abstract void ResetMods();

    public abstract void ResetRings();

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