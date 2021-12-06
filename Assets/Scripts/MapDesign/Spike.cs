using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public int damage;
    public BuffBase Originbuff;
    public Buff buff;

    private PlayerHealth playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        buff = new Buff(Originbuff.m_BuffKind, Originbuff.m_BuffOverlap, Originbuff.m_BuffShutDownType,
        Originbuff.m_BuffCalculateType, Originbuff.m_Length, Originbuff.m_Num);
    }

    // Update is called once per frame
    void Update()
    {
        
        //Originbuff = buff;
        //buff = new Buff(buffKind, buffOverlap, buffShutDownType,
        //buffCalculateType, length, num);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.GetType().ToString() == "UnityEngine.PolygonCollider2D")
        //if (other.CompareTag("Player") && other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            buff = new Buff(Originbuff.m_BuffKind, Originbuff.m_BuffOverlap, Originbuff.m_BuffShutDownType,
            Originbuff.m_BuffCalculateType, Originbuff.m_Length, Originbuff.m_Num);
            playerHealth.DamagePlayer(damage, buff);
        }
    }
}
