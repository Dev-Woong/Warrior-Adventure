using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject dialogueUI;
    public Text npcNameText;
    public Text dialogueText;
    public Image npcIllustration;
    public Button nextButton;
    public Button acceptQuestButton;
    public Button negativeButton;

    private NPCData currentNPC;
    
    private int dialogueIndex = 0;
    private int questDialogueIndex = 0;
    public bool isTalking;
    public bool ableAddQuest;
    public int npcFaceType;
    void Awake()
    {
        Instance = this;
        dialogueUI.SetActive(false);
        nextButton.gameObject.SetActive(false);
        acceptQuestButton.gameObject.SetActive(false);
        negativeButton.gameObject.SetActive(false);
    }

    public void StartDialogue(NPCData npc)
    {
        currentNPC = npc;
        dialogueIndex = 0;
        questDialogueIndex = 0;
        npcNameText.text = npc.npcName;
        npcIllustration.sprite = npc.npcIllustration;
        ShowDialogueLine();
        dialogueUI.SetActive(true);
        nextButton.gameObject.SetActive(true);
        
    }

    public void ShowDialogueLine()
    {
        
        //Debug.Log(currentNPC.npcName + "�� ��ȭ��..");

        //����Ʈ �̿Ϸ�� ���â ȣ��.
        if (currentNPC.questToGive.questRooting == true // ����Ʈ �����߰�
            && !currentNPC.questToGive.isComplete // ���� �Ϸ��� ���� �ƴϸ� 
            )
        {
            dialogueText.text = currentNPC.questToGive.notCompliteQuestDialogue;
            nextButton.gameObject.SetActive(false);
            acceptQuestButton.gameObject.SetActive(false);
            negativeButton.gameObject.SetActive(false);
            Invoke("EndDialogue", 1.5f);
        }

        //�⺻ npc ��� ȣ��
        else if (dialogueIndex < currentNPC.dialogueLines.Length) // �⺻ ���â
        {
            dialogueText.text = currentNPC.dialogueLines[dialogueIndex];
            isTalking = true;
            dialogueIndex++;
        }

        //npc ����Ʈ ��� ����
        else if (dialogueIndex == currentNPC.dialogueLines.Length //�⺻ ��������� �����ٸ�
            && questDialogueIndex < currentNPC.questToGive.questDialogue.Length //����Ʈ ��������� �������� �ʾҴٸ�
            && !currentNPC.questToGive.isComplete// ����Ʈ�� �Ϸ������� ���ٸ�
            && QuestManager.Instance.CanActivateQuest(currentNPC.questToGive) // ��������Ʈ ���� �����ϸ� ���â ȣ��
            )
        {
            dialogueText.text = currentNPC.questToGive.questDialogue[questDialogueIndex];
            isTalking = true;
            questDialogueIndex++;
        }

        //npc����Ʈ ����â ȣ��
        else if (currentNPC.questToGive.questRooting == false  // ����Ʈ ������ ����
            && !currentNPC.questToGive.isComplete // ����Ʈ �Ϸ������� ����
            && QuestManager.Instance.CanActivateQuest(currentNPC.questToGive) // ��������Ʈ ���� �����ϸ� ���â ȣ��
            )
        {
            // ��ȭ ������ ����Ʈ ����
            dialogueText.text = $"����Ʈ �̸� : {currentNPC.questToGive.title} \n����Ʈ ���� : {currentNPC.questToGive.description}\n\n�����Ͻðڽ��ϱ�?";
            nextButton.gameObject.SetActive(false);
            acceptQuestButton.gameObject.SetActive(true);
            negativeButton.gameObject.SetActive(true);
        }

        else // ���â ��� ����
        { EndDialogue(); }
        
    }
    public void ShowCompleteDialogue(NPCData npc, QuestInstance quest) // ����Ʈ ���� �Ϸ�� ����Ʈ �Ϸ� ��� ���� �� ���� ����
    {
        dialogueUI.SetActive(true);
        npcNameText.text = npc.npcName;
        dialogueText.text = quest.qData.completeDialogue+"\n\n ���� : "+quest.qData.rewardGold+"���,   " + quest.qData.rewardEXP+"����ġ ȹ��!!" ;
        npc.questToGive.questRooting = false;
        //quest.isRewardGiven = true;
        npc.questToGive.isComplete = true;
        QuestManager.Instance.CompleteQuest(quest);

        Debug.Log("���� ���� �Ϸ�: ��� " + quest.qData.rewardGold);
        Invoke("EndDialogue", 1.5f);
    }
    
    public void OnClickAcceptButton() // ����Ʈ ������ư 
    {
        QuestManager.Instance.TryAddQuest(currentNPC.questToGive);
        currentNPC.questToGive.questRooting = true;
        dialogueText.text = "����Ʈ�� �����߽��ϴ�!";
        acceptQuestButton.gameObject.SetActive(false);
        negativeButton.gameObject.SetActive(false);
        Invoke("EndDialogue", 1.5f);
        isTalking = false;
    }
    public void OnClickExitButton()
    {
        EndDialogue();
    }

    public void EndDialogue() // ���â ����
    {
        dialogueUI.SetActive(false);
        nextButton.gameObject.SetActive(false);
        acceptQuestButton.gameObject.SetActive(false);
        negativeButton.gameObject.SetActive(false);
        isTalking = false;
    }

}