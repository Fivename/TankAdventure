using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player
{
    private static Player sPlayer;
    private float runSpeed;
    private float jumpSpeed;
    private float doubleJumpSpeed;
    private int health;
    private int magicPoint;
    private int money = 10;
    private int damage;
    private int defens;
    private int experience;
    private int level;
   // private Text moneyText;
    private Skill curSkill;
    public Dictionary<string, Item> items = new Dictionary<string, Item>();
    public Dictionary<string, Skill> skills = new Dictionary<string, Skill>();
    private Player()
    {

    }

    public float RunSpeed { get => runSpeed; set => runSpeed = value; }
    public float JumpSpeed { get => jumpSpeed; set => jumpSpeed = value; }
    public float DoubleJumpSpeed { get => doubleJumpSpeed; set => doubleJumpSpeed = value; }
    public int Health { get => health; set => health = value; }
    public int Money { get => money; set => money = value;}
    public int Damage { get => damage; set => damage = value; }
    public int Defens { get => defens; set => defens = value; }
    public int MagicPoint { get => magicPoint; set => magicPoint = value; }
    public int Experience { get => experience; set => experience = value; }
    public int Level { get => level; set => level = value; }
    public Skill CurSkill { get => curSkill; set => curSkill = value; }

    public void AddItem(Item item)
      {
          if (sPlayer.items != null && sPlayer.items.ContainsKey(item.Name))
          {
              sPlayer.items[item.Name] = item;
          }
          else
          {
              sPlayer.items.Add(item.Name, item);
          }
      }
      public void RemoveItem(Item item)
      {
          if (sPlayer.items != null && sPlayer.items.ContainsKey(item.Name))
          {
              sPlayer.items.Remove(item.Name);
          }
          else
          {
              return;
          }
      }


    public void AddSkill(Skill skill)
    {
        if (sPlayer.skills != null && sPlayer.skills.ContainsKey(skill.SkillName))
        {
            sPlayer.skills[skill.SkillName] = skill;
        }
        else
        {
            sPlayer.skills.Add(skill.SkillName, skill);

        }
    }
    public void RemoveSkill(Skill skill)
    {
        if (sPlayer.skills != null && sPlayer.skills.ContainsKey(skill.SkillName))
        {
            sPlayer.skills.Remove(skill.SkillName);
        }
        else
        {
            return;
        }
    }

    public bool ConsumeMoney(int amount)
    {
        if (amount > money) return false;
        money -= amount;
       // moneyText.text = money.ToString();

        return true;
    }

    public void EarnMoney(int amount)
    {
        money += amount;
       // moneyText.text = money.ToString();
    }

    public static Player getInstance()
    {
        if (sPlayer == null)
        {
            sPlayer = new Player();
        }
        return sPlayer;
    }
}
