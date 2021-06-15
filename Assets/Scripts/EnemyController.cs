using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    public float lookRadius = 10f;

    Transform target;
    NavMeshAgent agent;
    CharacterCombat combat;

    // Start is called before the first frame update
    void Start()
    {
        //determine which object is the player
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        combat = GetComponent<CharacterCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        //determines the distance from the player.
        float distance = Vector3.Distance(target.position, transform.position);

        if(distance <= lookRadius)
        {
            //Moves the enemy towards the player
            agent.SetDestination(target.position);
            //rotates the enemy towards the player
            if (distance <= agent.stoppingDistance)
            {

                CharacterStats targetStats = target.GetComponent<CharacterStats>();
                
                if (targetStats != null)
                {
                    combat.Attack(targetStats);
                    
                }
               
                FaceTarget();
            }
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation,lookRotation,Time.deltaTime *5f);
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }


}
