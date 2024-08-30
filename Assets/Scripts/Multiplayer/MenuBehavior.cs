using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBehavior : MonoBehaviour
{
    public string menuName;
    public bool isOpen;

    //Open the menu
    public void Open()
    {
        isOpen = true;
        gameObject.SetActive(true);
    }

    //Close the menu
    public void Close()
    {
        isOpen = false;
        gameObject.SetActive(false);
    }
}
