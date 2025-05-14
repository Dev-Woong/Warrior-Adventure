using System.Collections;
using UnityEngine;

public class Boss_Slime_SlamAtk : MonoBehaviour
{
    public Animator animator;
    [Header("공격 반경")]
    public float slamRadius;
    [Header("슬라임 공격력 * slamDamage")]
    public int slamDamage;

    public LayerMask PlayerLayer;

    public bool isSlamming = false;
    public Vector3 targetPosition;
    public void TrySlam()
    {
        if (!isSlamming)
        {
            
            StartCoroutine(SlamSequence());
        }
    }
    IEnumerator SlamSequence()
    {
        isSlamming = true;
        animator.SetTrigger("Jump");
        for (int i = 0; i < 3; i++)
        {
            Vector3 slamPos = PlayerController.Instance.transform.position;

            GameObject warning = ObjectPoolManager.instance.GetObject("Boss_SlamTelegraph");
            warning.transform.position = slamPos;
            warning.transform.localScale = Vector3.one * slamRadius * 2;
            yield return new WaitForSeconds(2f);
            
            // 예고 제거
            ObjectPoolManager.instance.ReleaseObject(warning);

            // 폭발 이펙트 재생
            GameObject effect = ObjectPoolManager.instance.GetObject("Boss_SlamEffect");
            effect.transform.position = slamPos;

            // 데미지 판정
            Collider[] hits = Physics.OverlapSphere(slamPos, slamRadius, PlayerLayer);
            foreach (Collider hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    float SlamAtk = gameObject.GetComponent<Enemy>().atk * slamDamage;
                    hit.GetComponent<PlayerController>().OnDamage(SlamAtk);
                }
            }

            yield return new WaitForSeconds(0.3f); // 폭발 사이 텀
            ObjectPoolManager.instance.ReleaseObject(effect);
        }

        isSlamming = false;

    }
    //public void DoDamage()
    //{
    //    Instantiate(slamEffectPref, targetPosition, Quaternion.identity);

    //    Collider[] hits = Physics.OverlapSphere(targetPosition, slamRadius,PlayerLayer);
    //    foreach (Collider hit in hits)
    //    {
    //        if (hit.CompareTag("Player"))
    //        {
    //            float SlamAtk = gameObject.GetComponent<Enemy>().atk * slamDamage;
    //            if (hit != null)
    //            {
    //                hit.GetComponent<PlayerController>().OnDamage(SlamAtk);
    //            }
    //        }
    //    }
    //}
}
