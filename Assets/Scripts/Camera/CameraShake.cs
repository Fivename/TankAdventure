using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 用于调用shake动画的类
/// </summary>
public class CameraShake : MonoBehaviour
{
    public Animator camAnim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Shake()
    {
        camAnim.SetTrigger("Shake");
    }
}
