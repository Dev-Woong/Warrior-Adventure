using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropItem : MonoBehaviour
{
    public static DropItem Instance;
    public GameObject Panel;
    public GameObject RecheckPanel;
    public TMP_InputField tifField;
    private InventorySlot targetSlot; // 버릴 슬롯 정보
    public Text readMe;
    public int a = 2;

    private void Awake()
    {
        Instance = this;
        
    }
    private void Start()
    {
        Panel.SetActive(false);
        RecheckPanel.SetActive(false);
    }
    public void CancelBtnClick()
    {
        this.gameObject.SetActive(false);
    }
    public void Open(InventorySlot slot)
    {
        targetSlot = slot;
        tifField.text = "1"; // 기본값 1
        Panel.SetActive(true);
    }
    public void ReCheckButtonClick()
    {
        targetSlot.RemoveCount(1);
        RecheckPanel.SetActive(false);
        Panel.SetActive(false);
        targetSlot.OOCPanel.SetActive(false);
    }
    public void ReCheckButtonCancel()
    {
        RecheckPanel.SetActive(false);
    }
    public void OnClickConfirm()
    {
        if (int.TryParse(tifField.text, out int amount))
        {
            if (amount < 0)
            {
                readMe.text = "잘못된 수량입니다.\n다시 입력해주세요.";
                tifField.text = "";
            }
           
            else if (amount > 0 && amount >= targetSlot.itemCount)
            {
                amount = targetSlot.itemCount;
                if (targetSlot.itemData.itemType == ItemType.Equipment)
                {
                    RecheckPanel.SetActive(true);
                }
                else 
                { 
                    targetSlot.RemoveCount(amount);
                    Panel.SetActive(false);
                    targetSlot.OOCPanel.SetActive(false);
                }
                readMe.text = "버릴 수량을 입력해주세요.";
            }
            else 
            { 
                targetSlot.RemoveCount(amount);

                readMe.text = "버릴 수량을 입력해주세요.";
                Panel.SetActive(false);
                targetSlot.OOCPanel.SetActive(false);
            }
        }
        
    }
    
}
