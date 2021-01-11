using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class Ability : ScriptableObject
{
    // temp
    [HideInInspector] public List<RingElement> modsApplied;

    public List<RingMenu> modRings;

    public abstract void StartUp();

    public abstract void Use(Transform _user);
    public abstract void AddMods(RingElement _element);

    public abstract void ResetMods();

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
}

public class CoroutineSurrogate : MonoBehaviour
{

}