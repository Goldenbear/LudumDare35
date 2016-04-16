using System.Collections;
using UnityEngine;

public class BurstGun : Gun
{
    [SerializeField, Tooltip("Time between projectiles in burst")]
    protected float burstTime;
    [SerializeField, Tooltip("Number of projectiles in each burst")]
    protected int burstNum;

    public override IEnumerator Fire()
    {
        BurstCheck();

        for(int i = 1; i <= burstNum; i++)
        {
            Projectile proj = CreateProjectile();

            proj.SetInitialVelocity(GetAimDirection() * initialSpeed);

            yield return new WaitForSeconds(burstTime);
        }

        yield break;
    }

    protected bool BurstCheck()
    {
        if(FireRate == 0 || FireRate > (burstNum * burstTime))
        {
            return true;
        }
        else
        {
            Debug.LogWarningFormat("Fire rate will start another round before the last one completes:/nFireTime:{0} BurstTime:{1}", FireRate, (burstNum * burstTime));
            return false;
        }
    }
}