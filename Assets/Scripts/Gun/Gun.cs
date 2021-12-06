using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Start is called before the first frame update
    public int normalDamage;
    //[HideInInspector]
    public int damage;
    public float normalMoveSpeed;
    [HideInInspector]
    public float moveSpeed;
    public bool isPlayerWeapon = false;
    public float deletTime;
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        time = deletTime;
    }

    void OnEnable()
    {
        damage = normalDamage;
        moveSpeed = normalMoveSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(transform.right * moveSpeed * Time.fixedDeltaTime, Space.World);
        deletTime -= Time.fixedDeltaTime;
        if (deletTime <= 0)
        {
            ObjectPool.Instance.PushObject(gameObject);
            deletTime = time;
            //Destroy(gameObject);
        }
    }
    /* private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && isPlayerBullet == true)
        {
            collision.GetComponent<Enemy>().takeDamage(damage);
            Destroy(gameObject);
        }
    }*/
    protected void OnTriggerEnter2D(Collider2D other)
    {
        //子弹碰到东西了 碰到了啥？
        switch (other.tag)
        {
            case "Player":
                if (!isPlayerWeapon && other.GetType().ToString() == "UnityEngine.CapsuleCollider2D" && other.isActiveAndEnabled)
                    //非玩家的武器就伤害玩家
                {
                    other.GetComponent<PlayerHealth>().DamagePlayer(damage);
                    ObjectPool.Instance.PushObject(gameObject);
                    //Destroy(gameObject);
                }
                break;
            case "Enemy":
                if (isPlayerWeapon && other.isActiveAndEnabled)
                    //非敌人武器伤害敌人
                {
                    other.GetComponent<EnemyStragglers>().takeDamage(damage);
                    ObjectPool.Instance.PushObject(gameObject);
                    //Destroy(gameObject);
                }
                break;
            case "Boss":
                if (isPlayerWeapon && other.isActiveAndEnabled)
                {
                    other.GetComponent<EnemyBoss>().takeDamage(damage);
                    ObjectPool.Instance.PushObject(gameObject);
                    //Destroy(gameObject);
                }
                break;
            case "Ground":
                    ObjectPool.Instance.PushObject(gameObject);
                break;
            default:
                break;
        }
    }
}
