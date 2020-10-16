using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponSwitch : MonoBehaviour
{

    public int selectedWeapon = 0;
    public AudioSource switchAudio;
    // Start is called before the first frame update
    void Start()
    {
        selectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= transform.childCount - 1)
            {
                //switchAudio.Play();
                selectedWeapon = 0;
            }
            else {
                //switchAudio.Play();
                selectedWeapon++;
            }
        } else if (Input.GetAxis("Mouse ScrollWheel") < 0f) { 
            if (selectedWeapon <= 0)
            {
               // switchAudio.Play();
                selectedWeapon = transform.childCount - 1;
            }
            else {
              //  switchAudio.Play();
                selectedWeapon--;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedWeapon = 1;
        }
        if (previousSelectedWeapon != selectedWeapon)
        {
            selectWeapon();
        }
        
    }

    void selectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if ( i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i ++;
        }
    }
}
