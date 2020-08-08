using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MyAttributes;

public class EnemyBaseController : MonoBehaviour
{
    [Range(0, 100)]
    public float Health = 100f;

    //[Range(0f, 5f)] public float RatePerSecond;
    public ParticleSystem BloodSplatterParticles;
    
    //README: 
    // Seek depends on gameObject of this script.
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

    [Range(0f, 10f)] public float SeekStopRange = 1f;

    public Rigidbody2D rigidbody2d;

    public float moveInput;

    private bool isPlayerInSeekRange{
        get {
            moveInput = (getPlayerPosition() - getThisEntityPosition()).x;
            if(moveInput > 0) moveInput = 1;
            else if(moveInput < 0) moveInput = -1;
            else moveInput = 0;

            float distance = (Vector2.Distance(getPlayerPosition(), getThisEntityPosition()));
            return distance <= SeekRange && distance >= SeekStopRange;
        }
    }

    public bool isFacingRight = true;

    private void DoMovement(){
        if(isPlayerInSeekRange){
            //Is moving.
            transform.position = new Vector2(transform.position.x + Time.deltaTime * moveInput * MoveSpeed, transform.position.y);

            float valx = PlatformerMovement.Instance.transform.position.x - transform.position.x;

            if(valx > 0) valx = 1;
            else if(valx < 0) valx = -1;

            moveInput = valx;

            if(moveInput != 0) FlipEnemySprite();
        }
        else{
            //Not moving.
        }
    }

    public void FlipEnemySprite()
    {
        if (moveInput > 0)
        {
            //Look to the right
            if(!isFacingRight){
                transform.Rotate(0, 180, 0);

                //animator.SetTrigger("turn");
                isFacingRight = true;
            }

        }
        if (moveInput < 0)
        {
            //Look to the left

            if(isFacingRight){
                transform.Rotate(0, 180, 0);

                //animator.SetTrigger("turn");
                isFacingRight = false;
            }

        }
    }

    #endregion movement

    float entityTime = 0f;

    void Update(){
        if(!isAlive) return;

        DoMovement();

        entityTime += Time.deltaTime;

        if(isPlayerInSeekRange){
            //Move towards player logic.
            
        }

        if (entityTime >= nextMeleeFire)
        {
            if(isPlayerInMeleeAttackRadius() && isMelee){
                nextMeleeFire = entityTime + 1f/MeleeAttackRPS;

                StartCoroutine(StartMeleeAttack());
            }
        }

        if (entityTime >= nextRangedFire)
        {
            if(isPlayerInRangedAttackRadius() && isRanged){
                nextRangedFire = entityTime + 1f/RangedAttackRPS;

                StartCoroutine(StartRangedAttack());
            }
        }
    }

    
    private bool isAlive = true;
    
    [Header("Death Controls")]
    [Range(0, 10f)] public float DeathAnimationDuration = 3;
    public bool destroyAfterDeath = true;

    //Take damage from Player.
    public void OnDamageTaken(float damage, GameObject obj = null)
    {   
        if(obj != null)
            BloodSplatterParticles.transform.position = obj.transform.position;
        else BloodSplatterParticles.transform.position = transform.position;
        
        AudioManager.Instance.Play("Hurt2");
        BloodSplatterParticles.Play();

        Health -= damage;
        if(Health<=0) {
            Health = 0;

            OnDeath();
        }
    }

    private void OnDeath(){
        StartCoroutine(StartDeathSequence());
    }

    IEnumerator StartDeathSequence(){
        //Start Death Animation
        //anim.SetBool();
        isAlive = false;

        yield return new WaitForSeconds(DeathAnimationDuration);

        if(destroyAfterDeath) Destroy(gameObject);
    }

    #region meleeAttack
    [Header("Melee Attack")]

    public bool isMelee;

    [ConditionalField("isMelee")]
    public GameObject meleeSword;
    
    [ConditionalField("isMelee")]
    [Range(0f, 5f)] public float MeleeAttackRPS = 1; //Melee Attack Rate Per Second.

    [ConditionalField("isMelee")]
    [Range(0f, 50f)] public int DamagePerHitMelee = 3;

    [ConditionalField("isMelee")]
    [Range(0f, 10f)] public float MeleeRadius = 1f;

    [ConditionalField("isMelee")]
    [Range(0f, 50f)] public float meleeAttackDuration = 1f;

    [ConditionalField("isMelee")]
    public bool DamageAfterDurationMelee = true;

    [ConditionalField("isMelee")]
    public bool isMeleeAttacking;//TODO: Make private.

    [ConditionalField("isMelee")]
    [Range(0f, 10f)] public float MeleeRange = 1f;

    [ConditionalField("isMelee")]
    private float nextMeleeFire = 0f;

    private bool isPlayerInMeleeAttackRadius(){
        return (Vector2.Distance(getPlayerPosition(), getThisEntityPosition())) <= MeleeRange ;
    }

    IEnumerator StartMeleeAttack(){
        OnStartMeleeAttack();

        yield return new WaitForSeconds(meleeAttackDuration);

        if(isAlive) OnEndMeleeAttack();
    }

    public void OnStartMeleeAttack()
    {   
        print("Enemy: Start Melee Attack");

        //Start Melee animation
        isMeleeAttacking = true;
        
        SwingSword();

        if(!DamageAfterDurationMelee) CauseMeleeDamage();
        //transform.GetChild(1).gameObject.tag = "Untagged";
    }

    public void OnEndMeleeAttack()
    {   
        print("Enemy: End Melee Attack");

        isMeleeAttacking = false;

        if(DamageAfterDurationMelee) CauseMeleeDamage();
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
    [Range(0f, 50f)] public int DamagePerHitRanged = 1;
    
    //[ConditionalField("isRanged")]
    //public bool isRangedAttacking;//TODO: Make private.

    [ConditionalField("isRanged")]
    [Range(0f, 50f)] public float rangedAttackDuration = 1f;

    [ConditionalField("isRanged")]
    public bool ThrowAfterDuration = true;

    [ConditionalField("isRanged")]
    private float nextRangedFire = 0f;

    IEnumerator StartRangedAttack(){
        OnStartRangedAttack();

        yield return new WaitForSeconds(rangedAttackDuration);

        OnEndRangedAttack();
    }
    
    private void SwingSword(){
        //TODO: Start swing sword animation.
    }

    private void CauseMeleeDamage(){
        //TODO: Check if trigger collider is hit on player.

        //TODO: If its true then cause damage.
    }

    public void OnStartRangedAttack()
    {
        if(!ThrowAfterDuration) ThrowProjectile();
    }

    public void OnEndRangedAttack()
    {
        if(ThrowAfterDuration) ThrowProjectile();
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
        
        BulletScript bulletScript = newProjectile.GetComponent<BulletScript>();
        
        bulletScript.SetBullet(DamagePerHitRanged, bulletForce, "Player");

        Rigidbody2D rb_projectile = newProjectile.GetComponent<Rigidbody2D>();
        //rb_projectile.AddForce( firePoint.transform.right * bulletForce, ForceMode2D.Impulse);
        
        Destroy( newProjectile, 5f );
    }

    private bool isPlayerInRangedAttackRadius(){
        return (Vector2.Distance(getPlayerPosition(), getThisEntityPosition())) <= RangeRadius ;
    }

    #endregion rangedAttack

    /*
    #region kamikaze
    [Header("Kamikaze Attack")]

    public bool kamikaze = false;
    
    [ConditionalField("kamikaze")]
    [Range(0f, 50f)] public float kamikazeAttackDamage = 20f;
    //public bool ActivelySeekPlayer;
    
    #endregion kamikaze
    */

    
}
