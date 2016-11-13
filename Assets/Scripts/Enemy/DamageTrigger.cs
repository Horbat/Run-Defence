using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
public class DamageTrigger : MonoBehaviour
{
    [HideInInspector]
    public int minDamage = 1;
    [HideInInspector]
    public int maxDamage = 2;
    [HideInInspector]
    public LayerMask targetMask;

    SphereCollider _sphereColl;

    void Awake()
    {
        _sphereColl = GetComponent<SphereCollider>();
        _sphereColl.isTrigger = true;
    }

    public void StartAttack()
    {
        _sphereColl.enabled = true;
    }

    public void SetStartProperties()
    {
        _sphereColl.enabled = false;
    }

    public void OnTriggerEnter(Collider target)
    {
        if (Mathf.Pow(2, target.gameObject.layer) == targetMask.value)
        {
            LivingEntity entity = target.gameObject.GetComponent<LivingEntity>();

            if (entity != null)
            {
                entity.TakeDamage(Random.Range(minDamage, maxDamage + 1));
            }

            SetStartProperties();    
        }
    }
}
