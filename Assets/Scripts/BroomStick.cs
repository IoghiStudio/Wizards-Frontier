using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomStick : MonoBehaviour
{
    //public GameObject broomEquip;
    public GameObject broomEquipText;
    PlayerController playerScript;
    Rigidbody rb;
    public Animator animator;
    

    public float boostSpeed = 3;
    private float broomSpeed = 4.5f;
    public float initialPlayerSpeed;
    static float equippedBroomHeight = 10;

    //Broom Booleans
     static bool boostReady;
     bool equipped;
     bool equippable;
     static bool onBoost;
     public static bool slotFull;

    
    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        boostReady = true;

        //speeds
        initialPlayerSpeed = playerScript.walkSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        EquipBroom();
        ExitBroom();
        BroomBoost();
    }
    
    public void EquipBroom()
    {
        if (Input.GetKeyDown(KeyCode.E) && !equipped && equippable && !slotFull)
        {
            playerScript.controller.enabled = false;
            transform.SetParent(playerScript.broomContainer);
            playerScript.transform.position += new Vector3(0, equippedBroomHeight, 0);
            playerScript.controller.enabled = true;
            
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            /*transform.localScale = Vector3.one;*/

            slotFull = true;
            equipped = true;
            equippable = false;
            playerScript.walkSpeed = initialPlayerSpeed * broomSpeed;
            broomEquipText.SetActive(false);
            animator.SetTrigger("onBroom");

        }
    }
    public void ExitBroom()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && equipped && !onBoost && slotFull)
        {
            playerScript.controller.enabled = false;
            playerScript.transform.position += new Vector3(0, -equippedBroomHeight, 0);
            playerScript.controller.enabled = true;
            transform.SetParent(null);
            broomEquipText.SetActive(false);

            equipped = false;
            slotFull = false;
            playerScript.walkSpeed = initialPlayerSpeed;
            playerScript.exitBroomEffect.Play();

            transform.position = playerScript.transform.position /*+ new Vector3(0, -0.9f, 0)*/;
            animator.SetTrigger("onBroom");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            equippable = true;
            if(!slotFull)
            {
                broomEquipText.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            broomEquipText.SetActive(false);
            equippable = false;
        }
    }
    void BroomBoost()
    {
        if (Input.GetKeyDown(KeyCode.Q) && equipped && boostReady && playerScript.mana > 30)
        {
            playerScript.mana -= 30;
            playerScript.walkSpeed *= boostSpeed;
            boostReady = false;
            onBoost = true;
            StartCoroutine(BoostIsGone(2));

        }
    }

    IEnumerator BoostIsGone(float time)
    {
        yield return new WaitForSeconds(time);
        playerScript.walkSpeed /= boostSpeed;
        onBoost = false;
        StartCoroutine(BoostCharging(2));
    }

    IEnumerator BoostCharging(float time)
    {
        yield return new WaitForSeconds(time);
        boostReady = true;
    }
}
