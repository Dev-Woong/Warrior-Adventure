using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class QuestSlotUI : MonoBehaviour
{
    public Text titleText;
    public Text progressText;

    public void Setup(QuestInstance quest)
    {
        
        titleText.text = quest.qData.title;
        if (quest.qData.questType == QuestType.Kill)
            progressText.text = $"{quest.currentAmount}/{quest.qData.requiredAmount}";
        else if (quest.qData.questType == QuestType.Talk)
        { 
            progressText.text = $"{quest.qData.targetName}에게 말걸기"; 
        }
        if (quest.IsCompleted)
        {
            titleText.color = Color.green;
            progressText.color = Color.green;
            progressText.text = "완료";
            StartCoroutine(CompliteTextEffect(quest));
            if (quest.isRewardGiven)
            {
                StopCoroutine(CompliteTextEffect(quest));
                gameObject.GetComponent<Image>().color = new Color32(0,0,0,190);
            }
        }
        else
        {
            titleText.color = Color.white;
            progressText.color = Color.white;
        }
    }
    IEnumerator CompliteTextEffect(QuestInstance quest)
    {
        int a = 0;
        while (true)
        {
            if (a % 4 == 0)
            {
                gameObject.GetComponent<Image>().color = new Color32(85, 255, 0, 90);
            }
            else if (a % 4 == 1)
            {
                gameObject.GetComponent<Image>().color = new Color32(85, 255, 0, 115);
            }
            else if (a % 4 == 2)
            {
                gameObject.GetComponent<Image>().color = new Color32(85, 255, 0, 140);
            }
            else
            {
                gameObject.GetComponent<Image>().color = new Color32(85, 255, 0, 115);
            }
            yield return new WaitForSeconds(0.2f);
            a++;
        }
    }
}