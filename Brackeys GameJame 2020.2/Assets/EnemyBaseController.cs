using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MyAttributes;

public class EnemyBaseController : MonoBehaviour
{
    [Range(0,100)]
    public float Health = 100f;

    [Range(0f, 5f)] public float RatePerSecond;
    
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

    #endregion

    #region rangedAttack

    [Header("Ranged Attack")]

    public bool isRanged;

    
    [ConditionalField("isRanged")]
    [Range(0f, 10f)] public float RangeRadius;

    [ConditionalField("isRanged")]
    [Range(0f, 50f)] public float DamagePerHitRanged;
    
    [ConditionalField("isRanged")]
    public bool isRangedAttacking;//TODO: Make private.

    public void OnStartRangedAttack()
    {

    }

    public void OnEndRangedAttack()
    {

    }

    
    private void ThrowProjectile()
    {

    }
    #endregion

    #region kamikaze
    [Header("Kamikaze Attack")]

    public bool kamikaze = false;
    
    [ConditionalField("kamikaze")]
    [Range(0f, 50f)] public float kamikazeAttackDamage = 20f;
    #endregion
}
