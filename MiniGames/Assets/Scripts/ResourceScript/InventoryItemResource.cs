using NUnit.Framework;
using UnityEngine;

public enum WeightClass
{
    None,
    Light,
    Medium,
    Heavy
}

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
    public WeightClass weight { get; private set; }
    public ItemType type { get; private set; }
}

