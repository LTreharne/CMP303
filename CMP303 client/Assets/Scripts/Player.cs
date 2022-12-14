//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


//public class Player : MonoBehaviour
//{

//    public int id;
//    public string username;
//    public CharacterController controller;
//    public Transform shootOrigin;
//    public float gravity = -19.0f;
//    public float jumpStrength = 9;

//    private float moveSpeed = 5f;
//    private bool[] inputs;
//    private float yVelocity = 0;
//    public float health;
//    public float maxHealth = 100.0f;
//    public float playerDamage = 35.0f;

//    private void Start()
//    {
//        gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
//        moveSpeed *= Time.fixedDeltaTime;
//        jumpStrength *= Time.fixedDeltaTime;
//    }
//    public void Initialize(int ID, string name)
//    {
//        id = ID;
//        username = name;
//        health = maxHealth;


//        inputs = new bool[5];

//    }
//    public void FixedUpdate()
//    {
//        if (health <= 0)
//        {
//            return;
//        }

//        if (transform.position.y <= -10)
//        {
//            TakeDamage(maxHealth);
//        }
//        Vector2 moveDirection = Vector2.zero;

//        if (inputs[0])
//        {
//            moveDirection.y += 1;
//        }
//        if (inputs[1])
//        {
//            moveDirection.y -= 1;
//        }
//        if (inputs[2])
//        {
//            moveDirection.x -= 1;
//        }
//        if (inputs[3])
//        {
//            moveDirection.x += 1;
//        }

//        Move(moveDirection);
//    }

//    private void Move(Vector2 inputDir)
//    {
        

//        Vector3 moveDirection = transform.right * inputDir.x + transform.forward * inputDir.y;
//        moveDirection *= moveSpeed;

//        if (controller.isGrounded)
//        {
//            yVelocity = 0;
//            if (inputs[4])
//            {
//                yVelocity = jumpStrength;
//            }
//        }
//        yVelocity += gravity;
//        moveDirection.y = yVelocity;
//        controller.Move(moveDirection);
//    }

//}
