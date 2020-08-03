using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MyAttributes;

public class EnemyBaseController : MonoBehaviour
{
    [Range(0,100)]
    public float Health = 100f;

    //[Range(0f, 5f)] public float RatePerSecond;

    
    //README: 
    // Seek depends on gameObject of this script.
    // Either place the 
    #region universalMethods

    private Vector2 getPlayerPosition(){
        return PlatformerMovement.Instance.gameObject.transform.position;
    }

    private Vector2 getThisEntityPosition(){
        return gameObject.transform.position;
    }

    #endregion universalMethods

    #region movement
    [Range(0f, 10f)] public float MoveSpeed = 5f;
    
    [Range(0f, 10f)] public float SeekRange = 10f;

    private bool isPlayerInSeekRange{
        get {
            return (Vector2.Distance(getPlayerPosition(), getThisEntityPosition())) >= SeekRange ;
        }
    }

    #endregion

    float entityTime = 0f;

    void Update(){
        entityTime += Time.deltaTime;

        if(isPlayerInSeekRange){
            //Move towards player logic.
        }

        //if(isPlayerInRangedAttackRadius() && isRanged){
        //    StartCoroutine(StartRangedAttack());
        //}
        
        if (entityTime >= nextRangedFire)
        {
            if(isPlayerInRangedAttackRadius() && isRanged){
                nextRangedFire = entityTime + 1f/RangedAttackRPS;

                StartCoroutine(StartRangedAttack());
            }
        }

        
    }

    //Take damage from Player.
    public void OnDamageTaken(float damage)
    {
        Health -= damage;
    }

    #region meleeAttack
    [Header("Melee Attack")]

    public bool isMelee;

    [ConditionalField("isMelee")]
    [Range(0f, 50f)] public float DamagePerHitMelee;

    [ConditionalField("isMelee")]
    public bool isMeleeAttacking;//TODO: Make private.

    public void OnStartMeleeAttack()
    {
        isMeleeAttacking = true;
        //transform.GetChild(1).gameObject.tag = "Untagged";
    }

    public void OnEndMeleeAttack()
    {
        isMeleeAttacking = false;
        //transform.GetChild(1).gameObject.tag = "Enemy";
    }

    #endregion meleeAttack

    #region rangedAttack

    [Header("Ranged Attack")]

    public bool isRanged;

    [ConditionalField("isRanged")]
    public GameObject bulletPrefab;

    [ConditionalField("isRanged")]
    public GameObject firePoint;

    [ConditionalField("isRanged")]
    public GameObject gun;
    
    [ConditionalField("isRanged")]
    [Range(0f, 5f)] public float RangedAttackRPS = 1; //Ranged Attack Rate Per Second.

    [ConditionalField("isRanged")]
    [Range(0f, 50f)] public float bulletForce = 30f;

    [ConditionalField("isRanged")]
    [Range(0f, 10f)] public float RangeRadius = 5f;

    [ConditionalField("isRanged")]
    [Range(0f, 50f)] public float DamagePerHitRanged = 10;
    
    [ConditionalField("isRanged")]
    public bool isRangedAttacking;//TODO: Make private.

    [ConditionalField("isRanged")]
    [Range(0f, 50f)] public float rangedAttackDuration = 1f;

    [ConditionalField("isRanged")]
    public bool throwAfterDuration = false;


    private float nextRangedFire = 0f;

    IEnumerator StartRangedAttack(){
        OnStartRangedAttack();

        yield return new WaitForSeconds(rangedAttackDuration);

        OnEndRangedAttack();
    }

    public void OnStartRangedAttack()
    {
        if(!throwAfterDuration) ThrowProjectile();
    }

    public void OnEndRangedAttack()
    {
        if(throwAfterDuration) ThrowProjectile();
    }

    
    private void ThrowProjectile()
    {   
        #region rotateGun

        Vector2 playerPos = getPlayerPosition();
        Vector2 thisEntityPos = getThisEntityPosition();

        Vector2 lookDir;

        lookDir.x = playerPos.x - thisEntityPos.x;
        lookDir.y = playerPos.y - thisEntityPos.y;

        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        gun.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        #endregion rotateGun

        Shoot();
    }
    
    void Shoot()
    {
        GameObject newProjectile = Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);
        
        Rigidbody2D rb_projectile = newProjectile.GetComponent<Rigidbody2D>();
        rb_projectile.AddForce( firePoint.transform.right * bulletForce, ForceMode2D.Impulse);
        
        Destroy(newProjectile, 5f);
    }

    private bool isPlayerInRangedAttackRadius(){
        return (Vector2.Distance(getPlayerPosition(), getThisEntityPosition())) >= RangeRadius ;
    }

    #endregion rangedAttack

    #region kamikaze
    [Header("Kamikaze Attack")]

    public bool kamikaze = false;
    
    [ConditionalField("kamikaze")]
    [Range(0f, 50f)] public float kamikazeAttackDamage = 20f;
    //public bool ActivelySeekPlayer;
    
    #endregion kamikaze
}
