using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStragglers : Enemy
{
    public float moveSpeed;
    public float jumpSpeed;
    public float doubleJumpSpeed;
    public int rank = 2;
    public EnemyType enemyType;
    public GameObject bulletPrefab;
    private Vector3 bulletEulerAngles;
    //private new GameObject player;
    private Rigidbody2D TankRigidbody;
    private Animator TankAnim;
    public float waitTime;
    public float startWaitTime;
    public float patrolTime;
    private float stayTime;
    private Vector2 tankVel;
    private bool isGround;
    private bool canDoubleJump;
    private SkillImplList skImpl;
    public Skill sk;
    // Start is called before the first frame update

    public new void Start()
    {
        //之后可以考虑吧可选择的模式分离的更细
        //比如说 飞机、直升机 和坦克、战车的移动模式不一样  就给移动的模式分类
        //有的怪 残暴的蝙蝠 和普通蝙蝠 除了AI的玩家寻找的模式不一样 一个是保守搜索 一个是突击搜索  其他属性一样
        //在这种时候再给每个行动都分类就显得很多余 所以我觉得给 更细 的不同模式分类就好 ----之后的改善方向。
        switch (enemyType)
        {
            case EnemyType.NomalTank:
                base.Start();
                health = 10;
                waitTime = startWaitTime;
                patrolTime = 2;
                //测试技能
                skImpl = SkillImplList.GetInstance();
                sk = new Skill();
                sk.SkillName = "SelfKill";
                TankRigidbody = GetComponent<Rigidbody2D>();
                TankAnim = GetComponent<Animator>();
                break;
        }
    }

    public new void FixedUpdate()
    {
        switch (enemyType)
        {
            case EnemyType.NomalTank:
                base.FixedUpdate();
                player = GameObject.Find("Player");
                if (player != null)
                {
                    if (Alert(5f))
                    {
                        Move();
                    }
                    else
                    {
                        Patrol();
                    }
                    if (GetDistance() < rank)
                    {
                        stayTime += Time.deltaTime;
                        if (stayTime >= 1)
                        {
                            skImpl.UseSkill(sk, this.gameObject, this);
                        }
                    }
                }
                break;
        }

    }
    public void Update()
    {
        switch (enemyType)
        {
            case EnemyType.NomalTank:
               
                Flip();
                checkGrounded();
                break;
        }
    }

    void Flip()
    {
        bool playerHasAxisSpeed = Mathf.Abs(TankRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasAxisSpeed)
        {
            //不是大于0而是0.1 是为了防止摩擦力等因素导致停止原地不动也会左右晃
            if (TankRigidbody.velocity.x > 0.01f)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            if (TankRigidbody.velocity.x < -0.01f)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
    void Attack()
    {
        switch (enemyType) 
        {
            case EnemyType.NomalTank:
                //Instantiate(bulletPrefab, transform.position, Quaternion.Euler(transform.eulerAngles + bulletEulerAngles));
                GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab);
                bullet.transform.position = transform.position;
                bullet.transform.rotation = Quaternion.Euler(transform.eulerAngles);
                break;
        }
    }
    void checkGrounded()
    {
        isGround = TankRigidbody.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    /// <summary>
    /// 敌人坦克的寻路，跟随逻辑。
    /// </summary>
    void Move()
    {
        Transform tankPos = gameObject.transform;
        Transform playerPos = player.transform;
        if (tankPos.position.x - playerPos.position.x > 0.1)
        {
            TankAnim.SetBool("right", false);
            tankVel = new Vector2(-1f * moveSpeed, TankRigidbody.velocity.y);
            TankRigidbody.velocity = tankVel;
        }
        else if(playerPos.position.x - tankPos.position.x > 0.1)
        {
            TankAnim.SetBool("right", true);
            tankVel = new Vector2(1f * moveSpeed, TankRigidbody.velocity.y);
            TankRigidbody.velocity = tankVel;
        }
        //test
        #region
        //if (TankPos.position.y-PlayerPos.position.y > 0.1)
        //{
        //    tankVel = new Vector2(TankRigidbody.velocity.x, -1f * moveSpeed);
        //    TankRigidbody.velocity = tankVel;
        //}
        //else if(PlayerPos.position.y - TankPos.position.y > 0.1)
        //{
        //    tankVel = new Vector2(TankRigidbody.velocity.x, 1f * moveSpeed);
        //    TankRigidbody.velocity = tankVel;
        //}
        #endregion

        //跳跃
        if (playerPos.position.y - tankPos.position.y > 0.2f)
        {
            if (isGround)
            {
                //tankVel = new Vector2(TankRigidbody.velocity.x, -1f * moveSpeed);
                //TankRigidbody.velocity = tankVel;
                Vector2 tankVel = new Vector2(0.0f, jumpSpeed);
                TankRigidbody.velocity = Vector2.up * tankVel;
                canDoubleJump = true;
            }
            else
            {
                if (canDoubleJump)
                {
                    //tankVel = new Vector2(TankRigidbody.velocity.x, 1f * moveSpeed);
                    //TankRigidbody.velocity = tankVel;
                    Vector2 doubleJumpVel = new Vector2(0.0f, doubleJumpSpeed);
                    TankRigidbody.velocity = Vector2.up * doubleJumpVel;
                    canDoubleJump = false;
                }
            }
        }

        if(waitTime <= 0)
        {
            if (Mathf.Abs(tankPos.position.y - playerPos.position.y) < 1f)
            {
                Attack();
                waitTime = startWaitTime;
            }
        }
        else
        {
                waitTime -= Time.deltaTime;
        }
    }
    //巡逻
    void Patrol()
    {
        if (patrolTime>=1)
        {
            TankAnim.SetBool("right", false);
            tankVel = new Vector2(-1f * moveSpeed, TankRigidbody.velocity.y);
            TankRigidbody.velocity = tankVel;
            patrolTime -= Time.deltaTime;
        }
        else
        {
            TankAnim.SetBool("right", true);
            tankVel = new Vector2(1f * moveSpeed, TankRigidbody.velocity.y);
            TankRigidbody.velocity = tankVel;
            patrolTime -= Time.deltaTime;
            if(patrolTime <= 0)
            {
                patrolTime = 2f;
            }
        }
    }

}
