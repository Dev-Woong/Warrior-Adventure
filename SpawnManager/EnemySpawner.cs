using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;
    public Transform[] SpawnSpot;
    public Transform BossSpawnSpot;
    public float enemyCreateTime;
    public float bossCreateTime;
    public int monster_Count = 0;
    public int boss_Count = 0;
    public int boss_maxCount = 1;
    public int monster_maxCount = 11;
    public bool isGameOver = false;

    private Transform lastSpawnSpot = null; // 마지막으로 소환된 위치를 저장

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }
    private void Start()
    {
        
        // 배열의 길이가 1 이상일 때만 몬스터 스폰 시작
        if (SpawnSpot.Length > 0)
        {
            StartCoroutine(MonSpawn());
        }
        else 
        {
            Debug.LogWarning("SpawnSpot. 스폰 포인트의 위치정보가 너무 적습니다!");
        }
        StartCoroutine(BossSpawn());
    }
    IEnumerator BossSpawn()
    {
        while (!isGameOver)
        {

            if (boss_Count < boss_maxCount)
            {
                yield return new WaitForSeconds(bossCreateTime);
                if (BossSpawnSpot != null)
                {
                    GameObject boss = EnemyPoolManager.instance.GetObject("Slime_Boss");
                    if (boss != null)
                    {
                        boss.transform.position = BossSpawnSpot.position;
                        boss_Count++;
                    }
                    else 
                    {
                        Debug.LogWarning("Slime_Boss 오브젝트를 찾을 수 없습니다.");
                    }
                }
                else
                {
                    Debug.LogWarning("Slime_Boss 오브젝트의 소환위치를 찾을 수 없습니다.");
                }
            }
            else 
            {
                yield return new WaitForSeconds(1f);
            }
        }
    }

    IEnumerator MonSpawn()
    {
        while (!isGameOver)
        {
            // 현재 몬스터 수가 최대 몬스터 수보다 적으면 몬스터를 스폰
            if (monster_Count < monster_maxCount)
            {
                // 몬스터 생성 주기 기다리기
                yield return new WaitForSeconds(enemyCreateTime);

                // 마지막에 소환된 위치를 제외한 랜덤 위치 선택
                Transform spawnSpot = GetRandomSpawnSpot();

                // "Slime" 몬스터를 오브젝트 풀에서 가져오기
                GameObject enemy = EnemyPoolManager.instance.GetObject("Slime");
                if (enemy != null)
                {
                    enemy.transform.position = spawnSpot.position;
                    enemy.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0,359), 0));
                    monster_Count++;
                    lastSpawnSpot = spawnSpot; // 마지막 소환된 위치 업데이트
                }
                else
                {
                    Debug.LogWarning("Slime 오브젝트를 풀에서 가져올 수 없습니다.");
                }
            }
            else
            {
                // 몬스터 수가 최대치에 도달하면 대기
                yield return new WaitForSeconds(enemyCreateTime); // 대기 시간만큼 기다림
            }
        }
    }

    // 마지막 소환된 위치를 제외한 랜덤 위치를 반환하는 함수
    private Transform GetRandomSpawnSpot()
    {
        Transform spawnSpot = null;

        // 마지막 소환된 위치와 겹치지 않는 랜덤한 위치 찾기
        do
        {

            int idx = Random.Range(0, SpawnSpot.Length);
            spawnSpot = SpawnSpot[idx];
        } while (spawnSpot == lastSpawnSpot); // 마지막 위치와 같으면 다시 랜덤으로 찾기

        return spawnSpot;
    }
}