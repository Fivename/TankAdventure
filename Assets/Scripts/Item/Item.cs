using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{


    //public int Id { get; set; }
    public int Id;
    public string Name;
    public ItemType Type;
    public ItemQuality Quality;
    public string Description;
    public int Capacity;
    public int BuyPrice;
    public int SellPrice;
    public string Sprite;


    public Item(int id,string name,ItemType type,ItemQuality quality,string description,int capacity,
        int buyPrice,int sellPrice, string sprite)
    {
        this.Id = id;
        this.Name = name;
        this.Type = type;
        this.Quality = quality;
        this.Description = description;
        this.Capacity = capacity;
        this.BuyPrice = buyPrice;
        this.SellPrice = sellPrice;
        this.Sprite = sprite;
    }
    public enum ItemType
    {
        Weapon,
        Equipment,
        Material,
        Consumable,
    }
    public enum ItemQuality
    {
        Common,
        Unmmon,
        Rare,
        Epic,
        Legendary,
        Artifact
    }

    public virtual string GetToolTipContent()
    {
        return Name;
    }
}
