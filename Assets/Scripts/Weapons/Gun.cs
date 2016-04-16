
using System;
using System.Collections;
using UnityEngine;

public class Gun : Weapon
{
    [SerializeField, Tooltip("Initial speed of the projectile")]
    protected float initialSpeed;
    
    [SerializeField, Tooltip("Whether or not we aim at a user")]
    protected bool fireAtEnemy;
    [SerializeField, Tooltip("Our default shooting direction")]
    protected Vector3 fireDirection;


    public override IEnumerator Fire()
    {
        Projectile proj = CreateProjectile();
        proj.SetInitialVelocity(GetAimDirection() * initialSpeed);
        yield return null;
    }

    protected Projectile CreateProjectile()
    {
        Projectile p = Instantiate(ProjectilePrefab).GetComponent<Projectile>();
        p.transform.position = this.transform.position;
        return p;
    }

    protected Vector3 GetAimDirection()
    {
        if(fireAtEnemy)
        {
            // TODO: Find a target
            return new Vector3(1, 0, 0);
        }
        else
        {
            return fireDirection.normalized;
        }
    }
}