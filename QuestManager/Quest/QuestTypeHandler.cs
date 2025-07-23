using UnityEngine;

public class QuestTypeHandler : MonoBehaviour
{
    public interface IQuestLogicHandler
    {
        void Init(QuestInstance quest);
        void OnEvent(string targetID); // ��: �� óġ, ������ ȹ�� ��
    }
    public class KillQuestHandler : IQuestLogicHandler
    {
        private QuestInstance quest;

        public void Init(QuestInstance quest)
        {
            this.quest = quest;
        }

        public void OnEvent(string targetID)
        {
            if (quest.qData.targetID == targetID && !quest.IsCompleted)
            {
                quest.currentAmount++;
                Debug.Log(quest.currentAmount);
                QuestManager.NotifyQuestListUpdated();
                if (quest.IsCompleted)
                {
                    Debug.Log("����Ʈ �Ϸ��~");
                }
                
            }
        }
    }
    public class CollectQuestHandler : IQuestLogicHandler
    {
        private QuestInstance quest;

        public void Init(QuestInstance quest)
        {
            this.quest = quest;
        }

        public void OnEvent(string targetID)
        {
            if (quest.qData.targetID == targetID && !quest.IsCompleted)
            {
                quest.currentAmount++;
            }
        }
    }
    public class TalkQuestHandler : IQuestLogicHandler
    {
        private QuestInstance quest;

        public void Init(QuestInstance quest)
        {
            this.quest = quest;
        }

        public void OnEvent(string npcID)
        {
            if (quest.qData.targetID == npcID)
            {
                quest.currentAmount = 1;
            }
        }
    }
}
