using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunctions : MonoBehaviour
{
    MenuButtonController menuButtonController;

    private void Start()
    {
        menuButtonController = MenuButtonController.instance;
    }

    void PlaySound(AudioClip _sound)
    {
        menuButtonController.audioSource.Stop();
        menuButtonController.audioSource.PlayOneShot(_sound);
    }
}
