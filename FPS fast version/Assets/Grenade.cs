using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    //Grenade variables:
    public float delay = 3f;
    private float countdown;
    private bool hasExploded = false;
    public float force = 700f;
    public float radius = 5f;


    //Components:
    public ParticleSystem explosion;
    public Transform explosionPrefab;

    //Audio:
    public AudioSource impactSound;

    // Start is called before the first frame update
    void Start()
    {
        //Sets the countdown value equals to the delay;
        countdown = delay;  
    }

    // Update is called once per frame
    void Update()
    {
        //Decrease the countdown value:
        countdown -= Time.deltaTime;
        //If the time runs out and it hasen´t exploded:
        if (countdown <= 0f && !hasExploded)
        {
            //Explode it:
            Explode();
            hasExploded = true; 
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Play the impact sound on every collision
        impactSound.Play();
    }
    void Explode()
    {
        //Instantiate metal explosion prefab on ground
        Instantiate(explosionPrefab, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            { 
                if (nearbyObject.transform.gameObject.GetComponent<ExplosiveBarrelScript>() != null)
                {
                    //If the target is a barrel, explode it:
                    nearbyObject.transform.gameObject.GetComponent<ExplosiveBarrelScript>().explode = true;
                }
                else
                {
                    //If the target is a damageble object, damage it:
                    rb.AddExplosionForce(force, transform.position, radius);
                }
                
            }
            if (nearbyObject.transform.gameObject.GetComponent<TargetScript>() != null)
            {
                //If the target is a shooting target, hit it:
                nearbyObject.transform.gameObject.GetComponent<TargetScript>().isHit = true;
            }
        }
        Destroy(gameObject);
    }
}

