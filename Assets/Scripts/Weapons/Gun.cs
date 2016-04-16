
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

	public Ship AttachedToShip {get; set;}

	public Vector3 FireDirection
	{
		get { return fireDirection; }
		set { fireDirection = value; }
	}

	public void ProjectileHit(Projectile proj, Ship target)
	{
		// Award points for kill to player that shot the gun ONLY if we didnt shoot ourselves!
		Player player = AttachedToShip as Player;
		if((player != null) && (player != target))
			player.m_score += target.m_points;
	}

    public override IEnumerator Fire()
    {
        Projectile proj = CreateProjectile();
        proj.SetInitialVelocity(GetAimDirection() * initialSpeed);
        yield return null;
    }

    protected Projectile CreateProjectile()
    {
        Projectile p = (Instantiate(ProjectilePrefab, transform.position, transform.rotation) as GameObject).GetComponent<Projectile>();
        p.FiredFromGun = this;
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