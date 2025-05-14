using UnityEngine;

public class NpcAnim : MonoBehaviour
{
    Animator anim; 
    float idleRotTime = 6;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    void IdleRotation()
    {
        if (idleRotTime <= 0)
        {
            anim.SetTrigger("Idle2");
            idleRotTime = 7;
        }
    }
    // Update is called once per frame
    void Update()
    {
        idleRotTime -= Time.deltaTime;
        IdleRotation();
    }
}
