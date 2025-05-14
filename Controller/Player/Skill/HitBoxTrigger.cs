using UnityEngine;
using System.Collections.Generic;
public class HitBoxTrigger : MonoBehaviour
{
    public readonly HashSet<Enemy> HitEnemies = new();
    public void ClearHash()
    {
        HitEnemies.Clear();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {   
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null&& !HitEnemies.Contains(enemy))
            {
                enemy.OnDamaged(PlayerController.Instance.FinalAtk);
                HitEnemies.Add(enemy);
                
            }
        }
    }
}
