using System.Collections;
using System.Linq.Expressions;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private Vector3 respawnPosition;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        respawnPosition = PlayerControler.Instance.transform.position;
    }


    void Update()
    {
        
    }

    public void Respawn()
    {

        StartCoroutine("RespawnWaiter");

    }

    public IEnumerator RespawnWaiter()
    {
        PlayerControler.Instance.gameObject.SetActive(false);

        CameraController.instance.callbrain.enabled = false;

        yield return new WaitForSeconds(2f);

        PlayerControler.Instance.transform.position = respawnPosition;

        CameraController.instance.callbrain.enabled = true;


        PlayerControler.Instance.gameObject.SetActive(true);
    }


}
