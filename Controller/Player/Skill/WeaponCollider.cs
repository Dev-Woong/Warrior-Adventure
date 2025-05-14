using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    public SphereCollider DamageBox;
    
    private void Start()
    {
        if (DamageBox != null)
            DamageBox.enabled = false;
    }
    public void WeaponOnEnable()
    {
        DamageBox.enabled = true;
    }
    public void WeaponOnDisable()
    {
        DamageBox.enabled = false;
        DamageBox.GetComponent<HitBoxTrigger>().ClearHash();
    }

    
}
