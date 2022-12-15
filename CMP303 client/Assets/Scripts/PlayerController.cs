using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform camTransform;
    public CharacterController controller;
    public float gravity = -19.0f;
    public float jumpStrength = 9;
    public bool prediction=true;
    private float moveSpeed = 5f;
    private bool[] inputs;
    private float yVelocity = 0;
    public int tester = 31456;
    private void Start()
    {
        gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        moveSpeed *= Time.fixedDeltaTime;
        jumpStrength *= Time.fixedDeltaTime;
        inputs = new bool[5];
    }
    private void FixedUpdate()
    {
        SendInputsToServer();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ClientSend.PlayerShoot(camTransform.forward);
        }
    }
   private void Move(Vector2 inputDir)//used for input prediction to predict where the player will be based off the inputs while letting movement still be server authoritative
    {
        Vector3 moveDirection = transform.right * inputDir.x + transform.forward * inputDir.y;
        moveDirection *= moveSpeed;

        if (controller.isGrounded)
        {
            yVelocity = 0;
            if (inputs[4])
            {
                yVelocity = jumpStrength;
            }
        }
        yVelocity += gravity;
        moveDirection.y = yVelocity;
        controller.Move(moveDirection);
    }
    private void SendInputsToServer()
    {
        bool[] inputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.D),
            Input.GetKey(KeyCode.Space),
        };


        Vector2 moveDirection = Vector2.zero;

        if (inputs[0])
        {
            moveDirection.y += 1;
        }
        if (inputs[1])
        {
            moveDirection.y -= 1;
        }
        if (inputs[2])
        {
            moveDirection.x -= 1;
        }
        if (inputs[3])
        {
            moveDirection.x += 1;
        }
        if (prediction) { //prediction is disabled for 1 frame after recieving a position packet from the server to avoid occasional rubber banding from client charecter controller not being able to update in time and trying to snap the player to the wron possition 
        
            Move(moveDirection);
        }
        else
        {
            prediction = true;
        }
        ClientSend.PlayerMovement(inputs);
    }
}
