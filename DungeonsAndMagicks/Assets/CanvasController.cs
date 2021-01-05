using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum MenuOptions
{
    Main,
    NewGame,
    Options,
    Game,
    Audio,
    Video,
    Controls,
    Back,
    Quit
}

public class CanvasController : MonoBehaviour
{
    #region Singleton
    public static CanvasController instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one CanvasController exists!");
            Destroy(this.gameObject);
        }
        instance = this;
    }
    #endregion Singleton

    public MenuOptions menu = MenuOptions.Main;
    public MenuOptions prevMenu;

    private void Start()
    {
        prevMenu = menu;
        Invoke("StartUpFunc", 0.01f);
    }

    private void StartUpFunc()
    {
        ToggleMenuOption(menu);
    }

    public Action<MenuOptions> toggleMenuOption;
    public void ToggleMenuOption(MenuOptions _option)
    {
        switch (_option)
        {
            case MenuOptions.Quit:
            {
                Application.Quit();
                break;
            }
            case MenuOptions.Back:
            {
                if (toggleMenuOption != null)
                {
                    menu = prevMenu;
                    prevMenu = MenuOptions.Main;
                    toggleMenuOption(menu);
                }
                break;
            }
            default:
            {
                if (toggleMenuOption != null)
                {
                    prevMenu = menu;
                    menu = _option;
                    toggleMenuOption(menu);
                }
                break;
            }
        }
    }
}
