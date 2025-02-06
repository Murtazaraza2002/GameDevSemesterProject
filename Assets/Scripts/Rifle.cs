using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    public Camera cam;


    public float Damage = 10f;
    public float ShootRange = 100f;
    public float fireCharge = 15f;
    private float nextTimeToShoot = 0f;
    public PlayerMovement pScript;

    //Related to ammo
    private int maxAmmo = 30;
    public int mag = 10;
    private int presentAmmo;
    public float reloadingTime = 1.5f;
    private bool setReloading = false;


    //Effects(HitEffect/FireEffect)
    public ParticleSystem MuzzleFlash;
    public GameObject WoodImpact;//Testing

    // Start is called before the first frame update
    void Start()
    {
        presentAmmo = maxAmmo;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if(setReloading)
        {
            return;
        }

        if(presentAmmo<=0)
        {
            StartCoroutine(Reload());
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToShoot)
        {
            nextTimeToShoot = Time.time + 1f / fireCharge;
            Shoot();
        }
    }

    void Shoot()
    {
        if(mag==0)
        {
            //Show That you are out of ammo
            return;
        }
        presentAmmo--;
        if(presentAmmo==0)
        {
            mag--;
        }

        MuzzleFlash.Play();
        RaycastHit HitInfo;
        
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out HitInfo,ShootRange))
        {
            Debug.Log(HitInfo.transform.name);

            ObjectDamage objectDamage = HitInfo.transform.GetComponent<ObjectDamage>();
            if(objectDamage!=null)
            {
                objectDamage.ObjectHit(Damage);
                GameObject Impact = Instantiate(WoodImpact, HitInfo.point,Quaternion.LookRotation(HitInfo.normal));
                Destroy(Impact, 1f);
            }
        }
    }

    IEnumerator Reload()
    {
        pScript.PlayerSpeed = 0f;
        pScript.PlayerSprint = 0f;
        setReloading = true;
        Debug.Log("Reloading...");
        //need animation + sound effect
        yield return new WaitForSeconds(reloadingTime);
        presentAmmo = maxAmmo;
        pScript.PlayerSpeed = 3.3f;
        pScript.PlayerSprint = 5f;
        setReloading = false;
    }

}
