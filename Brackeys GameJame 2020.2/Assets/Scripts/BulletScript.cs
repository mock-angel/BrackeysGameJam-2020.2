using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    public int Damage;
    public void SetBullet(int _damage, float BulletSpeed)
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * BulletSpeed;
        Damage = _damage;
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        
        if(collision.tag == "Player")
        {   /*
            if(collision.GetComponent<PlayerMovement>() != null)
                collision.GetComponent<PlayerMovement>().OnDamageTaken(Damage);
            */
        }
        Destroy(gameObject);
    }
}