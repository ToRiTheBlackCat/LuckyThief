using NUnit.Framework;
using System;
using UnityEngine;

[Serializable]
public enum WeightClass
{
    None = 0,
    Light = 1,
    Medium = 2,
    Heavy = 3
}

[Serializable]
public enum ItemType
{
    Normal,
    Throwable,
    Consumable
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MyResource", order = 1)]
public class InventoryItemResource : ScriptableObject
{
    public string itemName;
    public Sprite sprite;
    [SerializeField] private WeightClass weight;
    [SerializeField] private ItemType type;

    public WeightClass Weight => weight;
    public ItemType Type => type;
}

