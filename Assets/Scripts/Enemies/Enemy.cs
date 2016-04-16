using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{

    enum AttackInstinct
    {
        None = -1,
        Immediate,
        OnScreen,
        Delayed
    }

    [SerializeField, Tooltip("The primary weapon this ship uses")]
    Weapon mainTurret;

    [SerializeField, Tooltip("The ship begins firing from spawn")]
    AttackInstinct attack;

    [SerializeField, Tooltip("Is the ship destroyed when it leaves the frame?")]
    bool destroyWhenInvisible;

    [SerializeField, Tooltip("Default direction to move")]
    Vector3 defaultMoveDirection;

    bool isOnScreen;
    Vector3 initialVelocity;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

	// Use this for initialization
	void Start () {
	    if(attack == AttackInstinct.Immediate && mainTurret != null)
        {
            mainTurret.StartFiring();
        }

        rb.velocity = initialVelocity;
    }
	
	// Update is called once per frame
	void Update () {

        Move();
	}

    protected virtual void Move()
    {
        if (rb == null) return;


        // TODO: Write ship controls and call those instead of affecting the rb directly
        rb.velocity = defaultMoveDirection;

        // TODO: Do more stupid
    }

    public void SetInitialVelocity(Vector3 newV)
    {
        initialVelocity = newV;
    }

    void OnBecameVisible()
    {
        isOnScreen = true;

        if (mainTurret != null && attack == AttackInstinct.OnScreen) mainTurret.StartFiring();
    }

    void OnBecameInvisible()
    {
        if(destroyWhenInvisible) Destroy(this.gameObject);
    }

}
