using UnityEngine;

public enum QuestType { Kill, Collect, Talk }

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest/Quest")]
public class QuestData : ScriptableObject
{
    public string questID;
    public string title;
    [TextArea] public string description;
    [TextArea] public string[] questDialogue;
    
    public QuestType questType;

    public string targetID;
    public string targetName;
    public int requiredAmount;

    public QuestData[] prerequisiteQuests;

    public int rewardGold;
    public int rewardEXP;
    public bool questRooting;
    public bool isComplete;
    [TextArea] public string notCompliteQuestDialogue;
    [TextArea] public string completeDialogue;
    
}