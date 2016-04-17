using UnityEngine;
using System;
using System.Collections;


/// <summary>
/// Abstract base class for all ships.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Ship : MonoBehaviour
{
    // What ship shape is a shape shifting ship if the ship's shape keeps on shifting
    public enum ShipShape
    {
        k_square	= 0,
        k_circle	= 1,
        k_triangle	= 2,
        k_cross		= 3
    }

	// Public members
	public int m_health = 100;
	public ShipShape m_oldShape = ShipShape.k_square;
	public ShipShape m_currentShape = ShipShape.k_square;
	public int m_points = 100;
	private Gun m_gun;

	public Rigidbody ShipBody {get{ return m_rigidbody; }}
	// Manually assign this guy
	public Animator anim;

	// Take a hit
	public void Hit(int damage=1)
	{
		m_health -= damage;
		if(m_health <= 0)
		{
			// Die
		}
	}

    // Private members
    private Rigidbody m_rigidbody;
	private GameObject[] m_shapes;


    protected virtual void Start()
	{
		// Find the shape gameobjects
		m_shapes = new GameObject[Enum.GetValues(typeof(ShipShape)).Length];
		foreach(ShipShape shape in Enum.GetValues(typeof(ShipShape)))
		{
			string shapeName = shape.ToString().Substring(2);
			shapeName = "PlayerAnimNode/"+Char.ToUpper(shapeName[0])+shapeName.Substring(1).ToLower();
			Transform shapeT = transform.Find(shapeName);
			if(shapeT != null)
				m_shapes[(int)shape] = shapeT.gameObject;
		}
    }
		
	// Face ship in this direction
	protected void FaceDirection(Vector3 desiredDir, float turnSpeed=-1)
	{
		Quaternion qCurrent = transform.rotation;
		Quaternion qDesired = Quaternion.LookRotation(Vector3.forward, desiredDir);

		if(turnSpeed > 0.0f)
			transform.rotation = Quaternion.RotateTowards(qCurrent, qDesired, turnSpeed * Time.deltaTime);
		else
			transform.rotation = qDesired;
	}

    public void SetVelocity(Vector3 newVelocity)
    {
        m_rigidbody.velocity = newVelocity;
		FaceDirection(newVelocity.normalized);
    }

    public void Move(Vector3 horizontalForce, Vector3 verticalForce)
    {
        Vector3 totalForce = horizontalForce + verticalForce;
        m_rigidbody.AddForce(totalForce);
    }

	public void ShapeShift(ShipShape newShape)
	{
		// If already this shape do nothing
		if(newShape == m_currentShape)
			return;
		
		// If a shift is already pending dont trigger another one
		if(m_currentShape != m_oldShape)
			return;
		
		m_oldShape = m_currentShape;
		m_currentShape = newShape;
		anim.SetTrigger("changeShape");
	}

    // Change ship shape
	public void ShapeShiftNow()
	{
		// Deactivate previous shape and activate new shape
		m_shapes[(int)m_oldShape].SetActive(false);
		m_shapes[(int)m_currentShape].SetActive(true);

		// Set old shape to current to signify we have shifted
		m_oldShape = m_currentShape;

		// Get new active gun
		m_gun = GetComponentInChildren<Gun>();
		m_gun.AttachedToShip = this;
	}

	// Fire a bullet in the given direction
	public void Fire(Vector3 dir)
	{
        m_gun.StartFiring();
		m_gun.FireDirection = dir;
	}

	// Stop firing bullets
	public void StopFiring()
	{
        m_gun.StopFiring();
	}

	// Private members

	// Constructor
	void Awake() 
	{
		// Get active gun
		m_gun = GetComponentInChildren<Gun>();
		m_gun.AttachedToShip = this;

        m_rigidbody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () 
	{
	}
}
