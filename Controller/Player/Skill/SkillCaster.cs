using UnityEngine;

public class SkillCaster : MonoBehaviour
{
    public Transform firePoint; // 오브젝트 발사 위치
    public void FireSkill(string effectName,float finalDmg)
    {
        GameObject skillObj = ObjectPoolManager.instance.GetObject(effectName); // 매개변수로 이름 받아와서 풀링
        skillObj.transform.position = firePoint.position + new Vector3(0, 0.5f, 0);
        skillObj.transform.rotation = firePoint.rotation;
        SkillObject skill = skillObj.GetComponent<SkillObject>();
        skill.Init(finalDmg); 
    }
}
