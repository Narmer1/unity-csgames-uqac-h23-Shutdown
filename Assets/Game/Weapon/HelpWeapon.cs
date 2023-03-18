using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class HelpWeapon : MonoBehaviour
{
    
    
    public GameObject myBullet;

    public GameObject Shell;

    public GameObject myShellposition;

    public int dammage;
    public float timeBetweeShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletShots;

    bool shooting, readyToShoot, reloading;

    public Camera fpsCamera;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask WhatIsEnemy;

    public GameObject MuzzleEffect;


    //Graphics
    public GameObject muzzleFlash, bulletHoleGraphic;
    public float camShakeMagnitude, camShakeDuration;

    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
        
        text.SetText(bulletsLeft + " / " + magazineSize);
    }

    private void MyInput()
    {
        //Shooting
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        //Reload
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();
        
        //Shoot
        if (readyToShoot && shooting && !reloading && HaveBulletsLeft())
        {
            bulletShots = bulletsPerTap;
            Shoot();
        }
    }

    private void Shoot()
    {

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 direction = fpsCamera.transform.forward + new Vector3(x, y, 0);

        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out rayHit,range, WhatIsEnemy))
        {
            Debug.Log(rayHit.collider.name);
            if (rayHit.collider.CompareTag("Enemy"))
            {
               // rayHit.collider.GetComponent<ShootingAi>().TakeDamage(dammage);
            }
            
            
                Object balle = Instantiate(myBullet, transform.position, transform.rotation);

                Object douille = Instantiate(Shell, myShellposition.transform.position, myShellposition.transform.rotation);
                DestroyObject(balle, 0.5f);
                DestroyObject(douille, 10f);


            
            //ShakeCamera
            Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
            MuzzleEffect = Instantiate(muzzleFlash, attackPoint.position, attackPoint.rotation);

            MuzzleEffect.transform.SetParent(attackPoint);
        }
       
      
        readyToShoot = false;
        bulletsLeft--;
        bulletShots--;
        
        Invoke(nameof(ResetShot), timeBetweeShooting);
        
        if(bulletShots > 0 && HaveBulletsLeft())
            Invoke(nameof(Shoot),timeBetweenShots);
        
        
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished",reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

    private bool HaveBulletsLeft()
    {
        return bulletsLeft > 0;
    }
}
