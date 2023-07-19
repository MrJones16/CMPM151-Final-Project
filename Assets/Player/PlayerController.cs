 using UnityEngine;
 using System.Collections;
 
 public class PlayerController : MonoBehaviour {
    //other things:
    public PlatformScript platformScript;
    public AudioController audioController;
    //Variables
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F; 
    public float gravity = 20.0F;
    public Camera playercam;
    private Vector3 moveDirection = Vector3.zero;
    private Vector2 mouseInput;
    public float sensitivity = 3f;

    private bool leftToggle = true;
    private bool rightToggle = true;
    public bool startedMusic = false;
    public Vector3 CheckpointPosition;
    public GameObject PlatformGeneratorPrefab;
    public bool reachedCheckpoint = false;
    private void Start() {
        Cursor.visible = false;
    }
    void Update() {
        CharacterController controller = GetComponent<CharacterController>();

        //CONTROLLING THE MUSIC/BLOCKS
        if (Input.GetMouseButtonDown(0)){//LEFT CLICK
            if(leftToggle){
                leftToggle = false;//turning off left click instrument
                audioController.MuteStrings();
                platformScript.disableStrings();
                audioController.stringsDisabled = true;
            }else{
                leftToggle = true;//turning on left click instrument
                audioController.UnmuteStrings();
                audioController.stringsDisabled = false;
            }
        }
        if (Input.GetMouseButtonDown(1)){//Right CLICK
            if(rightToggle){
                rightToggle = false;//turning off right click instrument
                audioController.MuteKeyboard();
                platformScript.disableKeyboard();
                audioController.keyboardDisabled = true;
            }else{
                rightToggle = true;//turning on right click instrument
                audioController.UnmuteKeyboard();
                audioController.keyboardDisabled = false;
            }
        }
        //staring music with jump
        if (!startedMusic && Input.GetAxisRaw("Jump") > 0){
            startedMusic = true;
            if (reachedCheckpoint){
                audioController.ChangeScale();
                reachedCheckpoint = false;
            }
            
            audioController.StartMetro();
        }
        //resetting to checkpoint
        if (transform.position.y < -20f){
            //controller.transform.SetPositionAndRotation(CheckpointPosition, transform.rotation);
            controller.enabled = false;
            transform.SetPositionAndRotation(CheckpointPosition, transform.rotation);
            this.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
            audioController.StopMetro();
            startedMusic = false;
            audioController.ResetSequence();
            controller.enabled = true;
            
        }
        //CHARACTER MOVEMENT
        
        //Feed moveDirection with input.
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), moveDirection.y, Input.GetAxis("Vertical"));
        moveDirection = transform.TransformDirection(moveDirection);
        //Multiply it by speed.
        moveDirection = new Vector3(moveDirection.x*speed, moveDirection.y, moveDirection.z*speed);
        // is the controller on the ground?
        if (controller.isGrounded) {
            
            //Jumping
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;
             
        }
        //Applying gravity to the controller
        moveDirection.y -= gravity * Time.deltaTime;
        //Making the character move
        controller.Move(moveDirection * Time.deltaTime);

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