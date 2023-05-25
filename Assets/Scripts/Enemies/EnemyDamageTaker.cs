using UnityEngine;

public class EnemyDamageTaker : MonoBehaviour
{
    [SerializeField] private AnimationClip death;
    [SerializeField] private AnimationClip hurt;
    [SerializeField] private Animator anim;
    [SerializeField] private int maxHealth;
    private int currentHealth;

    void Start() 
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage) 
    {
        currentHealth -= damage;

        if (currentHealth <= 0) 
        {
            Die();
        }
        else 
        {
            anim.Play(hurt.name);
        }
    }

    private void Die() 
    {
        anim.Play(death.name);
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
