using UnityEngine;

public enum ItemType 
{
    Equipment,
    Consumable,
    QuestItem,
    ETC
}
public enum EquipType
{
    None,
    Weapon,
    Helmet,
    Armor,
    Belt,
    Gloves,
    Boots
}

[CreateAssetMenu (fileName = "ItemData", menuName = "Item/Item Data")]
public class ItemData: ScriptableObject
{
    [Header("기본 정보")]
    public int id;
    public string itemName;
    public Sprite itemIcon;
    public ItemType itemType;
    public int amount;
    [TextArea]
    public string itemDescription;

    [Header("장비타입")]
    public EquipType equipType;
    [Header("스택")]
    public int maxStack = 99;

    [Header("장비 아이템 옵션")]
    public float equipAtkBonus;
    public float equipDefBonus;
    public float equipHpBonus;
    public float equipMpBonus;

    [Header("소비 아이템 효과")]
    public int hpRecovery;
    public int mpRecovery;
}
