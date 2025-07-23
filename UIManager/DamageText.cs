using UnityEngine;
using TMPro;
public class DamageText : PoolAble
{
    public float floatingSpeed = 2f;
    public float releaseTime = 1f;
    private float timer;
    public Vector3 moveDir = Vector3.up;

    private TextMeshPro DmgText;
    private Transform cam;
    void Awake()
    {
        cam = Camera.main.transform;
    }
    private void OnEnable()
    {
        timer = 0;
        if (DmgText == null)
            DmgText = GetComponent<TextMeshPro>();
    }
    void Update()
    {
        transform.position += floatingSpeed * Time.deltaTime * moveDir;

        // ī�޶� �׻� �ٶ󺸰�
        if (cam != null)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - cam.position);
        }

        // �ð� ������ Ǯ�� ��ȯ
        timer += Time.deltaTime;
        if (timer >= releaseTime)
        {
            if (Pool != null)
                ReleaseObject();
            else
                Destroy(gameObject);
        }
    }
    public void SetText(float damage)
    {
        DmgText.text = damage.ToString();
        if (damage <= 0)
        {
            DmgText.text = "Miss";
        }
        if (DmgText == null)
            DmgText = GetComponent<TextMeshPro>();
    }
}
