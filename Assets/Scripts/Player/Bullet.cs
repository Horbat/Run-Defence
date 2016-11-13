using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public int minDamage = 1;
    public int maxDamage = 3;
    public LayerMask damageMask;

    float _speed = 50f;

    void Start()
    {
        Destroy(gameObject, 5f);
        StartSphereCast();
    }

    void StartSphereCast()
    {
        Collider[] closeEnemyColl = Physics.OverlapSphere(transform.position, 0.5f);
        foreach (var enemyColl in closeEnemyColl)
        {
            LivingEntity entity = enemyColl.gameObject.GetComponent<LivingEntity>();
            if (entity != null)
            {
                entity.TakeDamage(Random.Range(minDamage, maxDamage + 1));
                Destroy(gameObject);
            }
        }
    }

	void FixedUpdate()
    {
        CheckCollision();
	}

    void CheckCollision()
    {
        Vector3 velocity = transform.forward * _speed * Time.fixedDeltaTime;
        Ray ray = new Ray(transform.position, velocity);

        RaycastHit hit;        
        if (Physics.Raycast(ray, out hit, velocity.sqrMagnitude, damageMask))
        {
            LivingEntity entity = hit.collider.gameObject.GetComponent<LivingEntity>();
            if (entity != null)
            {
                entity.TakeDamage(Random.Range(minDamage, maxDamage + 1));
                Destroy(gameObject);
                return;
            }
        }

        transform.position += velocity;
    }
}
