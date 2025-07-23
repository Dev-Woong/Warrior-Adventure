using UnityEngine;
using TMPro;

public class ChatManager : MonoBehaviour
{
    public static ChatManager Instance;

    public Transform chatParent;
    public GameObject chatTextPref;
    private void Awake()
    {
        Instance = this;
    }
    public void AddItemMessage(string itemName,int Amount)
    {
        GameObject chatText = Instantiate(chatTextPref, chatParent);
        TMP_Text text = chatText.GetComponent<TMP_Text>();
        text.text = $"{itemName} {Amount}°³ È¹µæ!";
    }
    public void AddUseItemMessage(string itemName, int Amount)
    {
        GameObject chatText = Instantiate(chatTextPref, chatParent);
        TMP_Text text = chatText.GetComponent<TMP_Text>();
        text.text = $"{itemName} {Amount}°³¸¦ ¼Òºñ!";
    }


}
