using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    //Variables:
    public float throwForce = 40f;

    //Components:
    public GameObject grenadePrefab;   

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            throwGrenade();
        }
    }

    void throwGrenade()
    {
        GameObject grenade = Instantiate(grenadePrefab,transform.position,transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
    }
}
