
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
		GameObject obj = Instantiate(ProjectilePrefab, transform.position, transform.rotation) as GameObject;

        Projectile proj = obj.GetComponent<Projectile>();
		proj.FiredFromGun = this;

        if(fireAtEnemy)
        {

        }
        else
        {
            proj.SetInitialVelocity(fireDirection.normalized * initialSpeed);
        }
        

		yield return null;
    }
}