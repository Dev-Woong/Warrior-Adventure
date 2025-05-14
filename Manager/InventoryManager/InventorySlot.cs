using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;
    public TMP_Text countText;
    public ItemData itemData;
    public int itemCount;
    public GameObject OOCPanel;
    public void SetSlot(ItemData data, int count )
    {
        itemData = data;
        itemCount = count;
        icon.sprite = data.itemIcon;
        icon.color = new Color(1, 1, 1, 1);
        icon.enabled = true;
        UpdateCount();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemData != null)
        {
            
            ToolTipManager.Instance.ShowTooltipPanel(itemData,this.transform.position+new Vector3(-400,-50,0));
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipManager.Instance.HideTooltipPanel();
    }
    public void OnClickSlot()
    {
        if (itemData == null) return;

        OOCPanel.GetComponent<OOCPanel>().SetText(itemData);
        OOCPanel.SetActive(true);
    }
    public void CancelBtnClick()
    {
        if (OOCPanel.activeSelf)
        {
            OOCPanel.SetActive(false);
        }
    }
    public void DropBtnClick()
    {
        if (itemData != null)
        {
            DropItem.Instance.Open(this);
        }
        ToolTipManager.Instance.HideTooltipPanel();
    }
    public void OkBtnClick()
    {
        switch (itemData.itemType)
        {
            case ItemType.Consumable:
                UseConsumable();
                break;
            case ItemType.Equipment:
                EquipmentManager.Instance.Equip(itemData);
                ClearSlot();
                break;
        }
        OOCPanel.SetActive(false);
        ToolTipManager.Instance.HideTooltipPanel();
    }
    public void UseConsumable()
    {
        switch (itemData.id)
        {
            case 1:
                PlayerController.Instance.UsePotion(itemData);
                    break;
            case 2:
                PlayerController.Instance.UsePotion(itemData);
                break;
        }
        ChatManager.Instance.AddUseItemMessage(itemData.itemName, 1);
        RemoveCount(1);
    }
    
    public void AddCount(int count)
    {
        itemCount += count;
        UpdateCount();
    }

    public void RemoveCount(int count)
    {
        itemCount -= count;
        if (itemCount <= 0)
            ClearSlot();
        else
            UpdateCount();
    }

    public void ClearSlot()
    {
        itemData = null;
        itemCount = 0;
        icon.sprite = null;
        icon.color = new Color(1,1,1,0);
        icon.enabled = false;
        countText.text = "";
    }

    private void UpdateCount()
    {
        if (itemData.itemType == ItemType.Equipment || itemCount <= 1)
            countText.text = "";
        else
            countText.text = itemCount.ToString();
    }
    private void Start()
    {
        OOCPanel.SetActive(false);
        SlotEmpty();
    }
    public void HideOOCPanel()
    {
        OOCPanel.SetActive(false);
    }
    private void Update()
    {
        if (InventoryManager.Instance.isOpenInven == false)
        {
            OOCPanel.SetActive(false);
            ToolTipManager.Instance.HideTooltipPanel();
        }
    }
    public void SlotEmpty()
    {
        if (itemData == null)
        {
            
            icon.sprite = null;
            icon.color = new Color(1, 1, 1, 0);
            icon.enabled = false;
            countText.text = "";
        }
    }
}

