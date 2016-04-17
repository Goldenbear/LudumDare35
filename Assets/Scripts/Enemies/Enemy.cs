using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(Ship))]
public class Enemy : MonoBehaviour
{
    const float TIME_TO_GET_ON_SCREEN = 10f;

    enum AttackInstinct
    {
        None = -1,
        Immediate,
        OnScreen,
        Delayed
    }

    public enum ExitStrategy
    {
        Top,
        Bottom,
        Left,
        Right,
        Random
    }

    [Serializable]
    public class DeathEvent : UnityEvent<Enemy> { }

    public DeathEvent OnDeath = new DeathEvent();

    [SerializeField, Tooltip("The primary weapon this ship uses")]
    Weapon mainTurret;

    [SerializeField, Tooltip("The ship begins firing from spawn")]
    AttackInstinct attack;

    [SerializeField, Tooltip("Is the ship destroyed when it leaves the frame?")]
    bool destroyWhenInvisible;

    [SerializeField, Tooltip("Default direction to move")]
    Vector3 defaultMoveDirection;

    [SerializeField, Tooltip("True if the enemy doesn't leave until they die")]
    bool isOnScreenUntilDeath;

    [SerializeField, Tooltip("Time to spend on screen before moving on")]
    [Range(3, 10)]
    protected float minScreenTime;

    bool hasBeenOnScreen = false;
    Ship theShip;
    Renderer r;
    Vector3 initialVelocity;
    Coroutine moveRoutine;
    ExitStrategy exit;
    

    void Awake()
    {
        theShip = GetComponent<Ship>();
        r = GetComponent<Renderer>();
        initialVelocity = defaultMoveDirection;
    }

	// Use this for initialization
	void Start ()
    {
        if (attack == AttackInstinct.Immediate && mainTurret != null)
        {
            mainTurret.StartFiring();
        }

        this.StartCoroutine(Move());
    }
    
    // Update is called once per frame
    void Update ()
    {
        TestVisible();
    }

    protected IEnumerator Move()
    {
        if (theShip == null)
        {
            Debug.LogError("Enemy does not have a ship");
            yield break;
        }

        if (!hasBeenOnScreen)
        {
            moveRoutine = this.StartCoroutine(MoveOntoScreen());
        }

        while(!hasBeenOnScreen) yield return null;

        StopMoveRoutine();
        moveRoutine = this.StartCoroutine(MoveOnScreen());

        // Quit here if we never move off screen
        if (isOnScreenUntilDeath) yield break;

        float timeOnScreen = 0;

        while(timeOnScreen < minScreenTime)
        {
            yield return null;
            timeOnScreen += Time.deltaTime;
        }

        StopMoveRoutine();
        moveRoutine = this.StartCoroutine(MoveOffScreen());
        yield break;
    }

    protected virtual IEnumerator MoveOntoScreen()
    {
        theShip.SetVelocity(initialVelocity);

        float timeAlive = 0;
        while(timeAlive < TIME_TO_GET_ON_SCREEN)
        {
            yield return null;
            timeAlive += Time.deltaTime;
        }

        // If we get here we've taken too long, just destroy
        Debug.LogWarning("Enemy has taken too long to get on screen, killing");
        RemoveEnemy();
        yield break;
    }

    protected virtual IEnumerator MoveOnScreen()
    {
        theShip.SetVelocity(initialVelocity * .5f);
        yield break;
    }

    protected virtual IEnumerator MoveOffScreen()
    {
        Vector3 exitVelocity;
        float exitSpeed = initialVelocity.magnitude;

        switch(exit)
        {
            case ExitStrategy.Random:
                exitVelocity = ChooseRandomExitVector(exitSpeed);
                break;
            case ExitStrategy.Top:
                exitVelocity = new Vector3(0, exitSpeed);
                break;
            case ExitStrategy.Bottom:
                exitVelocity = new Vector3(0, -exitSpeed);
                break;
            case ExitStrategy.Right:
                exitVelocity = new Vector3(exitSpeed, 0);
                break;
            case ExitStrategy.Left:
            default:
                exitVelocity = new Vector3(-exitSpeed, 0);
                break;
        }

        theShip.SetVelocity(exitVelocity);
        yield break;
    }

    void StopMoveRoutine()
    {
        if (moveRoutine != null) this.StopCoroutine(moveRoutine);
    }

    public void SetInitialVelocity(Vector3 newV)
    {
        initialVelocity = newV;
    }

    public void SetExitStrategy(ExitStrategy newExit)
    {
        exit = newExit;
    }

    void EnteredScreen()
    {
        hasBeenOnScreen = true;

        if (mainTurret != null && attack == AttackInstinct.OnScreen) mainTurret.StartFiring();
    }

    void LeftScreen()
    {
        if (destroyWhenInvisible) RemoveEnemy();
    }

    void TestVisible()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        bool isVisible = GeometryUtility.TestPlanesAABB(planes, r.bounds);

        if (isVisible && !hasBeenOnScreen)
        {
            EnteredScreen();
        }
        else if(!isVisible && hasBeenOnScreen)
        {
            LeftScreen();
        }
        
    }

    protected void RemoveEnemy()
    {
        OnDeath.Invoke(this);
        Destroy(this.gameObject);
    }

    Vector3 ChooseRandomExitVector(float exitSpeed)
    {
        Vector3 exitVector = theShip.ShipBody.velocity.normalized;

        int degree = UnityEngine.Random.Range(-45, 45);
        exitVector = Quaternion.AngleAxis(degree, Vector3.back) * exitVector * exitSpeed;

        return exitVector;
    }

}
