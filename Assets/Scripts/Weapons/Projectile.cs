using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour {

    public enum ProjectileType
    {
        None = -1,
        Square,
        Circle,
        Triangle,
        Cross,
        Nether // Hurts all ship types
    }

    public float LifeTime;
    public float Damage;
    
    [SerializeField, Tooltip("The type of projectile this is.")]
    ProjectileType type;

    float elapsedLife;
    Vector3 initialVelocity;

    Rigidbody rb;
    Renderer r;

	public Gun FiredFromGun {get; set;}		// Who shot this projectile so we can notify them of hits


	// Use this for initialization
	void Awake () {
        rb = GetComponent<Rigidbody>();
        r = GetComponentInChildren<Renderer>();
	}

    void Start()
    {
        rb.velocity = initialVelocity;
    }
	
	// Update is called once per frame
	void Update () {
        elapsedLife += Time.deltaTime;
        if(elapsedLife >= LifeTime)
        {
            Destroy(this.gameObject);
        }
	}

    public void SetInitialVelocity(Vector3 newV)
    {
        initialVelocity = newV;
    }

    public virtual void Move()
    {
        // Nothing in the base for now
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        // If projectile hit a ship then notify the gun that fired us
        Ship otherShip = otherCollider.gameObject.GetComponent<Ship>();

        if (otherShip != null && FiredFromGun != null && FiredFromGun.AttachedToShip != null)
        {
            Ship originShip = FiredFromGun.AttachedToShip;

            // Enemies don't shoot each other
            if (!(otherShip is Player) && !(originShip is Player))
                return;

            // Don't hit yourself stupid
            if (otherShip == originShip)
                return;

            if (DoesProjectileHurtShip(otherShip.m_currentShape, this.type))
            {
                // WE MADE IT
                //if (otherShip is Player) Debug.Log("Hit player with " + this.type);
                //if (FiredFromGun.AttachedToShip is Player) Debug.Log("Player hit " + otherShip.m_currentShape);
                FiredFromGun.ProjectileHit(this, otherShip);

                // Once a projectile hits something it should disappear
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// The function that determines the rules of hurting each other!
    /// </summary>
    /// <param name="ship">The ship's shape</param>
    /// <param name="proj">The projectile's type</param>
    /// <returns></returns>
    bool DoesProjectileHurtShip(Ship.ShipShape ship, Projectile.ProjectileType proj)
    {
        if ((int)ship != (int)proj) return true;

        return false;
    }

    void OnBecameInvisible()
    {
        // TODO: Why doesn't this work?
        Destroy(this.gameObject);
    }
}
