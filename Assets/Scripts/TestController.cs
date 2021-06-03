using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestController : MonoBehaviour
{
    // This variable was made to attempt logic for held-down movement
    // Vector3 newTargetPosition;
    
    Vector3 targetPosition;
    Vector3 lookAtTarget;
    Quaternion playerRot;
    float rotSpeed = 5; // may need to be adjusted or removed. do we want turn radius?
    float speed = 10; //placeholder, will likely need to be variable for different heroes
    bool moving = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            SetTargetPosition();

        if (moving)
            Move();
    }

    void SetTargetPosition()
    {
        //Rays translate the mouse clicking on the monitor to a point in the game
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            // Set targetPosition to Ray's point
            targetPosition = hit.point;

            //The hero rotates towards targetPosition
            lookAtTarget = new Vector3(targetPosition.x - transform.position.x,
                transform.position.y,
                targetPosition.z - transform.position.z);
            playerRot = Quaternion.LookRotation(lookAtTarget);

            //moving is set to true so that the Move function will run
            moving = true;
        }
    }

    void Move()
    {
        //The hero moves to targetPosition
        transform.rotation = Quaternion.Slerp(transform.rotation,
                                                playerRot,
                                                rotSpeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position,
                                                targetPosition,
                                                speed * Time.deltaTime);
        if (transform.position == targetPosition)
            //The hero stops moving once it reaches its destinatation
            moving = false;
    }
}
