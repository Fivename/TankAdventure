using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : Enemy
{
    //private  GameObject player;
    public GameObject BossBulletPrefab;
    public EnemyType enemyType;
    private LineRenderer laser;
    public Transform midPos;
    public Transform leftPos;
    public Transform rightPos;
    //private static bool playerIsNear;
    public float waitTime;
    public float preTime;
    public float contTime;
    public float aimTime;
    public float intervalTime;
    public float laserDamageInterval;
    
    public int bulletNum;
    public float bulletBornSecond;
    public Vector2 direction;
    private Vector2 moveDirection;
    public RaycastHit2D hit2d;
    public Vector2 maxPos;
    public Vector2 minPos;
    public Vector3 aimPos;
    private bool isArrive;
    private float it;
    private float wt;
    private float pt;
    private float ct;
    private float at;
    // Start is called before the first frame update
    new void Start()
    {
        switch (enemyType)
        {
            case EnemyType.BlueBoss:
                midPos = transform.Find("mid");
                leftPos = transform.Find("left");
                rightPos = transform.Find("right");
                laser = midPos.GetComponent<LineRenderer>();
                health = 50;
                base.Start();
                wt = waitTime;
                pt = preTime;
                ct = contTime;
                at = aimTime;
                it = intervalTime;
                player = GameObject.Find("Player");
                break;
            default: break;
        }
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        switch (enemyType)
        {
            case EnemyType.BlueBoss:
                base.FixedUpdate();
                Move();
                break;
            default:break;
        }
       
    }
    private void Update()
    {
        //是否到达目标地点
        isArrive = IsArrive();
        switch (enemyType)
        {
            case EnemyType.BlueBoss:
                if (player != null && Alert(20f))
                {
                    Attack();
                }
                break;
            default: break;
        }





        //如果没到达但是要撞东西了认为到达了 -暂不实现
        //isArrive = true
        //if (isArrive)
        //{
        //    direction = MoveDirection(minPos, maxPos);
        //}
    }

    void Move()
    {
        moveDirection = MoveDirection(minPos, maxPos);
        

    }

    /// <summary>
    /// 返回方向
    /// </summary>
    /// <param name="minPosition"></param>
    /// <param name="maxPosition"></param>
    /// <returns></returns>
    Vector3 MoveDirection(Vector2 minPosition,Vector2 maxPosition)
    {
        aimPos = new Vector2(Random.Range(minPos.x, maxPos.x), Random.Range(minPos.y, maxPos.y));
        return aimPos - transform.position;
    }
    bool IsArrive()
    {
        return Vector2.Distance(transform.position, aimPos) < 1;
    }
    void Attack()
    {
        switch (enemyType)
        {
            case EnemyType.BlueBoss:
                if (it <= 0)
                {
                    it = intervalTime;
                    StartCoroutine(RandonRain(bulletNum, bulletBornSecond));
                }
                else
                {
                    it -= Time.deltaTime;
                }
                //瞄准时间到了 准备射击
                if (at <= 0)
                {
                    //准备时间到了 射击
                    if (pt <= 0)
                    {
                        //shoot
                        laser.enabled = true;
                        Shoot();
                        //持续射击结束
                        if (ct <= 0)
                        {
                            
                            laser.enabled = false;
                            //等待waitTime后让所有时间复原
                            //如果休息完成
                            if (wt <= 0)
                            {
                                wt = waitTime;
                                pt = preTime;
                                ct = contTime;
                                at = aimTime;
                            }
                            else
                            {
                                //Debug.Log("Waiting");
                                wt -= Time.deltaTime;
                            }
                        }
                        //持续射击中
                        else
                        {
                            //Debug.Log("On Shooting!!");
                            ct -= Time.deltaTime;
                        }
                    }
                    //准备时间没到 继续准备
                    else
                    {
                        ShowPreLaser();
                        AimPlayer();
                        //Debug.Log("Preparing!!!!");
                        pt -= Time.deltaTime;
                    }
                }
                //瞄准时间未到 继续瞄准
                else
                {
                    //Debug.Log("Aiming!!!");
                    at -= Time.deltaTime;
                    AimPlayer();
                }
                break;
        }
    }
    /// <summary>
    /// 在laser蓄力的时候 有激光蓄力的感觉
    /// </summary>
    void ShowPreLaser()
    {

    }

    //计算两点角度
    float Angle_360(Vector2 from,Vector2 to)
    {
        float x = from.x - to.x;
        float y = from.y - to.y;
        float hypotenuse = Mathf.Sqrt(Mathf.Pow(x, 2f) + Mathf.Pow(y, 2f));

        //求出弧度
        float cos = x / hypotenuse;
        float radian = Mathf.Acos(cos);

        //用弧度算出角度    
        float angle = 180 / (Mathf.PI / radian);

        if (y < 0)
        {
            angle = -angle;
        }
        else if ((y == 0) && (x < 0))
        {
            angle = 180;
        }
        return angle;
    }
    //让boss转向player
    void AimPlayer()
    {
        switch (enemyType)
        {
            case EnemyType.BlueBoss:
                Transform playerPos = player.transform;
                Transform bossPos = gameObject.transform;
                float angle = Angle_360(bossPos.position, playerPos.position);
                //让boss的炮口转向player
                //gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle - 90);
                Quaternion targetRotation = Quaternion.Euler(0.0f, 0.0f, angle - 90);
                gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2);
                //瞄准到player 然后固定方向
                float x = midPos.position.x - player.transform.position.x;
                float y = midPos.position.y - player.transform.position.y;
                direction = new Vector2(x, y);
                break;
        }
    }
    #region
    //判断boss是否被玩家惊扰
    //void Alert(float distance)
    //{
    //    Transform bossPos = gameObject.transform;
    //    Transform playerPos = player.transform;
    //    //判断直线距离是否小于警戒距离
    //    if (Mathf.Sqrt(Mathf.Pow(playerPos.position.x - bossPos.position.x, 2)
    //        + Mathf.Pow(playerPos.position.y - bossPos.position.y, 2)) <= distance)
    //    {
    //        playerIsNear = true;
    //    }
    //}
    #endregion
    void Shoot()
    {
        switch (enemyType)
        {
            case EnemyType.BlueBoss:
                BulletRain();
                LaserShot();
                break;
        }

    }
    //生成激光
    void LaserShot()
    {
        
        //boss和玩家的角度    
        //direction = new Vector2(x * 0.85f, y * 0.85f);
        //direction = new Vector2(x , y );
        //if(at > 0)
        //{
        //    float x = midPos.position.x - player.transform.position.x;
        //    float y = midPos.position.y - player.transform.position.y;
        //    direction = new Vector2(x, y); 
        //}
        
        //hit2d = Physics2D.Raycast(midPos.position, -direction, 30);
        //从炮口 以之前瞄准的方向发射
        hit2d = Physics2D.Raycast(midPos.position, -direction, 300, ~(1 << 10));
        if(!hit2d)
        {
            Debug.LogError("null hit");
        }
        laser.SetPosition(0, midPos.position);
        laser.SetPosition(1, hit2d.point);
        Debug.Log(hit2d.point);
        
        //Debug.Log(hit2d.transform.position);
        //Debug.Log(player.transform.position);
        if (Vector2.Distance(hit2d.point,player.transform.position) <= 0.1)
        {
            if(laserDamageInterval <= 0)
            {
                playerHealth.DamagePlayer(2);
                laserDamageInterval = 0.25f;
            }
            else
            {
                laserDamageInterval -= Time.deltaTime;
            }
        }

        //laser.startWidth = 5;
        //laser.endWidth = 3;
    }
    void BulletRain()
    {
        Vector2 l = new Vector2(leftPos.position.x,leftPos.position.y);
        Vector2 r = new Vector2(rightPos.position.x,rightPos.position.y);
        Vector3 Aim = new Vector3(0, 0, 90 + Random.Range(-90,90));
        GameObject BossBulletl = ObjectPool.Instance.GetObject(BossBulletPrefab);
        BossBulletl.transform.position = l;
        BossBulletl.transform.rotation = Quaternion.Euler(transform.eulerAngles - Aim);
        GameObject BossBulletr = ObjectPool.Instance.GetObject(BossBulletPrefab);
        BossBulletr.transform.position = r;
        BossBulletr.transform.rotation = Quaternion.Euler(transform.eulerAngles - Aim);
    }

    IEnumerator RandonRain(int bulletNum, float seconds)
    {
        for (int i = 0; i < bulletNum; i++)
        {
            yield return new WaitForSeconds(seconds);
            BulletRain();
        }
    }
}
