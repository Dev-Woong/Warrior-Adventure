using UnityEngine;
using TMPro;
public enum EnemyType 
{
    normal,
    boss
}
public class Enemy : PoolAble
{
    public EnemyType enemyType;
    public GameObject MonsterPref;
    public float maxHp = 100;
    public float curHp;
    public float atk = 10;
    public float def = 2;
    public float dieTimer = 0;
    public int deathCount = 0;
    public float dropExp= 30;
    public string EnemyID;
    public string[] dropItemName;
    GameObject cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        curHp = maxHp;
        cam = Camera.main.gameObject;
    }
    void DamageTextSpawn(Vector3 position, float damage)
    { 
        var DmgText = ObjectPoolManager.instance.GetObject("DamageText");
        DmgText.transform.position = position;
        DmgText.GetComponent<DamageText>().SetText(damage);
    }
    public void ChangeStateTo(SlimeAnimationState state)
    {
        if (MonsterPref == null) return;
        if (state == MonsterPref.GetComponent<EnemyAI>().currentState) return;
        MonsterPref.GetComponent<EnemyAI>().currentState = state;
    }
    public void ChangeStateTo(BossAnimationState state)
    {
        if (MonsterPref == null) return;
        if (state == MonsterPref.GetComponent<EnemyAI_Boss>().currentState) return;
        MonsterPref.GetComponent<EnemyAI_Boss>().currentState = state;
    }
    public void BossDie()
    {
        this.gameObject.GetComponent<EnemyAI_Boss>().TraceTimeZero();
        QuestManager.Instance.UpdateQuestEvent(EnemyID);
        ChangeStateTo(BossAnimationState.Die);
        curHp = maxHp;
        //EnemySpawner.monster_Count--;
        PlayerController.Instance.curExp += dropExp;
        DropItem();
        ReleaseObject();
    }
    public void OnDamaged(float causerAtk)
    {
        Vector3 DmgTextPos = transform.position + new Vector3(0, 2, 0);
        float finalDamage = causerAtk - def;
        if (finalDamage <= 0)
            finalDamage = 0; 
        curHp -= finalDamage;
        DamageTextSpawn(DmgTextPos,finalDamage);
        if (curHp <= 0)
        {
            if (enemyType == EnemyType.normal)
            {
                Die();
            }
            else if (enemyType == EnemyType.boss)
            {
                BossDie();
            }
        }
    }
    public void Die()
    {
        this.gameObject.GetComponent<EnemyAI>().TraceTimeZero();
        QuestManager.Instance.UpdateQuestEvent(EnemyID);
        ChangeStateTo(SlimeAnimationState.Die);
        curHp = maxHp;
        EnemySpawner.Instance.monster_Count--;
        PlayerController.Instance.curExp += dropExp;
        DropItem();
        ReleaseObject();
    }

    void DropItem()
    {
        int rnd = Random.Range(0, 50);
        if (rnd <30)
        {
            Drop("HpPotion");
        }
        if (rnd <20)
        {
            Drop("MpPotion");
        }
        if (rnd >= 0 && rnd <= 5)
        {
            Drop("Weapon_1");
        }
        else if (rnd >= 6 && rnd <= 10)
        {
            Drop("Helmet_1");
        }
        else if (rnd >= 11 && rnd <= 15)
        {
            Drop("Armor_1");
        }
        else if (rnd >= 16 && rnd <= 20)
        {
            Drop("Belt_1");
        }
        else if (rnd >= 21 && rnd <= 25)
        {
            Drop("Boots_1");
        }
        else if (rnd >= 26 && rnd <= 30)
        {
            Drop("Gloves_1");
        }
        else return;
    }
    void Drop(string itemName)
    {
        GameObject itemObj = ObjectPoolManager.instance.GetObject(itemName); // 매개변수로 이름 받아와서 풀링
        itemObj.transform.position = transform.position + new Vector3(0, 1.5f, 0);
        itemObj.transform.rotation = transform.rotation;
        itemObj.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0f, 1f), 1, Random.Range(0f, 1f)) * 3, ForceMode.Impulse);
        
    }
}
