using System.Collections;
using System.Linq.Expressions;
using UnityEngine;
using TMPro; // NUEVO: Importar la librería de TextMeshPro

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private Vector3 respawnPosition;
    public GameObject deathEffect;

    public int currentCoins;

    // NUEVO: Referencia al elemento de texto en la pantalla
    public TextMeshProUGUI coinText;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        respawnPosition = PlayerControler.Instance.transform.position;

        // NUEVO: Asegurar que el texto empiece en 0 al iniciar el nivel
        UpdateCoinUI();
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

        Instantiate(deathEffect, PlayerControler.Instance.transform.position + new Vector3(0f, 1f, 0f), PlayerControler.Instance.transform.rotation);

        yield return new WaitForSeconds(2f);

        PlayerControler.Instance.transform.position = respawnPosition;
        CameraController.instance.callbrain.enabled = true;
        PlayerControler.Instance.gameObject.SetActive(true);
    }

    public void SetSpawnPoint(Vector3 newSpawnPoint)
    {
        respawnPosition = newSpawnPoint;
        Debug.Log("SpawnSet");
    }

    public void AddCoin(int amount)
    {
        currentCoins += amount;

        UpdateCoinUI();
    }

    // NUEVO: Función dedicada a cambiar el texto visual
    private void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = "$" + currentCoins;
        }
        
    }
}