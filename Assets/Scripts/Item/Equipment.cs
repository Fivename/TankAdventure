using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{
    public int Defense { get; set; }
    public float Speed { get; set; }
    public int Hp { get; set; }
    public int Mp { get; set; }
    public EquipmentType EquipType { get; set; }
    public Equipment(int id, string name, ItemType type, ItemQuality quality, string description, int capacity,
        int buyPrice, int sellPrice, string sprite,int defense,int speed,int hp, int mp,EquipmentType equipType)
        : base(id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite)
    {
        this.Defense = defense;
        this.Speed = speed;
        this.Hp = hp;
        this.Mp = mp;
        this.EquipType = equipType;
    }
    public enum EquipmentType
    {
        None,
        Body,
        track
    }
}
