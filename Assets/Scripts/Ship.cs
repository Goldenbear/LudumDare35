using UnityEngine;
using System.Collections;

/// <summary>
/// Abstract base class for all ships.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Ship : MonoBehaviour 
{
	// What ship shape is a shape shifting ship if the ship's shape shifts
	public enum ShipShape
	{
		k_square,
		k_circle,
		k_triangle,
		k_cross
	}

	// Public members
	public ShipShape m_currentShape = ShipShape.k_square;
	public int m_points = 100;
	private Gun m_gun;

    // Private members
    private Rigidbody m_rigidbody;

    protected virtual void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 horizontalForce, Vector3 verticalForce)
    {
        Vector3 totalForce = horizontalForce + verticalForce;
        m_rigidbody.AddForce(totalForce);
    }

    // Change ship shape
    public void ShapeShift(ShipShape newShape)
	{
		m_currentShape = newShape;

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
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
}
