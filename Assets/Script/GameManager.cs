using System.Collections;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private Vector3 respawnPosition;
    public GameObject deathEffect;

    public int currentCoins;
    public int currentGems;

    public TextMeshProUGUI coinText;
    public TextMeshProUGUI gemText;

    // NUEVO: resumen de nivel / meta final
    [Header("Meta final")]
    public GameObject panelResumen;
    public TextMeshProUGUI resumenMonedasText;
    public TextMeshProUGUI resumenGemasText;
    public TextMeshProUGUI resumenTiempoText;
    [Tooltip("Segundos que se muestra el resumen antes de reiniciar el nivel.")]
    public float tiempoAntesDeReiniciar = 4f;

    [HideInInspector] public bool nivelCompletado;
    private float tiempoTranscurrido;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        respawnPosition = PlayerControler.Instance.transform.position;

        UpdateCoinUI();
        UpdateGemUI();

        if (panelResumen != null)
            panelResumen.SetActive(false);
    }

    
    void Update()
    {
        if (!nivelCompletado)
            tiempoTranscurrido += Time.deltaTime;
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


    private void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = "$" + currentCoins;
        }

    }

    public void AddGem(int amount)
    {
        currentGems += amount;

        UpdateGemUI();
    }

    private void UpdateGemUI()
    {
        if (gemText != null)
        {
            gemText.text = currentGems.ToString();
        }
    }

   
    public void CompletarNivel()
    {
        if (nivelCompletado) return; 

        nivelCompletado = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (panelResumen != null)
        {
            panelResumen.SetActive(true);

            if (resumenMonedasText != null) resumenMonedasText.text = currentCoins.ToString();
            if (resumenGemasText != null) resumenGemasText.text = currentGems.ToString();
            if (resumenTiempoText != null) resumenTiempoText.text = FormatearTiempo(tiempoTranscurrido);
        }

        Time.timeScale = 0f;
        StartCoroutine(ReiniciarNivelTrasEspera());
    }

    private IEnumerator ReiniciarNivelTrasEspera()
    {
        
        yield return new WaitForSecondsRealtime(tiempoAntesDeReiniciar);

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private string FormatearTiempo(float segundos)
    {
        int minutos = Mathf.FloorToInt(segundos / 60f);
        int segs = Mathf.FloorToInt(segundos % 60f);
        return string.Format("{0:00}:{1:00}", minutos, segs);
    }
}