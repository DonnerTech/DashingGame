using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    public GameObject parentObject;
    public ObjectPool<GameObject> parentPool;

    public BulletData bulletData;

    private Rigidbody rb;

    private int lifetime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //TODO: set collider size to bulletData.size
    }

    void OnEnable()
    {
        lifetime = 0;
        if(rb != null)
            rb.linearVelocity = Vector3.zero;
    }

    // Update is called once per fixed frame
    void FixedUpdate()
    {
        // return the bullet to its source object pool if it is too old
        if (lifetime >= bulletData.lifetime)
        {
            parentPool.Release(gameObject);
        }

        // move the bullet
        Vector3 velocity = transform.forward * bulletData.speed;
        rb.linearVelocity = velocity;
        // rb.angularVelocity = Vector3.zero;

        lifetime++;
    }

    void OnTriggerEnter(Collider other)
    {
        // check if the bullet hit an object that is not the parent
        if (!other.gameObject.name.Equals(parentObject.name))
        {
            Debug.Log($"Bullet Collision with {other.gameObject.name}");
            Debug.Log($"Bullet Parent Name: {parentObject.name}");

            //TODO: do damage to hit object
            Health otherHealth = other.gameObject.GetComponent<Health>();
            if (otherHealth != null)
                otherHealth.TakeDamage(bulletData.damage);

            parentPool.Release(gameObject);
        }
    }
}
