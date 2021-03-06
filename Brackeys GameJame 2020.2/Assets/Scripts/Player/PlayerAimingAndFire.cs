﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimingAndFire : MonoBehaviour
{   
    public static PlayerAimingAndFire Instance { get; private set; }

    //For aiming.
    Vector3 mousePos;
    Vector3 lookDir;
    
    //For firing.
    public float rps = 2;
    public GameObject firePoint;
    public GameObject bulletPrefab;
    
    public float bulletForce = 30f;
    
    private float spriteTime = 0;
    private float nextFire = 0;
    
    private GameObject newProjectile;
    public GameObject gun;
    public GameObject laserGun;
    
    public int DamagePerSoot = 5;

    public bool canShoot = true;

    public bool canShootLaser = false;

    public LineRenderer lineRenderer;

    public void Awake(){
        Instance = this;
    }

    public void Start(){
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //Apply Aiming First.
        mousePos = Input.mousePosition;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        lookDir.x = mousePos.x - objectPos.x;
        lookDir.y = mousePos.y - objectPos.y;
        
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        //gun.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        
        //
        spriteTime = spriteTime + Time.deltaTime;
        
        
        if (Input.GetButton("Fire1"))
        {   if(canShoot){
                if (spriteTime >= nextFire)
                {
                    nextFire = 1f/rps;
                    spriteTime = 0.0F;

                    //if(canShootLaser) ShootLaser();
                    //else {
                    //    StopShootLaser();
                        shoot();
                    //}
                }
            }
            else{
                
            }
        }else StopShootLaser();
    }
    
    void shoot()
    {   
        AudioManager.Instance.Play("Bullet");

        newProjectile = Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);
        
        BulletScript bulletScript = newProjectile.GetComponent<BulletScript>();
        bulletScript.SetBullet(DamagePerSoot, bulletForce, "Enemy");

        Rigidbody2D rb_projectile = newProjectile.GetComponent<Rigidbody2D>();
        rb_projectile.AddForce( firePoint.transform.right * bulletForce, ForceMode2D.Impulse);
        
        Destroy(newProjectile, 5f);
    }

    public GameObject impactEffect;

    public void ShootLaser()
    {  
        RaycastHit2D hitInfo = Physics2D.Raycast(laserGun.transform.position, laserGun.transform.right, LayerMask.GetMask("Player", "Enemy", "Ground"));

        if(hitInfo){
            EnemyBaseController enemy = hitInfo.transform.GetComponent<EnemyBaseController>();
            if(enemy != null){
                enemy.OnDamageTaken(DamagePerSoot);
            }

            GameObject obj = Instantiate(impactEffect, hitInfo.point, Quaternion.identity);
            Destroy(obj, .2f);

            LineRenderer lRenderer = obj.GetComponent<LineRenderer>();
            
            lRenderer.SetPosition(0, laserGun.transform.position);
            lRenderer.SetPosition(1, hitInfo.point);
        }
        else {

            GameObject obj = Instantiate(impactEffect, hitInfo.point, Quaternion.identity);
            Destroy(obj, .2f);

            LineRenderer lRenderer = obj.GetComponent<LineRenderer>();

            lRenderer.SetPosition(0, laserGun.transform.position);
            lRenderer.SetPosition(1, laserGun.transform.position + laserGun.transform.right * 100);
        }
        AudioManager.Instance.Play("Laser");
    }

    public void StopShootLaser()
    {  
        AudioManager.Instance.Stop("Laser");
    }

}
