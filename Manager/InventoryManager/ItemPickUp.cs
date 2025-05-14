using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickUp : PoolAble
{
    public ItemData itemData;
    public TMP_Text itemName;
    public bool isPlayerInRange = false;
    readonly float delTime = 15f;
    Coroutine ItemLifeCoroutine;
    Transform cam;
    private void Start()
    {
        cam = Camera.main.transform;
    }
    private void Update()
    {
        PickUpItem();
        ShowItemName();
    }
    private void OnEnable()
    {
        ItemLifeCoroutine = StartCoroutine(ItemLifeTimeCoroutine());
    }
    private void OnDisable()
    {
        if (ItemLifeCoroutine != null)
        {
            StopCoroutine(ItemLifeCoroutine);
            ItemLifeCoroutine = null;
        }
    }
    IEnumerator ItemLifeTimeCoroutine()
    {
        yield return new WaitForSeconds(delTime);
        if (gameObject.activeSelf)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            isPlayerInRange = false;
            ReleaseObject();
        }
    }
    void ShowItemName()
    {
        itemName.text = $"{itemData.itemName}";
        itemName.rectTransform.LookAt(cam);
        itemName.transform.Rotate(0, 180, 0);
        itemName.GetComponent<RectTransform>().position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
    }
   
    void PickUpItem() 
    {
        if (isPlayerInRange == true )
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (itemData.itemType == ItemType.Equipment)
                {
                    InventoryManager.Instance.AddItem(itemData, itemData.amount);
                    InteractItem.Instance.ItemInteractPanel(itemData);
                    gameObject.GetComponent<Rigidbody>().isKinematic = false;
                    isPlayerInRange = false;
                    ReleaseObject();
                }
                else
                {
                    InventoryManager.Instance.AddItem(itemData, itemData.amount);
                    gameObject.GetComponent<Rigidbody>().isKinematic = false;
                    isPlayerInRange = false;
                    ReleaseObject();
                }
                ChatManager.Instance.AddItemMessage(itemData.itemName, itemData.amount);
            }
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
        if (other.CompareTag("Ground"))
        {

            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}

