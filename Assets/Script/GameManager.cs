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

        PlayerControler.Instance.gameObject.SetActive(false);
        PlayerControler.Instance.transform.position = respawnPosition;
        PlayerControler.Instance.gameObject.SetActive(true);

    }

}
