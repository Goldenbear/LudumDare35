
using System.Collections;
using UnityEngine;

public class Gun : Weapon
{
    private PlayerAudioManger pam;
    
    [SerializeField, Tooltip("Initial speed of the projectile")]
    protected float initialSpeed;
    
    [SerializeField, Tooltip("Whether or not we aim at a user")]
    protected bool fireAtEnemy;
    [SerializeField, Tooltip("Our default shooting direction")]
    protected Vector3 fireDirection;

	public Ship AttachedToShip {get; set;}
    private Ship targetPlayer;

	public Vector3 FireDirection
	{
		get { return fireDirection; }
		set { fireDirection = value; }
	}

    void Start()
    {
        if (AttachedToShip is Player)
        {
            pam = AttachedToShip.Pam;
        }

        if (fireAtEnemy)
        {
            AcquireNewTarget();
        }
    }

    public void ProjectileHit(Projectile proj, Ship target)
	{
		// Hit target
		target.Hit(15);

		// Award points for kill to player that shot the gun ONLY if we didnt shoot ourselves!
		Player player = AttachedToShip as Player;
		if((player != null) && (player != target))
			player.m_score += target.m_points;
	}

    public override IEnumerator Fire()
    {
        Projectile proj = CreateProjectile();
        if(proj != null)
        {
            proj.SetInitialVelocity(GetAimDirection() * initialSpeed);
        }
        
        yield return null;
    }

    protected void AcquireNewTarget()
    {
        Player[] targets = LevelManager.Get.Players;

        if(targets.Length > 0)
        {
            int idx = Random.Range(0,targets.Length);
            targetPlayer = targets[idx];
        }
        else
        {
            Debug.LogWarning("No targets to choose from");
        }
    }

    protected Projectile CreateProjectile()
    {
        Projectile p = null;
        if(IsPointVisible(transform.position))
        {
            p = (Instantiate(ProjectilePrefab, transform.position, transform.rotation) as GameObject).GetComponent<Projectile>();
            p.FiredFromGun = this;

            if (AttachedToShip is Player)
            {
                //Debug.Log("Pew Pew");
                pam.PlayBulletSound();
            }
        }

        return p;
    }

    protected Vector3 GetAimDirection()
    {
        if(fireAtEnemy && targetPlayer != null)
        {
            return (targetPlayer.transform.position - this.transform.position).normalized;
        }
        else
        {
            return fireDirection.normalized;
        }
    }

    bool IsPointVisible(Vector3 point)
    {
        Bounds b = new Bounds(point, new Vector3(.1f, .1f, .1f));
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        return GeometryUtility.TestPlanesAABB(planes, b);
    }
}