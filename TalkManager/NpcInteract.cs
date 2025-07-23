using TMPro;
using UnityEngine;

public class NPCInteract : MonoBehaviour
{
    public NPCData npcData;
    public GameObject Npc_InteractUI;
    public TMP_Text CheckInteractText;
    public TMP_Text nameText;
   //public string NpcID;
    private bool isPlayerInRange = false;
    GameObject cam;
    private void Start()
    {
        Npc_InteractUI.SetActive(false);
        cam = Camera.main.gameObject;
    }
    void Update()
    {
        FloatingObjName();
        if (DialogueManager.Instance.isTalking == true)
        {
            Npc_InteractUI.SetActive(false);
        }
        
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            var quest = QuestManager.Instance.GetQuestInstance(npcData.questToGive);
            OnTalk();
            if (quest != null && quest.IsCompleted && !quest.isRewardGiven)
            {
                DialogueManager.Instance.ShowCompleteDialogue(npcData, quest);
            }
            else
            {
                DialogueManager.Instance.StartDialogue(npcData);
            }
        }
    }
    public void VisibleNpcUI()
    {
        CheckInteractText.text = $"{npcData.npcName}과 대화하기(F)";
        Npc_InteractUI.SetActive(true);
    }
    public void InVisibleNpcUI()
    {

        CheckInteractText.text = null;
        Npc_InteractUI.SetActive(false);

    }
    void FloatingObjName()
    {
        nameText.text = $"{npcData.npcName}";
        if (cam != null)
        {
            nameText.rectTransform.LookAt(cam.transform);
            nameText.transform.Rotate(0, 180, 0);
            nameText.GetComponent<RectTransform>().position = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
        }
    }
    void OnTalk()
    {
        QuestManager.Instance.UpdateQuestEvent(npcData.NpcID);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        { isPlayerInRange = true; VisibleNpcUI(); }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        { isPlayerInRange = false; Npc_InteractUI.SetActive(false); CheckInteractText.text = ""; }
    }
}
