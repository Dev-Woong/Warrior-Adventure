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

    private Transform lastSpawnSpot = null; // ���������� ��ȯ�� ��ġ�� ����

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }
    private void Start()
    {
        
        // �迭�� ���̰� 1 �̻��� ���� ���� ���� ����
        if (SpawnSpot.Length > 0)
        {
            StartCoroutine(MonSpawn());
        }
        else 
        {
            Debug.LogWarning("SpawnSpot. ���� ����Ʈ�� ��ġ������ �ʹ� �����ϴ�!");
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
                        Debug.LogWarning("Slime_Boss ������Ʈ�� ã�� �� �����ϴ�.");
                    }
                }
                else
                {
                    Debug.LogWarning("Slime_Boss ������Ʈ�� ��ȯ��ġ�� ã�� �� �����ϴ�.");
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
            // ���� ���� ���� �ִ� ���� ������ ������ ���͸� ����
            if (monster_Count < monster_maxCount)
            {
                // ���� ���� �ֱ� ��ٸ���
                yield return new WaitForSeconds(enemyCreateTime);

                // �������� ��ȯ�� ��ġ�� ������ ���� ��ġ ����
                Transform spawnSpot = GetRandomSpawnSpot();

                // "Slime" ���͸� ������Ʈ Ǯ���� ��������
                GameObject enemy = EnemyPoolManager.instance.GetObject("Slime");
                if (enemy != null)
                {
                    enemy.transform.position = spawnSpot.position;
                    enemy.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0,359), 0));
                    monster_Count++;
                    lastSpawnSpot = spawnSpot; // ������ ��ȯ�� ��ġ ������Ʈ
                }
                else
                {
                    Debug.LogWarning("Slime ������Ʈ�� Ǯ���� ������ �� �����ϴ�.");
                }
            }
            else
            {
                // ���� ���� �ִ�ġ�� �����ϸ� ���
                yield return new WaitForSeconds(enemyCreateTime); // ��� �ð���ŭ ��ٸ�
            }
        }
    }

    // ������ ��ȯ�� ��ġ�� ������ ���� ��ġ�� ��ȯ�ϴ� �Լ�
    private Transform GetRandomSpawnSpot()
    {
        Transform spawnSpot = null;

        // ������ ��ȯ�� ��ġ�� ��ġ�� �ʴ� ������ ��ġ ã��
        do
        {

            int idx = Random.Range(0, SpawnSpot.Length);
            spawnSpot = SpawnSpot[idx];
        } while (spawnSpot == lastSpawnSpot); // ������ ��ġ�� ������ �ٽ� �������� ã��

        return spawnSpot;
    }
}