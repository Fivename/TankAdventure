using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPanel : Inventory
{
    private static ItemPanel _instance;
    public static ItemPanel Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("ItemPanel").GetComponent<ItemPanel>();
            }
            return _instance;
        }
    }
}
