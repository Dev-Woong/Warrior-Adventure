using UnityEngine;

using System.Collections.Generic;

public class QuestUIManager : MonoBehaviour
{
    public GameObject questSlotPrefab;
    public Transform questListParent;
    private List<GameObject> spawnedSlots = new();
    public GameObject MaxPanel;
    public GameObject MinPanel;
    public bool isMaxUI=true;
    public void UIMinMaxButtonClick()
    {
        isMaxUI = !isMaxUI;
    }
    public void UIMinMaxProcess()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isMaxUI = !isMaxUI;
        }
    }
    void UIMinMax()
    {
        MaxPanel.SetActive(isMaxUI);
        MinPanel.SetActive(!isMaxUI);
    }

    void OnEnable()
    {
        QuestManager.OnQuestListUpdated += RefreshUI;
    }

    void OnDisable()
    {
        QuestManager.OnQuestListUpdated -= RefreshUI;
    }

    public void RefreshUI()
    {
        if (questSlotPrefab == null || questListParent == null)
        {
            Debug.LogError("UI 설정이 빠졌습니다!");
            return;
        }

        if (QuestManager.Instance == null)
        {
            Debug.LogWarning("QuestManager가 아직 초기화되지 않았습니다.");
            return;
        }

        // 기존 슬롯 제거
        foreach (var slot in spawnedSlots)
        {
            Destroy(slot);
        }
        spawnedSlots.Clear();

        // 새로운 슬롯 생성
        foreach (var quest in QuestManager.Instance.activeQuests)
        {
            var slotGO = Instantiate(questSlotPrefab, questListParent);
            var slotUI = slotGO.GetComponent<QuestSlotUI>();
            if (slotUI == null)
            {
                Debug.LogError("QuestSlotUI 컴포넌트가 프리팹에 없습니다!");
                continue;
            }
            slotUI.Setup(quest);
            spawnedSlots.Add(slotGO);
        }
    }
    private void Update()
    {
        UIMinMax();
        UIMinMaxProcess();
    }
}