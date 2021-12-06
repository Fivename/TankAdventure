using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
public class Save
{
    //存档id
    public int id;
    //存档时间
    public string date;
    //存档最大到达关卡
    public int stage;

    //player数据
    
    public float runSpeed;
    public float jumpSpeed;
    public float doubleJumpSpeed;
    public int health;
    public int magicPoint;
    public int money;
    public int damage;
    public int defense;
    public int experience;
    public int level;
    public Skill currenSkill;
    public Dictionary<string, Item> items = new Dictionary<string, Item>();
    public Dictionary<string, Skill> skills = new Dictionary<string, Skill>();



    public Save()
    {

    }

    public Save(Player p)
    {
        this.runSpeed = p.RunSpeed;
        this.jumpSpeed = p.JumpSpeed;
        this.doubleJumpSpeed = p.DoubleJumpSpeed;
        this.health = p.Health;
        this.money = p.Money;
        this.damage = p.Damage;
        this.items = p.items;
        this.skills = p.skills;
        this.defense = p.Defens;
        this.magicPoint = p.MagicPoint;
        this.experience = p.Experience;
        this.level = p.Level;
        this.currenSkill = p.CurSkill;
}

    public Save(float runSpeed, float jumpSpeed, float doubleJumpSpeed, int health, int money, 
        int damage, Dictionary<string, Item> items, Dictionary<string, Skill> skills,int defense,int magicPoint,int experience,int level,Skill currenSkill)
    {
        this.runSpeed = runSpeed;
        this.jumpSpeed = jumpSpeed;
        this.doubleJumpSpeed = doubleJumpSpeed;
        this.health = health;
        this.money = money;
        this.damage = damage;
        this.items = items;
        this.skills = skills;
        this.defense = defense;
        this.magicPoint = magicPoint;
        this.experience = experience;
        this.level = level;
        this.currenSkill = currenSkill;
    }

    public Player JsonToPlayer(Player p)
    {
        p.RunSpeed = this.runSpeed;
        p.JumpSpeed = this.jumpSpeed;
        p.DoubleJumpSpeed = this.doubleJumpSpeed;
        p.Health = this.health;
        p.Money = this.money;
        p.Damage = this.damage;
        p.items = this.items;
        p.skills = this.skills;
        p.Defens = this.defense;
        p.MagicPoint = this.magicPoint;
        p.Experience = this.experience;
        p.Level = this.level;
        p.CurSkill = this.currenSkill;
        return p;
    }
}
