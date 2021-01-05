using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonController : MonoBehaviour
{
    [Header("Required Fields")]
    public MenuOptions menuOption;

    // Local Variables
    bool keyDown;
    List<MenuButton> buttons = new List<MenuButton>();
    private int maxIndex;
    [HideInInspector] public int index;
    [HideInInspector] public AudioSource audioSource;

    #region Setup
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        foreach(MenuButton n in transform.GetComponentsInChildren<MenuButton>())
        {
            buttons.Add(n);
        }
        foreach (ToggleButton n in transform.GetComponentsInChildren<ToggleButton>())
        {
            n.animatorFunctions.menuButtonController = this;
        }

        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].thisIndex = i;
            buttons[i].menuButtonController = this;
            buttons[i].animatorFunctions.menuButtonController = this;
        }

        index = -1;
        maxIndex = buttons.Count - 1;
    }
    #endregion Setup

    private void Start()
    {
        CanvasController.instance.toggleMenuOption += ToggleMenuOption;
    }

    private void OnDestroy()
    {
        CanvasController.instance.toggleMenuOption -= ToggleMenuOption;
    }

    private void Update()
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            if (!keyDown)
            {
                if (Input.GetAxis("Vertical") < 0)
                {
                    index = (index < maxIndex) ? index + 1 : 0;
                }
                else if (Input.GetAxis("Vertical") > 0)
                {
                    index = (index > 0) ? index - 1 : maxIndex;
                }
                keyDown = true;
            }
        }
        else
        {
            keyDown = false;
        }
    }

    public void ToggleMenuOption(MenuOptions _menuOption)
    {
        if (menuOption == _menuOption)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
