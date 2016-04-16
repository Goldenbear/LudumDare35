
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

	public Vector3 FireDirection
	{
		get { return fireDirection; }
		set { fireDirection = value; }
	}

    public override IEnumerator Fire()
    {
		GameObject obj = Instantiate(ProjectilePrefab, transform.position, transform.rotation) as GameObject;

        Projectile proj = obj.GetComponent<Projectile>();

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