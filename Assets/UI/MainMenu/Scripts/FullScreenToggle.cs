using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenToggle : MonoBehaviour
{
    private Toggle toggle;
    private GameSettings settings;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }

    // Start is called before the first frame update
    void Start()
    {
        settings = CanvasController.instance.gameSettings;
        SetSettings();

        CanvasController.instance.setSettings += SetSettings;
    }

    private void OnDestroy()
    {
        CanvasController.instance.setSettings -= SetSettings;
    }

    public void SetSettings()
    {

        toggle.isOn = settings.fullScreen;
    }

    public void ToggleSubtitles()
    {
        settings.fullScreen = toggle.isOn;
    }
}
