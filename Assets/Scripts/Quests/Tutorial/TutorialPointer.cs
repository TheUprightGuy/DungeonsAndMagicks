using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPointer : MonoBehaviour
{
    [HideInInspector] public int id;

    ParticleSystem ps;
    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        TutorialTracking.instance.togglePS += TogglePS;
    }
    private void OnDestroy()
    {
        TutorialTracking.instance.togglePS -= TogglePS;
    }

    public void TogglePS(int _id)
    {
        if (id == _id)
        {
            ps.Play();
        }
        else
        {
            ps.Stop();
        }
    }
}
