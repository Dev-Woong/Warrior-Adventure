using UnityEngine;
using UnityEngine.UI;
public class OOCPanel : MonoBehaviour
{
    public Text OkPanelText;
    public void CancelSlot()
    {
        if (gameObject.activeSelf)
        {
            this.gameObject.SetActive(false);
        }
    }

    



    public void SetText(ItemData item)
    {
        if (item.itemType == ItemType.Equipment)
        {
            OkPanelText.text = "ÀåÂø";
        }
        else if (item.itemType == ItemType.Consumable)
        { OkPanelText.text = "»ç¿ë"; }
    }

}
