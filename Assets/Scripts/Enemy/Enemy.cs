using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int health;
    public int damage;
    public float flashTime;
    public int[] randomItem;
    public DeathDrop deathDrop;
    public int deathDropId;
    public int rl;
    public int rr;
    public GameObject moneyPrefab;
    protected PlayerController playerController;
    protected PlayerHealth playerHealth;
    protected GameObject player;
    protected SpriteRenderer sr;
    protected Color originalColor;
    public Dictionary<string, Skill> skillDic = new Dictionary<string, Skill>();

    public enum EnemyType { 
        NomalTank,
        EliteTank,
        BlueBoss
    }
    public enum DeathDrop
    {
        Money,
        Weapon,
        Equipment,
        Material,
        Consumable
    }



    // Start is called before the first frame update
    protected virtual void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        if (health <= 0)
        {
            //自爆：　skillimpy.selfkill(gameobject,time);


            //DeathDroping();
            // yield return new WaitForSeconds(0.1f);
            // ObjectPool.Instance.PushObject(this.gameObject);
            //StartCoroutine(Dead());
            DeathDroping();
            //StopAllCoroutines();
            //Destroy(gameObject);
        }
    }



    public virtual void takeDamage(int damage)
    {
        health -= damage;
        FlashColor(flashTime);
        AudioManager.PlaySnd("Audio", "Explosion", this.gameObject.transform.position, 0.5f);
        GameController.camShake.Shake();
    }
    protected virtual void FlashColor(float time)
    {
        sr.color = Color.red;
        Invoke("ResetColor", time);
    }

    protected virtual void ResetColor()
    {
        sr.color = originalColor;
    }
    protected virtual bool Alert(float distance)
    {
        bool flag = false;
        Transform bossPos = this.gameObject.transform;
        Transform playerPos = player.transform;
        //判断直线距离是否小于警戒距离
        if (Mathf.Sqrt(Mathf.Pow(playerPos.position.x - bossPos.position.x, 2)
            + Mathf.Pow(playerPos.position.y - bossPos.position.y, 2)) <= distance)
        {
            flag = true;
        }
        return flag;
    }

    protected virtual void DeathDroping()
    {
        int moneyAmount = UnityEngine.Random.Range(rl, rr+1);
        Debug.Log(moneyAmount);
        //StartCoroutine(InitMoney(moneyAmount));
        InitMoney(moneyAmount);
        ObjectPool.Instance.PushObject(gameObject);
    }

    public void InitMoney(int amount)
    {
        GameObject money =  ObjectPool.Instance.GetObject(moneyPrefab);
        money.transform.position = gameObject.transform.position;
        money.transform.rotation = gameObject.transform.rotation;

        //Rigidbody2D rb = money.GetComponent<Rigidbody2D>();
        //float x = UnityEngine.Random.Range(-3, 3);
        //float y = UnityEngine.Random.Range(1, 5);
        //rb.AddForce(new Vector3(x, y,0), ForceMode2D.Impulse);
        money.transform.GetComponent<Money>().amount = amount;
    }
    public float GetDistance()
    {
        float x = playerController.gameObject.transform.position.x;
        float y = playerController.gameObject.transform.position.y;
        float ex = gameObject.transform.position.x;
        float ey = gameObject.transform.position.y;
        //如果在半径为level*3 的范围内 player就会受到damage
        return Mathf.Sqrt((x - ex) * (x - ex) + (y - ey) * (y - ey));
    }
    /// <summary>
    ///  按照金币 数量 调用产生金币的方法
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    //IEnumerator InitMoney(int amount)
    //{
    //    for (int i = 0; i < amount; i++)
    //    {
    //        InitMoney();
    //       // new WaitForSeconds(1f / amount)
    //    }
    //    yield return null;

    //}
    //IEnumerator Dead()
    //{
    //    DeathDroping();
    //    ObjectPool.Instance.PushObject(this.gameObject);
    //    yield return ne;
    //}

}
