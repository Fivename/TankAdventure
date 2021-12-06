using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    public int amount;
    private GameObject playerController;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    private void Awake()
    {
        System.Console.Read();
        playerController = GameObject.FindGameObjectWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        float x = UnityEngine.Random.Range(-3, 3);
        float y = UnityEngine.Random.Range(1, 5);
        rb.AddForce(new Vector3(x, y, 0), ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //碰到玩家就被拾取
        if(other.tag == "Player" && other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            other.GetComponent<PlayerController>().EarnMoney(amount);

            ObjectPool.Instance.PushObject(gameObject);
        }
    }
    public void PlayerGainMoneyUI(int amount)
    {

    }

}
