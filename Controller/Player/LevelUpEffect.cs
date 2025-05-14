using UnityEngine;

public class LevelUpEffect : PoolAble
{
    public static float LifeTime=0;
    public Transform target;

    private void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();
    }
    void Update()
    {
        LifeTime -= Time.deltaTime;
        if (LifeTime <= 0)
        {
            ReleaseObject();
        }
        transform.position = target.position;
    }
}
