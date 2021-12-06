using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int Blinks;
    public float time;
    public float HitBoxCdTime;
    private Renderer myRendere;
    private Animator anim;
    private PolygonCollider2D polygonCollider2D;
    private PlayerController pc;
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = Player.getInstance();
        health = player.Health;
        pc = GetComponent<PlayerController>();
        HealthBar.HealthMax = health;
        HealthBar.HealthCurrent = health;
        myRendere = GetComponent<Renderer>();
        anim = GetComponent<Animator>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ///player装备提升hp上限同时更新血条
        HealthBar.HealthMax = player.Health;
        health = Mathf.Max(0, health);
        health = Mathf.Min(health, player.Health);
        HealthBar.HealthCurrent = health;
    }
    public void DamagePlayer(int damage,BuffBase buff = null)
    {
        ///player免疫状态跳过damage
        if (pc.IsGodDefend)
        {
            return;
        }
        if (buff != null)
        {
            pc.AddBuff(buff);
        }
        health -= damage;
        //体力不低于0 不高于上限
        health = Mathf.Max(0, health);
        health = Mathf.Min(health, player.Health);

        HealthBar.HealthCurrent = health;
        if(health <= 0)
        {
            AudioManager.PlaySnd("Audio", "Die",gameObject.transform.position, 0.5f);
            anim.SetTrigger("Die");
            ObjectPool.Instance.PushObject(gameObject);
            StopAllCoroutines();
            //Destroy(gameObject,1f);
        }
        else
        {
            if(damage > 0)
            {
                AudioManager.PlaySnd("Audio", "Hit", gameObject.transform.position, 0.5f);
                BlinkPlayer(Blinks, time);
                polygonCollider2D.enabled = false;//碰撞检测地刺
                StartCoroutine(ShowPlayerHitBox());
            }
            else
            {
                //播放回复音效
            }
        }
    }
    IEnumerator ShowPlayerHitBox()
    {
        yield return new WaitForSeconds(HitBoxCdTime);
        polygonCollider2D.enabled = true;
    }


    void BlinkPlayer(int numBlinks,float seconds)
    {
        if(Player.getInstance() != null)
        StartCoroutine(DoBlink(numBlinks, seconds));
    }

    IEnumerator DoBlink(int numBlinks, float seconds)
    {
        if (Player.getInstance() != null)
        {
            for (int i = 0; i < numBlinks * 2; i++)
            {
                myRendere.enabled = !myRendere.enabled;
                yield return new WaitForSeconds(seconds);
            }
            myRendere.enabled = true;
    }
        }   
}
