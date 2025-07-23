using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    
    public List<QuestInstance> activeQuests = new();
    public static event System.Action OnQuestListUpdated;
    public PlayerController player;

    void Awake()
    {
        if (Instance == null) { Instance = this;  }
        else Destroy(gameObject);
    }
    public static void NotifyQuestListUpdated()
    {
        OnQuestListUpdated?.Invoke();
    }
    public void AddQuest(QuestData questData)
    {
        var newQuest = new QuestInstance(questData, player);
        activeQuests.Add(newQuest);
        Debug.Log("����Ʈ ����: " + questData.title);
        OnQuestListUpdated?.Invoke(); // �̺�Ʈ �߽�
    }
    public void CompleteQuest(QuestInstance quest)
    {
        if (activeQuests.Contains(quest))
        {
            quest.Complete();
            activeQuests.Remove(quest); // �Ϸ� �� ����Ʈ���� ����
            Debug.Log("����Ʈ �Ϸ�: " + quest.qData.title);
            OnQuestListUpdated?.Invoke(); // UI ����
        }
    }
    public void TryAddQuest(QuestData questData)
    {
        Debug.Log("TryAddQuest: ");
        if (CanActivateQuest(questData))
        {
            Debug.Log("Addquest : inTryAddQuest Method");
            AddQuest(questData);
        }
        else
        {
            Debug.Log("Can't Addquest : inTryAddQuest Method");
            DialogueManager.Instance.dialogueText.text = "���� ������ �������� �ʾҽ��ϴ�!";
            DialogueManager.Instance.dialogueUI.SetActive(true);
            Invoke("EndDialogueWrapper", 1.5f);
        }
    }
    public bool CanActivateQuest(QuestData questData)
    {
        if (questData.prerequisiteQuests == null || questData.prerequisiteQuests.Length == 0)
            return true;
        foreach (var prereq in questData.prerequisiteQuests)
        {
            if (prereq == null)
            {
                Debug.LogWarning($"����Ʈ '{questData.title}'�� ���� ����Ʈ �� null�� �ֽ��ϴ�.");
                continue; // �Ǵ� return false;
            }
            var instance = GetQuestInstance(prereq);
            if (instance == null || !instance.IsCompleted)
            {

                Debug.Log($"Can't Addquest : {prereq.title}");
                return false;
            }
        }
        return true;
    }
    void EndDialogueWrapper()
    {
        DialogueManager.Instance.EndDialogue();
    }


    public void UpdateQuestEvent(string targetID)
    {
        foreach (var quest in activeQuests)
        {
            if (!quest.IsCompleted )
            {
                quest.handler.OnEvent(targetID);
            }
        }
    }
    public QuestInstance GetQuestInstance(QuestData data)
    {
        return activeQuests.FirstOrDefault(q => q.qData.questID == data.questID);
    }
}