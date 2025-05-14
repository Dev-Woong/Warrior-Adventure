using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;
    public EquipmentSlot WeaponSlot;
    public EquipmentSlot ArmorSlot;
    public EquipmentSlot BeltSlot;
    public EquipmentSlot HelmetSlot;
    public EquipmentSlot GlovesSlot;
    public EquipmentSlot BootsSlot;
    public GameObject EquipmentUI;

    public bool isOpenEquip = false;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        EquipmentUI.SetActive(false);
    }
    public void EquipmentUIOpen()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            isOpenEquip = !isOpenEquip;
        }
        EquipmentUI.SetActive(isOpenEquip);
    }
    public void EquipTouchOpen()
    {
        isOpenEquip = !isOpenEquip;
    }
    public void Equip(ItemData newItem)
    {
        if (newItem == null || newItem.itemType != ItemType.Equipment)
            return;
        EquipmentSlot targetslot = GetSlot(newItem.equipType);

        if (targetslot == null )return;

        if (!targetslot.IsEmpty())
        {
            InventoryManager.Instance.AddItem(targetslot.equippedItem,1);
        }
        targetslot.SetSlot(newItem);
        PlayerController.Instance.EquipItemBonusStat(newItem);
    }
    public void UnEquip(EquipType type)
    {
        EquipmentSlot slot = GetSlot(type);

        if (slot == null || slot.IsEmpty()) return;

        PlayerController.Instance.EquipItemMinusStat(slot.equippedItem);
        InventoryManager.Instance.AddItem(slot.equippedItem,1);
        slot.SetSlot(null);
    }
    private EquipmentSlot GetSlot(EquipType type)
    {
        return type switch
        {
            EquipType.Weapon => WeaponSlot,
            EquipType.Armor => ArmorSlot,
            EquipType.Belt=> BeltSlot,
            EquipType.Gloves => GlovesSlot,
            EquipType.Helmet => HelmetSlot,
            EquipType.Boots => BootsSlot,
            _ => null
        };
    }
    private void Update()
    {
        EquipmentUIOpen();
    }
}
    
