using UnityEngine;
using UnityEngine.AI;
public enum SlimeAnimationState { Idle, Walk, Jump, Attack, Damage,Die }
public class EnemyAI : MonoBehaviour
{
    
    public Face faces;
    public GameObject SlimeBody;
    public GameObject SlimeHitbox;
    public GameObject Slime;

    public SlimeAnimationState currentState;

    public Animator animator;
    public NavMeshAgent agent;
    public Transform target;
    private Material faceMaterial;

    
    public bool isDamaged;
    public float traceTime = 0;
    public float hitboxHoldingTime = 0;


    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        
        faceMaterial = SlimeBody.GetComponent<SkinnedMeshRenderer>().materials[1];
        agent = gameObject.GetComponent<NavMeshAgent>();
        SlimeHitbox.SetActive(false);
    }
    
    void MonsterState()
    {
        
        switch (currentState)
        {
            case SlimeAnimationState.Idle:
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) return;
                animator.SetFloat("Speed", 0);
                AgentStop();
                break;
            case SlimeAnimationState.Walk:
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")) return;
                AgentStart();
                if (target == null) { return; } 
                else { TargetTrace(); }
                animator.SetFloat("Speed", agent.velocity.magnitude); 
                break;
            case SlimeAnimationState.Attack:
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) return;
                //agent.isStopped = true;
                
                //AgentStop();
                SetFace(faces.AttackFace);
                animator.SetTrigger("Attack"); 
                hitboxHoldingTime = 0.1f;
                
                break;
            case SlimeAnimationState.Die:
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Damage2")) return;
                animator.SetTrigger("Damage");
                animator.SetInteger("DamageType", 2);
                AgentStop();
                break;
            case SlimeAnimationState.Damage:
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Damage0")) return;
                AgentStop();
                animator.SetTrigger("Damage");
                animator.SetInteger("DamageType", 0);
                SetFace(faces.DamageFace);
                break;
        }
    }
    void SetFace(Texture tex)
    {
        faceMaterial.SetTexture("_MainTex", tex);
        
    }
    void CallHitBox()
    {
        if (hitboxHoldingTime >= 0)
        {
            SlimeHitbox.SetActive(true);
        }
        else if(hitboxHoldingTime<0)
        {
            SlimeHitbox.SetActive(false); 
        }
    }
    void TargetTrace()
    {
        if (isDamaged == true)
        {
            agent.SetDestination(target.position);
        }
    }
    void OnEnable()
    {
        ResetState();  // 풀에서 나올 때 상태 초기화
    }

    void ResetState()
    {
        traceTime = 0f;
        isDamaged = false;
        // 애니메이션이나 상태머신 초기화도 여기에
    }
    public void TraceTimeZero ()
    {
        traceTime = 0;
    }
    void StopTrace()
    {
        if (traceTime <= 0)
        {
            isDamaged = false;
        }
        if (isDamaged == false)
        {
            AgentStop();
        }
    }
    void Attack()
    {
        float distance = Vector3.Distance(gameObject.transform.position, target.position);
        if (distance <= 1f&&isDamaged== true)
        {
            currentState = SlimeAnimationState.Attack;
        }
        else currentState = SlimeAnimationState.Walk;
    }
    void AgentStop()
    {
        agent.isStopped = true;
        agent.updateRotation = false;
    }
    void AgentStart()
    {
        agent.isStopped = false;
        agent.updateRotation = true;
    }
    
    void Update()
    {
        hitboxHoldingTime -= Time.deltaTime;
        traceTime -= Time.deltaTime;
        MonsterState();
        StopTrace();
        Attack();
        CallHitBox();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AttackBox"))
        {
            currentState = SlimeAnimationState.Damage;
            isDamaged = true;
            traceTime = 10f;
        }
    }
}
