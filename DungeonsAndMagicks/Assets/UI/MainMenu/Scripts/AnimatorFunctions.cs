using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunctions : MonoBehaviour
{
    [HideInInspector] public MenuButtonController menuButtonController;

    void PlaySound(AudioClip _sound)
    {
        menuButtonController.audioSource.Stop();
        menuButtonController.audioSource.PlayOneShot(_sound);
    }
}
