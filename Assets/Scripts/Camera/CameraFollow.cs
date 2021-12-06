using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothing;
    public Vector2 maxPosition;
    public Vector2 minPosition;
    // Start is called before the first frame update
    void Start()
    {
        //初始化控制器脚本里的静态对象 使其值为：从标签为CameraShake的对象上获取  组件对象 里的 脚本对象
        //调用时则在任意位置使用 GameController.camShake.Shake()调用
        GameController.camShake = GameObject.FindGameObjectWithTag("CameraShake").GetComponent<CameraShake>();
    }

    void LateUpdate()
    {
        if(target != null)
        {
            if(transform.position != target.position)
            {
                Vector3 targetPos = target.position;
                targetPos.x = Mathf.Clamp(targetPos.x, minPosition.x, maxPosition.x);
                targetPos.y = Mathf.Clamp(targetPos.y, minPosition.y, maxPosition.y);
                transform.position = Vector3.Lerp(transform.position, targetPos, smoothing); 
            }
        }
    }
    public void SetCamPosLimit(Vector2 maxPos, Vector2 minPos)
    {
        maxPosition = maxPos;
        minPosition = minPos;
    }
}
