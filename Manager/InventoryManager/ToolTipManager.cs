using UnityEngine;
using UnityEngine.UI;
public class ToolTipManager : MonoBehaviour
{
    public static ToolTipManager Instance;
    public GameObject TooltipPanel;
    public Image Icon;
    public Text ItemName;
    public Text ItemDesc;
    public Text ItemEffect;
    private void Awake()
    {
        Instance = this;
        TooltipPanel.SetActive(false);
    }
    public void ShowTooltipPanel(ItemData item,Vector3 Pos)
    {
        TooltipPanel.SetActive(true);
        TooltipPanel.transform.position = Pos;
        Icon.sprite = item.itemIcon;
        ItemName.text = item.itemName;
        ItemDesc.text = item.itemDescription;
        if (item.itemType == ItemType.Equipment)
        {
            ItemEffect.gameObject.SetActive(true);
            ItemEffect.text = $"추가 공격력 : {item.equipAtkBonus} \n추가 방어력 : {item.equipDefBonus}\n추가HP : {item.equipHpBonus}\n추가MP:{item.equipMpBonus}";
        }
        else
        {
            ItemEffect.gameObject.SetActive(false);
        }
    }
    public void HideTooltipPanel()
    {
        TooltipPanel.SetActive(false);
    }
}
