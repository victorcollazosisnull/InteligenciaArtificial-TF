using UnityEngine;
namespace oldStructureVictorGozu
{
    public class Health : MonoBehaviour
    {
        public float maxHealth = 100f;

        public float currentHealth;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;

            Debug.Log(gameObject.name + " recibió daño. Vida actual: " + currentHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log(gameObject.name + " murió");

            Destroy(gameObject);
        }
    }
}