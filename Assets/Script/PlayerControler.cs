using Unity.Collections;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{

    public float moveSpeed;
    public float jumpForce;
    public float gravityScale = 5f;

    private Vector3 moveDirection;

    public CharacterController charController;
    private void Start()
    {
        
    }

    private void Update()
    {
        float yStore = moveDirection.y;

        moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        moveDirection = moveDirection * moveSpeed;
        moveDirection.y = yStore;

        if (Input.GetButtonDown("Jump"))
        {

            moveDirection.y = jumpForce;

        }

        moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale;

        charController.Move(moveDirection * Time.deltaTime);

    }
















}
