using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonController : MonoBehaviour
{
    // Local Variables
    bool keyDown;
    List<MenuButton> buttons = new List<MenuButton>();
    private int maxIndex;
    [HideInInspector] public int index;
    [HideInInspector] public AudioSource audioSource;

    #region Singleton&Setup
    public static MenuButtonController instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one MenuButtonController exists!");
            Destroy(this.gameObject);
        }

        instance = this;
        audioSource = GetComponent<AudioSource>();
        foreach(MenuButton n in transform.GetComponentsInChildren<MenuButton>())
        {
            buttons.Add(n);
        }

        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].thisIndex = i; 
        }

        index = 0;
        maxIndex = buttons.Count - 1;
    }
    #endregion Singleton&Setup

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
}
