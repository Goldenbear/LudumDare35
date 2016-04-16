using System.Collections;
using UnityEngine;

/// <summary>
/// A gun that shoots bursts in a predefined pattern
/// </summary>
public class SpreadGun : BurstGun
{
    [SerializeField, Tooltip("Degrees in which to spread an entire burst")]
    protected float spreadDegrees;

    [SerializeField, Tooltip("Direction in which to release the spread")]
    protected bool rotateClockwise;

    public override IEnumerator Fire()
    {
        BurstCheck();

        float halfSpread = -spreadDegrees / 2;
        float spreadPerProjectile = spreadDegrees / (burstNum - 1);

        if (!rotateClockwise)
        {
            halfSpread *= -1f;
            spreadPerProjectile *= -1f;
        }

        Vector3 aimDirection = GetAimDirection(); // This should be the center of our spread
        aimDirection = Quaternion.AngleAxis(halfSpread, Vector3.back) * aimDirection;

        for (int i = 1; i <= burstNum; i++)
        {
            Projectile proj = CreateProjectile();
            proj.SetInitialVelocity(aimDirection.normalized * initialSpeed);
            if (burstTime > 0) yield return new WaitForSeconds(burstTime);

            // Update direction for next firing
            aimDirection = Quaternion.AngleAxis(spreadPerProjectile, Vector3.back) * aimDirection;
        }

        yield break;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(this.transform.position, this.transform.position + GetAimDirection());
    }
}