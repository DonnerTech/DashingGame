using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private HealthData healthData;
    int hitPoints;

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

        //TODO: do damage animations and things
    }

    public void Die()
    {
        //TODO: trigger death animations and things

        Destroy(gameObject);
    }

}
