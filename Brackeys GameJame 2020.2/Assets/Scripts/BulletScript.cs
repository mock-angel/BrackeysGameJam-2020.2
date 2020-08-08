using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    public int Damage;
    public string CollisionTag;

    public bool shotFromBoss;

    public void SetBullet(int _damage, float BulletSpeed, string t_collisionTag)
    {
        CollisionTag = t_collisionTag;

        GetComponent<Rigidbody2D>().velocity = transform.right * BulletSpeed;
        Damage = _damage;
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == CollisionTag || collision.tag == "Ground")
        {   
            if(collision.GetComponent<PlatformerMovement>() != null && CollisionTag == "Player")
                collision.GetComponent<PlatformerMovement>().OnDamageTaken(Damage, gameObject);
            
            if(collision.GetComponent<EnemyBaseController>() != null && CollisionTag == "Enemy")
                collision.GetComponent<EnemyBaseController>().OnDamageTaken(Damage, gameObject);
        }

        if(collision.tag == "Boss")
        {
            if(!shotFromBoss)
            {
                collision.GetComponent<Health>().ChangeHealth(-Damage);
            }
        }

        //If not bullet
        if(collision.tag != "Bullet" && collision.tag != "Boss")
        {
            Destroy(gameObject);
        }
    }
}