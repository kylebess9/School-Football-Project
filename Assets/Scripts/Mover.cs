using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float maxSpeed = 2;
    public float yOffSet = .1f;
    public float acceleration = 2f;
    public int jumpTime = 2;
    public float speedInc = 8.0f;
    public float range = 1f;

    private Rigidbody2D thisBody = null;
    private Transform thisTransform = null;
    private Vector3 target = new Vector3(-10,-10);
    public string role = "BLOCK";
    public string type = "LINE";
    public GameObject Player = null;

    public float ReactionTime = 2f;
    private void Awake()
    {
        thisBody = gameObject.GetComponent<Rigidbody2D>();
        thisTransform = gameObject.GetComponent<Transform>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        InvokeRepeating("FindPlayer", 0, ReactionTime);
    }

    private void FindPlayer()
    {
        target = Player.GetComponent<Transform>().position;
    }

    private void FixedUpdate()
    {
        //This is the lineman AI
        //Try to contain the player, and then chase if needed
        if (type == "LINE")
        {
            //Set role
            if (Player.GetComponent<Transform>().position.x < thisTransform.position.x)
            {
                role = "BLOCK";
            }
            else
            {
                role = "CHASE";
            } 
        }
        //This is the lineback aggressive AI
        //The goal for this enemy will be to 'spy' (AKA, line the player up) and then attack when player is close
        if(type == "AGGRO")
        {
            if (Player.GetComponent<Transform>().position.x < thisTransform.position.x - range && role != "CHASE")
            {
                role = "BLOCK";
            }
            else
            {
                
                role = "CHASE";
            }
        }


        //Depending on the current role of the opponent, it'll move differently
        if (role == "BLOCK")
        {
            target = new Vector3(thisTransform.position.x, target.y + yOffSet);
            thisBody.AddForce((target - thisTransform.position) * maxSpeed * acceleration);
        }
        else if (role == "CHASE")
        {
            target = new Vector3(target.x, target.y);
            thisBody.AddForce((target - thisTransform.position) * maxSpeed * acceleration);
        }
        //clamp the velocity
        thisBody.velocity = new Vector3(Mathf.Clamp(thisBody.velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(thisBody.velocity.y, -maxSpeed, maxSpeed));

    }
}
