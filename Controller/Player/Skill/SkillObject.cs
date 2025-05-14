using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SkillObject : PoolAble
{
    public SkillData sd;
    private Rigidbody rb;
    public float timer;
    public float finalDmg;
    private readonly HashSet<Enemy> HitEnemy = new();
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
    }
    private void OnEnable()
    {
        rb.linearVelocity = Vector3.zero;
        timer = 0f;
    }
    public void Init(float casterAtk)
    {
        if (sd == null)
        {
            Debug.LogWarning("SkillData가 설정되지 않았습니다.");
            return;
        }
        HitEnemy.Clear();
        finalDmg = sd.damage * casterAtk;
        switch (sd.skilltarget) 
        {
            case SkillTarget.Enemy:
                if (sd.skillType != SkillType.AOE)
                {
                    rb.linearVelocity = transform.forward * sd.speed;
                }
                break;

            case SkillTarget.Player:
                if (sd.skillType != SkillType.AOE)
                {
                    Vector3 Dir = PlayerController.Instance.transform.position - transform.position;
                    rb.linearVelocity = Dir.normalized * sd.speed;
                }
                break;
        }
        
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= sd.lifeTime)
        {
            ReleaseObject();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (sd.skilltarget)
        {
            case SkillTarget.Enemy:

                if (other.CompareTag("Enemy"))
                {
                    Enemy enemy = other.GetComponent<Enemy>();
                    if (enemy != null && !HitEnemy.Contains(enemy))
                    {
                        HitEnemy.Add(enemy);
                        if (sd.skillType == SkillType.SingleTarget)
                        {
                            enemy.OnDamaged(finalDmg);
                            ReleaseObject();
                        }
                        else if (sd.skillType == SkillType.MultiTarget)
                        {
                            enemy.OnDamaged(finalDmg);
                        }
                        else if (sd.skillType == SkillType.AOE)
                        {
                            enemy.OnDamaged(finalDmg);
                        }
                    }
                }

                break;
            case SkillTarget.Player:
                if (other.CompareTag("Player"))
                {
                    PlayerController player = other.GetComponent<PlayerController>();
                    if (sd.skillType == SkillType.SingleTarget)
                    {
                        if (player.isUnHit == false)
                        {
                            player.OnDamage(finalDmg);
                            ReleaseObject();
                        }
                        else return;
                    }
                    else if (sd.skillType == SkillType.MultiTarget)
                    {
                        if (player.isUnHit == false)
                        {
                            player.OnDamage(finalDmg);
                        }
                    }
                    else if (sd.skillType == SkillType.AOE)
                    {
                        if (player.isUnHit == false)
                        {
                            player.OnDamage(finalDmg);
                        }
                    }
                }
                break;
        }
       
    }
}
