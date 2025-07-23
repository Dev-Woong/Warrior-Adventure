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
        
        //Debug.Log(currentNPC.npcName + "과 대화중..");

        //퀘스트 미완료시 대사창 호출.
        if (currentNPC.questToGive.questRooting == true // 퀘스트 수락했고
            && !currentNPC.questToGive.isComplete // 아직 완료한 상태 아니면 
            )
        {
            dialogueText.text = currentNPC.questToGive.notCompliteQuestDialogue;
            nextButton.gameObject.SetActive(false);
            acceptQuestButton.gameObject.SetActive(false);
            negativeButton.gameObject.SetActive(false);
            Invoke("EndDialogue", 1.5f);
        }

        //기본 npc 대사 호출
        else if (dialogueIndex < currentNPC.dialogueLines.Length) // 기본 대사창
        {
            dialogueText.text = currentNPC.dialogueLines[dialogueIndex];
            isTalking = true;
            dialogueIndex++;
        }

        //npc 퀘스트 대사 진행
        else if (dialogueIndex == currentNPC.dialogueLines.Length //기본 대사진행이 끝났다면
            && questDialogueIndex < currentNPC.questToGive.questDialogue.Length //퀘스트 대사진행이 시작하지 않았다면
            && !currentNPC.questToGive.isComplete// 퀘스트를 완료한적이 없다면
            && QuestManager.Instance.CanActivateQuest(currentNPC.questToGive) // 선행퀘스트 조건 만족하면 대사창 호출
            )
        {
            dialogueText.text = currentNPC.questToGive.questDialogue[questDialogueIndex];
            isTalking = true;
            questDialogueIndex++;
        }

        //npc퀘스트 수락창 호출
        else if (currentNPC.questToGive.questRooting == false  // 퀘스트 받은적 없고
            && !currentNPC.questToGive.isComplete // 퀘스트 완료한적도 없고
            && QuestManager.Instance.CanActivateQuest(currentNPC.questToGive) // 선행퀘스트 조건 만족하면 대사창 호출
            )
        {
            // 대화 끝나면 퀘스트 제시
            dialogueText.text = $"퀘스트 이름 : {currentNPC.questToGive.title} \n퀘스트 내용 : {currentNPC.questToGive.description}\n\n수락하시겠습니까?";
            nextButton.gameObject.SetActive(false);
            acceptQuestButton.gameObject.SetActive(true);
            negativeButton.gameObject.SetActive(true);
        }

        else // 대사창 모두 종료
        { EndDialogue(); }
        
    }
    public void ShowCompleteDialogue(NPCData npc, QuestInstance quest) // 퀘스트 조건 완료시 퀘스트 완료 대사 진행 및 보상 지급
    {
        dialogueUI.SetActive(true);
        npcNameText.text = npc.npcName;
        dialogueText.text = quest.qData.completeDialogue+"\n\n 보상 : "+quest.qData.rewardGold+"골드,   " + quest.qData.rewardEXP+"경험치 획득!!" ;
        npc.questToGive.questRooting = false;
        //quest.isRewardGiven = true;
        npc.questToGive.isComplete = true;
        QuestManager.Instance.CompleteQuest(quest);

        Debug.Log("보상 지급 완료: 골드 " + quest.qData.rewardGold);
        Invoke("EndDialogue", 1.5f);
    }
    
    public void OnClickAcceptButton() // 퀘스트 수락버튼 
    {
        QuestManager.Instance.TryAddQuest(currentNPC.questToGive);
        currentNPC.questToGive.questRooting = true;
        dialogueText.text = "퀘스트를 수락했습니다!";
        acceptQuestButton.gameObject.SetActive(false);
        negativeButton.gameObject.SetActive(false);
        Invoke("EndDialogue", 1.5f);
        isTalking = false;
    }
    public void OnClickExitButton()
    {
        EndDialogue();
    }

    public void EndDialogue() // 대사창 종료
    {
        dialogueUI.SetActive(false);
        nextButton.gameObject.SetActive(false);
        acceptQuestButton.gameObject.SetActive(false);
        negativeButton.gameObject.SetActive(false);
        isTalking = false;
    }

}