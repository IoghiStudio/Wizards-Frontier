using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform playerCamera = null;
    [SerializeField] float mouseSensitivity = 4.2f;
    [SerializeField] bool lockCamera = true;
    public float walkSpeed = 2;
    public float runningSpeed = 5;
    public float crouchSpeed = 0.8f;
    public Transform broomContainer;
    Rigidbody playerRb;
    public ParticleSystem exitBroomEffect;
    public GameObject chestOpen;
    public GameObject chestClose;
    

    public float maxHealth = 100;
    public float maxMana = 100;
    public float health = 75;
    public float mana = 75;

    public bool isRunning;
    public bool isDead;
    
    float cameraPitch = 0.0f;
    public CharacterController controller = null;
    // Start is called before the first frame update
    void Start()
    {
        

        controller = GetComponent<CharacterController>();
        if (lockCamera)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        playerRb = GameObject.Find("Player").GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 1)
            isDead = true;

        UpdateMouseLook();
        UpdateMouseMovement();
    }
    public void UpdateMouseLook()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        cameraPitch -= mouseDelta.y * mouseSensitivity * Time.deltaTime * 100;
        cameraPitch = Mathf.Clamp(cameraPitch, -50.0f, 30.0f);

        if(!isDead)
        {
            playerCamera.localEulerAngles = Vector3.right * cameraPitch;
            transform.Rotate(Vector3.up * mouseDelta.x * mouseSensitivity * Time.deltaTime * 100);
        } else
        {
            playerCamera.localEulerAngles = Vector3.right * cameraPitch;
            playerCamera.transform.Rotate(Vector3.up * mouseDelta.x * mouseSensitivity * Time.deltaTime * 100);
        }
    }

    public void UpdateMouseMovement()
    {
        Vector2 moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveDir.Normalize();

        Vector3 velocity1 = (transform.forward * moveDir.y + transform.right * moveDir.x) * walkSpeed;
        Vector3 velocity2 = (transform.forward * moveDir.y + transform.right * moveDir.x) * runningSpeed;
        Vector3 velocity3 = (transform.forward * moveDir.y + transform.right * moveDir.x) * crouchSpeed;
        //sprint
        if (!isDead)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                controller.Move(velocity2 * Time.deltaTime);
                isRunning = true;
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                //crouch
                controller.Move(velocity3 * Time.deltaTime);
                Debug.Log("hahaha");
                isRunning = false;
            }
            else
            {
                //walk
                controller.Move(velocity1 * Time.deltaTime);
                isRunning = false;
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("ChestClose"))
        {
            Destroy(other.gameObject);
            chestOpen.gameObject.SetActive(true);
        }
            
    }



}