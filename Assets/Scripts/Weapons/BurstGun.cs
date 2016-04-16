using System.Collections;
using UnityEngine;

public class BurstGun : Gun
{
    [SerializeField, Tooltip("Time between projectiles in burst")]
    float burstTime;
    [SerializeField, Tooltip("Number of projectiles in each burst")]
    int burstNum;

    public override IEnumerator Fire()
    {
        for(int i = 1; i <= burstNum; i++)
        {
            Projectile proj = Instantiate(ProjectilePrefab).GetComponent<Projectile>();

            if (fireAtEnemy)
            {
                // Find enemy first
            }
            else
            {
                proj.SetInitialVelocity(fireDirection.normalized * initialSpeed);
            }

            yield return new WaitForSeconds(burstTime);
        }

        yield break;
    }
}