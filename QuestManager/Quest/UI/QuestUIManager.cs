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
            Debug.LogError("UI ������ �������ϴ�!");
            return;
        }

        if (QuestManager.Instance == null)
        {
            Debug.LogWarning("QuestManager�� ���� �ʱ�ȭ���� �ʾҽ��ϴ�.");
            return;
        }

        // ���� ���� ����
        foreach (var slot in spawnedSlots)
        {
            Destroy(slot);
        }
        spawnedSlots.Clear();

        // ���ο� ���� ����
        foreach (var quest in QuestManager.Instance.activeQuests)
        {
            var slotGO = Instantiate(questSlotPrefab, questListParent);
            var slotUI = slotGO.GetComponent<QuestSlotUI>();
            if (slotUI == null)
            {
                Debug.LogError("QuestSlotUI ������Ʈ�� �����տ� �����ϴ�!");
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