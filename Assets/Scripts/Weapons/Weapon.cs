using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour
{

    [Tooltip("Time between firings")]
    public float FireRate;
    public GameObject ProjectilePrefab;

    float fireTime = 0f;

    [SerializeField]
    bool isFiring;

    Coroutine firingCoroutine;

    // Use this for initialization
    void Start()
    {
        if (ProjectilePrefab == null)
        {
            Debug.LogError("Gun does not have a projectile object");
        }
    }

    // Update is called once per frame
    void Update()
    {
		if(isFiring)
		{
	        fireTime += Time.deltaTime;
	        if(FireRate > 0f && fireTime > FireRate)
	        {
	            fireTime = 0f;
                StartFireRoutine();
	        }
		}
    }

    public void StartFiring()
    {
		if(!isFiring)
		{
			
        StartFireRoutine();
        isFiring = true;
		}
    }

    public void StopFiring()
    {
        if(firingCoroutine != null)
        {
            this.StopCoroutine(firingCoroutine);
        }
        isFiring = false;
    }

    private void StartFireRoutine()
    {
        if (firingCoroutine != null) this.StopCoroutine(firingCoroutine);
        firingCoroutine = this.StartCoroutine(Fire());
    }

    public abstract IEnumerator Fire();
}

