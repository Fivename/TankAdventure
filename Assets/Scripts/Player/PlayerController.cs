using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : CostumEntityLogic
{
    public Player player = Player.getInstance();
    public float runSpeed;
    public float jumpSpeed;
    public float doubleJumpSpeed;
    public float fireCd;
    private float fireCdNow;
    private float skillCd;
    private float skillCdNow;
    public GameObject bulletPrefab;
    public GameObject floatPoint;
    private Rigidbody2D myRigidbody;
    private Animator myAnim;
    private BoxCollider2D myFeet;
    private PlayerHealth playerHealth;
    private int health;
    public bool isGround;
    public bool canDoubleJump;
    private Text moneyText;
    private int money;
    private Vector2 direction;

    public int Money
    {
        get { return money; }
        set
        {
            money = value;
            moneyText.text = money.ToString();
        }
    }

    // Start is called before the first frame update
    public void Start()
    {
        //获取所需组件
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        myFeet = GetComponent<BoxCollider2D>();
        playerHealth = GetComponent<PlayerHealth>();
        //初始化角色属性
        runSpeed = player.RunSpeed;
        jumpSpeed = player.JumpSpeed;
        doubleJumpSpeed = player.DoubleJumpSpeed;
        health = playerHealth.health;
        money = player.Money;
        fireCdNow = fireCd;
        skillCd = player.CurSkill.CoolTime;
        skillCdNow = skillCd;
    }

    // Update is called once per frame
    public new void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.G))
        {
            int a = UnityEngine.Random.Range(1, 5);
            KnapsackPanel.Instance.StoreItem(a);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            ChestPanel.Instance.ItemShowAndHide();
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            KnapsackPanel.Instance.ItemShowAndHide();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            CharacterPanel.Instance.ItemShowAndHide();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            ShopPanel.Instance.ItemShowAndHide();
        }
       

    }
    public void FixedUpdate()
    {
        //冰冻
        if (IsFreeze)
        {
            return;
        }
        //Run();
        //Flip();
        //Jump();
        //checkGrounded();
        //Attack();
        if (health > 0)
        {
            checkGrounded();
            Jump();
            Run();
            Flip();

            Aim();
            
            Attack();
            Skill();
        }
        //if(health > 0)
        //{
        //    Skill();
        //}

    }
    void Aim()
    {
        Vector3 pos = Input.mousePosition;
        direction = pos - transform.position;

    }

    void Skill()
    {
        Dash();
    }
    void Dash()
    {
        //Skill skill = new Skill();
        //if (player.skills.ContainsKey(skill.SkillName) && Input.GetButtonDown("Dash"))
        //{
        //    SkillImplList sk = new SkillImplList();
        //    sk.fun(skill, gameObject,null,player);
        //}
        skillCdNow -= Time.fixedDeltaTime;
        if (player.skills.ContainsKey("Dash") && Input.GetButtonDown("Dash"))
        {
            if(skillCdNow <= 0)
            {
                SkillImplList sk =  SkillImplList.GetInstance();
                //int level = player.skills["Dash"].SkillLevel;
                sk.UseSkill(player.CurSkill, gameObject);
                //myAnim.SetBool("Dash", true);
                //this.transform.Translate(transform.right * runSpeed, Space.World);
                //myRigidbody.AddForce(transform.right * runSpeed*30, ForceMode2D.Impulse);

                skillCdNow = skillCd;
            }
        }
    }

    void Attack()
    {
        fireCdNow -= Time.fixedDeltaTime;
        if (Input.GetButtonDown("Attack"))
        {
            if(fireCdNow <= 0)
            {
                //Instantiate(bulletPrefab, transform.position, Quaternion.Euler(transform.eulerAngles));
                //改用对象池
                GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab);
                bullet.transform.position = transform.position;
                bullet.transform.rotation = Quaternion.Euler(transform.eulerAngles);
                bullet.GetComponent<Bullet>().damage += player.Damage;
                AudioManager.PlaySnd("Audio", "Fire",this.gameObject.transform.position,0.5f);

                fireCdNow = fireCd;
            }
        }
    }

    void checkGrounded()
    {
        isGround = myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (isGround)
        {
            canDoubleJump = true;
        }
    }
    /// <summary>
    /// 翻转精灵
    /// </summary>
    void Flip()
    {
        bool playerHasAxisSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasAxisSpeed)
        {
            //不是大于0而是0.1 是为了防止摩擦力等因素导致停止原地不动也会左右晃
            if (myRigidbody.velocity.x > 0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            if (myRigidbody.velocity.x < -0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    void Run()
    {
        float moveDir = Input.GetAxis("Horizontal");
        Vector2 playerVel = new Vector2(moveDir * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVel;
        //水平是否有速度
        bool playerHasAxisSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        //把bool值给Run
        myAnim.SetBool("Run", playerHasAxisSpeed);
    }
    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGround)
            {
                Vector2 playerVel = new Vector2(0.0f, jumpSpeed);
                myRigidbody.velocity = Vector2.up * playerVel;
                canDoubleJump = true;
            }
            else
            {
                if (canDoubleJump)
                {
                    Vector2 doubleJumpVel = new Vector2(0.0f, doubleJumpSpeed);
                    myRigidbody.velocity = Vector2.up * doubleJumpVel;
                    canDoubleJump = false;
                }
            }

        }
    }
    public bool ConsumeMoney(int amount)
    {
        moneyText = GameObject.Find("Money/Text").GetComponent<Text>();
        moneyText.text = money.ToString();
        if (money < amount) return false;
        money -= amount;
        moneyText.text = money.ToString();
        return true;
    }

    /// <summary>
    /// 赚取金钱
    /// </summary>
    public void EarnMoney(int amount)
    {
        moneyText = GameObject.Find("Money/Text").GetComponent<Text>();
        moneyText.text = money.ToString();
        money += amount;
        moneyText.text = money.ToString();
        GameObject Gb = Instantiate(floatPoint, transform.position, Quaternion.identity);
        Gb.GetComponentInChildren<TextMesh>().text = "+" + amount.ToString();
   
    }
}
