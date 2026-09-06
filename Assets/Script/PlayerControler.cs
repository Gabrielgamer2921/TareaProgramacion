using Unity.Collections;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{

    public float moveSpeed;
    public float jumpForce;
    public float gravityScale = 5f;

    private Vector3 moveDirection;

    public CharacterController charController;
    public Camera playerCamera;

    private void Start()
    {

    }

    private void Update()
    {
        float yStore = moveDirection.y;



        moveDirection = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"));
        moveDirection = moveDirection * moveSpeed;
        moveDirection.y = yStore;

        if (Input.GetButtonDown("Jump"))
        {

            moveDirection.y = jumpForce;

        }

        moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale;

        charController.Move(moveDirection * Time.deltaTime);

        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw ("Vertical") != 0)

        transform.rotation = Quaternion.Euler(0f, playerCamera.transform.rotation.eulerAngles.y, 0f);
    }
















}
