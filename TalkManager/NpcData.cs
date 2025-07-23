using UnityEngine;

[CreateAssetMenu(fileName = "New NPC", menuName = "NPC/NPCData")]
public class NPCData : ScriptableObject
{
    public string npcName;
    [TextArea] public string[] dialogueLines;
    public Sprite npcIllustration;
    public string NpcID;
    public QuestData questToGive;
   
    
}