using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{ 
    public int Damage { get; set; } 
    public WeaponType WPType { get; set; }
    public Weapon(int id, string name, ItemType type, ItemQuality quality, string description, int capacity,
        int buyPrice, int sellPrice, string sprite,int damage,WeaponType weaponType) 
        :base(id,name,type,quality,description,capacity,buyPrice,sellPrice,sprite)
    {
        this.Damage = damage;
        this.WPType = weaponType;
    }
    public enum WeaponType
    {
        None,
        OffHand,
        MainHand
    }
}
