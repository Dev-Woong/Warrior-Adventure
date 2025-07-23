using UnityEngine;
using static QuestTypeHandler;
//using static PlayerController;
public class QuestInstance
{
    public QuestData qData;
    private PlayerController pc;
    public int currentAmount; 
    public bool isRewardGiven = false;
    public IQuestLogicHandler handler;
    public bool IsCompleted => currentAmount >= qData.requiredAmount;

    public QuestInstance(QuestData questData,PlayerController player)
    {
        qData = questData;
        currentAmount = 0;
        pc = player;
        handler = CreateHandlerByType(qData.questType);
        handler.Init(this);
    }
    private IQuestLogicHandler CreateHandlerByType(QuestType type)
    {
        return type switch
        {
            QuestType.Kill => new KillQuestHandler(),
            QuestType.Collect => new CollectQuestHandler(),
            QuestType.Talk => new TalkQuestHandler(),
            _ => null
        };
    }
    public void Complete()
    {
        currentAmount = qData.requiredAmount;
        
        pc.curExp += qData.rewardEXP;
        isRewardGiven = true;
        Debug.Log($"Äù½ºÆ® ¿Ï·á Ã³¸®µÊ: {qData.title}");
    }
}
