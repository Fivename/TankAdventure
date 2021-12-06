using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LoadStage : MonoBehaviour, IPointerClickHandler
{
    public int id;
    //void Start()
    //{

    //}


    public void OnPointerClick(PointerEventData eventData)
    {
        SceneManager.LoadScene("Scene"+id.ToString());
    }
}
