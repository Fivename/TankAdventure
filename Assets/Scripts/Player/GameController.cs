using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //创建一个CamereShake静态对象 用于其他脚本使用
    public static CameraShake camShake;
    private float time = 0;
    private Player player = Player.getInstance();
    // Start is called before the first frame update
    public void Awake()
    {
        InitPlayer();
    }
    private void InitPlayer()
    {
        player.Damage = 1;
        player.DoubleJumpSpeed = 5;
        player.JumpSpeed = 7;
        player.RunSpeed = 5;
        player.Health = 40;
        player.Defens = 1;
        player.Level = 1;
        player.Experience = 0;
        Skill sk = new Skill();
        sk.SkillName = "Dash";
        sk.IsActive = true;
        sk.CoolTime = 4;
        player.CurSkill = sk;
        player.AddSkill(sk);
    }
    public void start()
    {
        
    }

    public void Update()
    {
        time += Time.deltaTime;
        //每5分钟存档一次
        if (time > 300f)
        {
            SavePlayer();
            time = 0;
        }
    }
    public void SavePlayer()
    {
        Debug.Log("自动存档...");
    }
}
