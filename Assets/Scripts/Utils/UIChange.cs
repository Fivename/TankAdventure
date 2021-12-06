using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChange
{
    //private GameObject oldUI;
    //private GameObject newUI;
    

    static public void ChangeUI(GameObject oldUI, GameObject newUI)
    {
        newUI.SetActive(true);
        oldUI.SetActive(false);
    }
}
