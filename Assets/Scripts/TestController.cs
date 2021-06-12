using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestController : MonoBehaviour
{


    public NavMeshAgent playerNavMeshAgent;

    public Camera playerCamera;



    //These are for HealthBar
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    // These are for movement
    Vector3 targetPosition;
    Vector3 lookAtTarget;
    Quaternion playerRot;
    float rotSpeed = 5; // may need to be adjusted or removed. do we want turn radius?
    float speed = 10; //placeholder, will likely need to be variable for different heroes
    bool moving = false;

    // Start is called before the first frame update
    void Start()
    {
        // begin the game with full health
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
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
        
        
        //Debug for taking damage
        // /*
        if (Input.GetKeyDown(KeyCode.Space))
            TakeDamage(20);
     
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }
}
