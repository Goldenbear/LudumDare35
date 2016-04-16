using UnityEngine;
using System.Collections;

public enum ColourType
{
    None = -1,
    Blue,
    Yellow,
    Red,
    Green,
    Purple
}

public enum ShapeType
{
    None = -1,
    Square,
    Triangle,
    Circle,
    Cross,
    Nether
}

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour {

    public float LifeTime;
    public float Damage;
    
    ColourType type;
    float elapsedLife;
    Vector3 initialVelocity;

    Rigidbody rb;
    //new Collider collider;

	public Gun FiredFromGun {get; set;}		// Who shot this projectile so we can notify them of hits


	// Use this for initialization
	void Awake () {
        rb = GetComponent<Rigidbody>();
        //collider = GetComponent<Collider>();
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

		if((FiredFromGun != null) && (otherShip != null))
		{
			FiredFromGun.ProjectileHit(this, otherShip);
		}
	}
}
