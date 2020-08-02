using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    //For aiming.
    Vector3 mousePos;
    Vector3 lookDir;
    
    //For firing.
    [Range(0, 5)]
    public float rps = 2;
    public GameObject firePoint;
    public GameObject bulletPrefab;
    
    [Range(0, 500)]
    public float bulletForce = 30f;
    
    [Range(0, 15)]
    public float maxRange = 4f;

    private float spriteTime = 0;
    private float nextFire = 0;
    
    private GameObject newProjectile;
    
    private float angle;

    // Update is called once per frame
    void Update()
    {
        //Apply Aiming First.
        mousePos = PlatformerMovement.Instance.transform.position;
        Vector3 objectPos = firePoint.transform.position;//Camera.main.WorldToScreenPoint(transform.position);
        lookDir.x = mousePos.x - objectPos.x;
        lookDir.y = mousePos.y - objectPos.y;
        
        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;        
        
        spriteTime += Time.deltaTime;
        
        if (lookDir.magnitude < maxRange && spriteTime >= nextFire)
        {
            nextFire = 1f/rps;
            spriteTime = 0.0F;
            shoot();
        }

    }
    
    void shoot()
    {
         GameObject newProjectile = Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation) as GameObject;
 
        Rigidbody2D rb_projectile = newProjectile.GetComponent<Rigidbody2D>();
 
        rb_projectile.AddForce(lookDir.normalized * bulletForce);
        
        Destroy(newProjectile, 5f);
    }
    
}