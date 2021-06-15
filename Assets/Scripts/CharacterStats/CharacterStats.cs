
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat damage;
    public int maxHealth = 100;
    public int currentHealth { get; private set; }

    void Awake ()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage (int damage)
    {
        currentHealth -= damage;
        Debug.Log(transform.name + " takes " + damage + ": damage");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    //allows this method to be changed based on the character.
    public virtual void Die()
    {
        //Die in some way
        //This method is meant to be overwritten
    }

}
