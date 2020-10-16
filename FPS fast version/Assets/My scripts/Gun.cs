using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class Gun : MonoBehaviour
{
    //Reloading:
    public int maxAmmo = 10;
    private int currentAmmo;
    public float reloadTime;
    private bool isReloading = false;

    //Weapon variables:
    private bool aiming = false;
    private float nextTimeToFire = 0f;
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 30f;
    

    //My stats:
    private float nextTimeToPush = 0f;
    public float pushRate = 30f;

    //Components:
    public Animator anim;
    public Camera cam;
    public ParticleSystem muzzleFlash;
    
        //Audio
            public AudioSource noAmmo;
            public AudioSource reloading;
            public AudioSource shot;
        //Scripts
            public PlayerController PlayerController;
            public MouseLook Mouse;


    //Set ammo to the maxAmmo:
    private void Start()
    {
        currentAmmo = maxAmmo;
    }
    //Toggles the weapon:
    private void OnEnable()
    {
        //Corrects the "Change weapon while reloading bug":
        isReloading = false;
        anim.SetBool("reloadAnim", false);
    }

    void Update()
    {
        //Inspect weapon when pressing T key
        if (Input.GetKeyDown(KeyCode.T))
        {
            anim.SetTrigger("Inspect");
        }

        //Wait for reload time:
        if (isReloading)
          return;

        
        
            //Get input to shoot:             
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
              //Checking for ammo:
            if (currentAmmo > 0)
            {
                //muzzleFlash.Play();
                nextTimeToFire = Time.time + 1f / fireRate;
                    
                //Runs the shooting function:
                Shoot();
                if (currentAmmo < 1)
                {
                    //Plays the out of ammo animation:
                    anim.SetBool("Out Of Ammo Slider", true);
                }
            }
            else
            {
                anim.SetBool("Out Of Ammo Slider",true);
                //Plays the "out of ammo" sound:
                noAmmo.Play();

                //Auto reload:
                    //StartCoroutine(Reload());
            }
        }
        //Get input to use the special power:
        if (Input.GetButton("Fire2") && Time.time >= nextTimeToPush)
        {
            //Start aiming:
            aiming = true;
            anim.SetBool("Aim",true);
            //Decrrease mouse sensitivity:
            Mouse.mouseSentitivity = 80f;
            //Zoom in the FOV:
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView,
                30.0f, 15.0f * Time.deltaTime);

            
        }
        else {
            //Stop aiming:
            aiming = false;
            anim.SetBool("Aim", false);

            //Set mouse sensitivity to default:
            Mouse.mouseSentitivity = 100f;
            //Set FOV to default:
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 60.0f, 15.0f * Time.deltaTime);
        }

        if (currentAmmo < maxAmmo && Input.GetKeyDown(KeyCode.R))
        {
            //If aiming, stop aiming:
            if (aiming)
            {
                aiming = false;
                anim.SetBool("Aim", false);
                //Set mouse sensitivity to default:
                Mouse.mouseSentitivity = 100f;
                //Set FOV to default:
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 60.0f, 15.0f * Time.deltaTime);
            }
            
            //Run reload coroutine:
            StartCoroutine(Reload());
        }
    }


    //Reloading:
    IEnumerator Reload()
    {
        //Plays the reloading animation:
        anim.SetBool("reloadAnim", true);
        //Locks any other action:
        isReloading = true;
            //UnityEngine.Debug.Log("Reloading");
        //Waits the reloading time:
        yield return new WaitForSeconds(reloadTime);

            //UnityEngine.Debug.Log("Reloaded");
        //Reloads:
        currentAmmo = maxAmmo;
        //Stops the animations:
        anim.SetBool("reloadAnim", false);
        anim.SetBool("Out Of Ammo Slider", false);
        //Unlocks the other actions:
        isReloading = false;
    }


    //Shooting:
    void Shoot()
    {
        //Animation, Sound and muzzle effects:
        if (!aiming)
        {
            //Plays the "fire from the hip" animation:
            anim.Play("Fire", 0, 0f);
        }
        else
        {
            //Plays the "aiming fire" animation:
            anim.Play("Aim Fire",0,0f);
        }
            //Plays the muzzle flash effect:
            muzzleFlash.Play();
            //Play the "shot" sound:
            shot.Play();
        //Uses ammo:
        currentAmmo--;
        //Raycasting to the target:
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {

                    //UnityEngine.Debug.Log(hit.transform.name);
            //Gets the target propeties:
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                
                if (hit.transform.gameObject.GetComponent<ExplosiveBarrelScript>() != null)
                {
                    //If the target is a barrel, explode it:
                    hit.transform.gameObject.GetComponent<ExplosiveBarrelScript>().explode = true;
                }else if (hit.transform.gameObject.GetComponent<TargetScript>() != null)
                {
                    //If the target is a shooting target, hit it:
                    hit.transform.gameObject.GetComponent<TargetScript>().isHit = true;
                }
                else
                {
                    //If the target is a damageble object, damage it:
                    target.TakeDamage(damage);
                }
            }
            
            //Adding physics to the target:
           if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
        }
    }

    //Using the special power:
   /* void Push()
    {
        RaycastHit push;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out push, range))
        {
            if (push.rigidbody != null)
            {
                push.rigidbody.AddForce(-push.normal * impactForce);
            }
            
        }
    }*/
}

