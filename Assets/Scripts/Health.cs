using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private HealthData healthData;
    int hitPoints;

    [Tooltip("Provides the ammount of healthPoints left")]
    public UnityEvent<int> TookDamage;
    public UnityEvent Dying;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hitPoints = healthData.maxHealth;
    }

    public void TakeDamage(int damage)
    {
        hitPoints -= damage;

        if (hitPoints <= 0)
        {
            Die();
        }
        else
        {
            TookDamage?.Invoke(hitPoints);
        }

        //TODO: do damage animations and things
    }

    public void Die()
    {
        //TODO: trigger death animations and things

        Dying?.Invoke();

        Destroy(gameObject);
    }

}
