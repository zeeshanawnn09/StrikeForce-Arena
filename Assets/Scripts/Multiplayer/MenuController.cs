using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public static MenuController instance;

    public MenuBehavior[] menus;

    // Awake is called before Start method
    void Awake()
    {
        instance = this;
    }

    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menuName)
            {
                OpenMenu(menus[i]);
            }
            else if (menus[i].isOpen)
            {
                CloseMenu(menus[i]);
            }
        }
    }

    public void CloseMenu(MenuBehavior menu)
    {
        menu.Close();
    }

    //If there is any already open menu, this method will close that menu
    public void OpenMenu(MenuBehavior menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].isOpen)
            {
                CloseMenu(menus[i]);
            }
        }

        menu.Open();
    }
}
