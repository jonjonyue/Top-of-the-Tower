using UnityEngine;
using System.Collections;

public class SkeletonSpawn : Skeleton
{
	protected override void Start()
	{
        base.Start();
	}

    public override void Move()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("SpawnSkeleton"))
        {
            Vector3 targetPosition = heroTransform.position;
            Vector3 targetVector = targetPosition - transform.position;

            agent.SetDestination(heroTransform.position);

            var enemyAngle = Mathf.Atan2(targetVector.z, targetVector.x) * Mathf.Rad2Deg;

            if (enemyAngle < 0.0f)
                enemyAngle += 360;

            if (enemyAngle >= 315f || enemyAngle < 45f)
            { /* Right */
                sr.flipX = false;
                anim.SetBool("idle", false);
                anim.SetInteger("direction", 1);
            }
            else if (enemyAngle >= 135f && enemyAngle < 225f)
            { /* Left */
                sr.flipX = true;
                anim.SetBool("idle", false);
                anim.SetInteger("direction", 1);
            }
            else if (enemyAngle >= 45f && enemyAngle < 135f)
            { /* Up */
                anim.SetBool("idle", false);
                anim.SetInteger("direction", 3);
            }
            else if (enemyAngle >= 225f && enemyAngle < 315f)
            { /* Down */
                anim.SetBool("idle", false);
                anim.SetInteger("direction", 0);
            }
        }
    }
}
