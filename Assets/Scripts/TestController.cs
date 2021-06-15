using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestController : MonoBehaviour
{


    public NavMeshAgent playerNavMeshAgent;

    public Camera playerCamera;

    private CharacterStats characterStats;

    //These are for HealthBar


    public HealthBar healthBar;

    // These are for movement
    Vector3 targetPosition;
    Vector3 lookAtTarget;
    Quaternion playerRot;
    

    // Start is called before the first frame update
    void Start()
    {
        // begin the game with full health
        characterStats = GetComponent<CharacterStats>();
        healthBar.SetMaxHealth(characterStats.maxHealth);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButton(1))
        {
            Ray myRay = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
        
            if(Physics.Raycast(myRay,out hit))
            {
                playerNavMeshAgent.SetDestination(hit.point);
            }
        }

        healthBar.SetHealth(characterStats.currentHealth);

    }

}
