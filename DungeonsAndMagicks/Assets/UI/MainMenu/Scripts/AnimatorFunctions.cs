using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunctions : MonoBehaviour
{
    public void PlaySound(AudioClip _sound)
    {
        CanvasController.instance.audioSource.PlayOneShot(_sound);
    }
}
