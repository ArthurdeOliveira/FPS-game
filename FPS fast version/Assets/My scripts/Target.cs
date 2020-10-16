using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 50f;
    public AudioSource impactSound;
    public GameObject destroyedVersion;	// Reference to the shattered version of the object

    public void TakeDamage(float amount)
    {

        health -= amount;
        impactSound.Play();
        if (health <= 0)
        {
            Die();
        }


    }
   
    public void Die()
    {
        Destroy(gameObject);
        // Spawn a shattered object
        Instantiate(destroyedVersion, transform.position, transform.rotation);
        // Remove the current object
        Destroy(gameObject);
    }

}