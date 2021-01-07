using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CallbackHandler : MonoBehaviour
{
    public static CallbackHandler instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one Callback Handler Exists!");
            Destroy(this.gameObject);
        }
        instance = this;
    }

    public Action togglePause;
    public void TogglePause()
    {
        // Temp
        CanvasController.instance.gameObject.SetActive(!CanvasController.instance.gameObject.activeSelf);

        if (togglePause != null)
        {
            togglePause();
        }
    }

    public void QuitToMenu()
    {
        // SceneLoad here
    }

    private void Start()
    {
        TogglePause();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
}
