using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GauntletWielderNPC : EnemyAi
{
    private Vector3 newProjectileSpawnPoint;
    public override void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!getAlreadyAttacked())
        {
            ///Attack code here
            newProjectileSpawnPoint = new Vector3(transform.position.x + (agent.transform.forward.x * getAttackDisplaceDist()),
                                                transform.position.y + (agent.transform.forward.y * getAttackDisplaceDist()),
                                                transform.position.z + (agent.transform.forward.z * getAttackDisplaceDist()));

            GameObject hookAndChain = Instantiate(projectile, newProjectileSpawnPoint, Quaternion.identity);
            hookAndChain.transform.SetParent(null);
            hookAndChain.transform.rotation = transform.rotation;
            getAlreadyAttacked(true);
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
}
