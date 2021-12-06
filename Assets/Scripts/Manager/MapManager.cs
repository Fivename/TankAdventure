using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MapManager:MonoBehaviour
{
    public GameObject mainMenu;
    //void LoadMap(int id)
    //{
    //    SceneManager.LoadScene("Scene" + id.ToString());
    //}
    public void Back()
    {
        UIChange.ChangeUI(gameObject, mainMenu);
        //mainMenu.SetActive(true);
        //gameObject.SetActive(false);
    }
}
