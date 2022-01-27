using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followMe : MonoBehaviour {

    //initialize variables
    public float interpVelocity;
    public float minDistance;
    public float followDistance;
    public GameObject target;
    public Vector3 offset;
    Vector3 targetPos;


    void Start()
    {
        //set your starting position
        targetPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //see if player still exists
        if (target)
        {
            //get position
            Vector3 posNoZ = transform.position;

            //get the z position
            posNoZ.z = target.transform.position.z;

            //find out which direction the player moved if any
            Vector3 targetDirection = (target.transform.position - posNoZ);

            //find out how fast the camera moves
            interpVelocity = targetDirection.magnitude * 5f;

            //move 
            targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, targetPos + offset, 0.25f);

        }
    }
}
