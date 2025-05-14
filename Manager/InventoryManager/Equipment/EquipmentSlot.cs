using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class EquipmentSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemData equippedItem;
    public Image Icon;
    public Image DefaultIcon;
    public GameObject OOCPanel;
    public Text OkButtonText;
    public int btnType;
    
    private void Start()
    {
        OOCPanel.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (equippedItem != null)
        {

            ToolTipManager.Instance.ShowTooltipPanel(equippedItem, this.transform.position + new Vector3(400, -50, 0));
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipManager.Instance.HideTooltipPanel();
    }
    public void SetSlot(ItemData item)
    {
        equippedItem = item;
        if (item != null)
        {
            Icon.sprite = item.itemIcon;
            Icon.enabled = true;
            Icon.color = new Color(1, 1, 1, 1);
            DefaultIcon.color =  new Color(DefaultIcon.color.r, DefaultIcon.color.g, DefaultIcon.color.b, 0);
        }
        else
        {
            Icon.sprite = null;
            Icon.enabled = false;
            Icon.color = new Color(0,0,0,0);
            DefaultIcon.color = new Color(DefaultIcon.color.r, DefaultIcon.color.g, DefaultIcon.color.b, 1);
        }

    }
    public void ClickSlot()
    {
        OOCPanel.SetActive(true);
        if (equippedItem == null)
        { OkButtonText.text = "¿Â¬¯"; btnType = 1; }
        else { OkButtonText.text = "«ÿ¡¶"; btnType = 2; }
    }
    public void ClickOkBtn()
    {
        if (btnType ==1) 
        {
            InventoryManager.Instance.UIActiveTrue();
        }
        else if (btnType == 2)
        {
            EquipmentManager.Instance.UnEquip(equippedItem.equipType);
        }
        OOCPanel.SetActive(false);
    }
    public void ClickCancelBtn()
    {
        OOCPanel.SetActive(false);
    }
    public ItemData RemoveItem()
    {
        ItemData oldItem = equippedItem;
        SetSlot(null);
        return oldItem;
    }
    
    public bool IsEmpty()
    {
        return equippedItem == null;
    }
}
