using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour
{

    [Tooltip("Time between firings")]
    public float FireRate;
    public GameObject ProjectilePrefab;

    float fireTime = 0f;



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
        fireTime += Time.deltaTime;
        if(FireRate > 0f && fireTime > FireRate)
        {
            fireTime = 0f;
            this.StartCoroutine(Fire());
        }
    }

    public abstract IEnumerator Fire();
}

