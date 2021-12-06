using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillImplList
{
    private static SkillImplList _Instance;
    PlayerController playerController;
    PlayerHealth playerHealth;
    Player player;
    public static SkillImplList GetInstance()
    {
        if (_Instance == null)
        {
            _Instance = new SkillImplList();
        }
        return _Instance;
    }
    private SkillImplList()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
        player = Player.getInstance();
    }
    public void UseSkill(Skill sk, GameObject go, Enemy enemy = null, int level = 1)
    {
        if(playerController==null||
            playerHealth==null||
            player == null)
        {
            playerController = GameObject.Find("Player").GetComponent<PlayerController>();
            playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
            player = Player.getInstance();
        }


        switch (sk.SkillName)
        {
            //player skill

            case "Dash":
                
                go.transform.Translate(go.transform.right * (player.RunSpeed + player.skills["Dash"].SkillLevel)*1/2, Space.World);
                break;

            //enemy skill
            case "SelfKill":
                float x = playerController.gameObject.transform.position.x;
                float y = playerController.gameObject.transform.position.y;
                float ex = enemy.transform.position.x;
                float ey = enemy.transform.position.y;
                //如果在半径为level*3 的范围内 player就会受到damage
                if (Mathf.Sqrt((x-ex)*(x-ex) + (y-ey)*(y-ey)) < level*3)
                {
                    playerHealth.DamagePlayer(5);
                    enemy.health = -1;
                }
                break;
           
        }
    }
}
