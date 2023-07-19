using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody rb;
    public CharacterController characterController;
    public Camera playercam;
    public float moveSpeed = 1;
    public float jumpHeight = 1;
    public float sensitivity = 1;
    public float gravity = 1;
    public float maxSpeed = 3f;

    private void Start() {
        playercam.gameObject.SetActive(true);
    }
    private Vector2 mouseInput;
    private bool canJump = false;
    private bool jumped = false;
    public bool isGrounded = false;
    Vector3 moveDirection;
    public float h = 0;
    public float v = 0;
    public int counter = 0;
    private void FixedUpdate() {
        //input and x/z movement
        

        Vector3 movement = new Vector3(h, 0, v).normalized;
        Vector3 transformMovement = transform.TransformDirection(movement) * moveSpeed;
        Vector3 flatMovement = moveSpeed * Time.fixedDeltaTime * transformMovement;
        moveDirection = new Vector3(flatMovement.x, moveDirection.y, flatMovement.z);

        //   jump / gravity
        canJump = characterController.isGrounded;

        
        if (jumped){
            moveDirection.y = jumpHeight;
            jumped = false;
            canJump = false;
        }else if(canJump){
            moveDirection.y = 0f;
        }else{
            moveDirection.y -= gravity * Time.fixedDeltaTime;
        }
        characterController.Move(moveDirection);
        Debug.Log("fixed: " + counter);
        counter++;
        
    }
    private void Update() {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        if (Input.GetAxisRaw("Jump") > 0 && canJump)jumped = true;
        //camera
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        moveCamera();
    }
    
    private float xRotation;
    private void moveCamera(){
        xRotation -= mouseInput.y * sensitivity;
        transform.Rotate(0f, mouseInput.x * sensitivity, 0f);
        playercam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

}

