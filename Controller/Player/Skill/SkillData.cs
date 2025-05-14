using UnityEngine;
public enum SkillType
{
    SingleTarget,
    MultiTarget,
    AOE
}
public enum SkillTarget
{
    Player,
    Enemy
}

[CreateAssetMenu(fileName = "SkillData", menuName = "Skill/SKill Data")]
public class SkillData : ScriptableObject
{
    public SkillType skillType;
    public SkillTarget skilltarget;
    public string SkillName; // 스킬이름 => 지금 딱히 필요없긴함
    //public Sprite Icon;  // 스킬 아이콘 => UI에 올려놓을 스킬이미지 (지금 못구함)
    //public float coolDown; // 쿨타임 => 확장 예정
    [Header("오브젝트 풀링시 가져올 스트링값")]
    public string effectName; // 스킬 이펙트 이름 => 오브젝트 풀링시 가져올값. 꼭필요함
    public float lifeTime; // 스킬 지속시간
    public float speed; // 스킬 날아가는 속도
    
    [Header("스킬 데미지 배율값(damage*Atk)")]
    public float damage; // 스킬 데미지
    
}
