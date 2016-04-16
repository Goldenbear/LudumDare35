using UnityEngine;
using System.Collections;

/// <summary>
/// Abstract base class for all ships.
/// </summary>
public abstract class Ship : MonoBehaviour 
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

	// Change ship shape
	public void ShapeShift(ShipShape newShape)
	{
		m_currentShape = newShape;
	}

	// Private members

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
}
