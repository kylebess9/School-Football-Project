using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {
    private Rigidbody2D ThisBody = null;
    private Transform ThisTransform = null;

    public bool moving = false;
    public float maxSpeed = 5.0f;
    public float Acceleration = 1.0f;
    private void Awake()
    {
        ThisBody = GetComponent<Rigidbody2D>();
        ThisTransform = GetComponent<Transform>();
    }
    // Update is called once per frame
    void Update () {
        float Horz = 0;
        float Vert = 0;
        if (Time.timeScale == 0)
        {
            Horz = 0;
            Vert = 0;
        }
        if (Input.GetKey("up"))
        {
            Vert = .1f;
        }
        if (Input.GetKey("down"))
        {
            Vert = -.1f;
        }
        if (Input.GetKey("left"))
        {
            Horz = -.1f;
        }
        if (Input.GetKey("right"))
        {
            Horz = .1f;
        }
        Vector3 MoveDirection = new Vector3(Horz, Vert);
        if (Input.GetKeyDown("space"))
        {
            GameController.ThisInstance.hike();
        }

        ThisBody.AddForce(MoveDirection.normalized * maxSpeed * Acceleration);

        ThisBody.velocity = new Vector3(Mathf.Clamp(ThisBody.velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(ThisBody.velocity.y, -maxSpeed, maxSpeed));



        //ThisTransform.localRotation = Quaternion.LookRotation(MoveDirection.normalized, Vector3.up);

	}

    public void resetVel()
    {
        ThisBody.velocity = Vector3.zero;
        ThisBody.angularVelocity = 0;
    }
}
