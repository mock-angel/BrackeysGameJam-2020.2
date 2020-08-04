using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    public int Damage;
    public string CollisionTag;

    public void SetBullet(int _damage, float BulletSpeed, string t_collisionTag)
    {
        CollisionTag = t_collisionTag;

        GetComponent<Rigidbody2D>().velocity = transform.right * BulletSpeed;
        Damage = _damage;
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        
        if(collision.tag == CollisionTag || collision.tag == "Ground")
        {   
            if(collision.GetComponent<PlatformerMovement>() != null && CollisionTag == "Player")
                collision.GetComponent<PlatformerMovement>().OnDamageTaken(Damage);
            
            if(collision.GetComponent<EnemyBaseController>() != null && CollisionTag == "Enemy")
                collision.GetComponent<EnemyBaseController>().OnDamageTaken(Damage);
        }
        Destroy(gameObject);
    }
}