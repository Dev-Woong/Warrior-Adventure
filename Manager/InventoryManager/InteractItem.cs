using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractItem : MonoBehaviour
{
    public static InteractItem Instance;
    public CanvasGroup cg;
    public GameObject InteractPanel;
    public TMP_Text itemInteractText;
    public Image InteractPanelIcon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        InteractPanel.SetActive(false);
    }
    public void ItemInteractPanel(ItemData itemData)
    {
        StopCoroutine(SetFadeOutPanel());
        InteractPanel.SetActive(true);
        cg.alpha = 1;
        InteractPanelIcon.sprite = itemData.itemIcon;
        itemInteractText.text = $"{itemData.itemName} È¹µæ";
        if (itemData.itemType == ItemType.Equipment)
        {
            itemInteractText.color = new Color32(255, 0, 0, 255);
        }
        else { itemInteractText.color = new Color32(255, 255, 255, 255); }
        
        StartCoroutine(SetFadeOutPanel());
    }
    IEnumerator SetFadeOutPanel()
    {
        yield return new WaitForSeconds(2f);
        float fadeDuration = 1;
        float elapsed = 0;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(1,0,elapsed/fadeDuration);
            yield return null;
        }
        InteractPanelIcon.sprite = null;
        itemInteractText.text = "";
        InteractPanel.SetActive(false);
    }
    
}
