using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;

    public GameObject panelPausa;
    public KeyCode teclaPausa = KeyCode.Escape;

    private bool pausado;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (panelPausa != null)
            panelPausa.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(teclaPausa) && PuedeUsarPausa())
            AlternarPausa();
    }

    // No se puede pausar mientras se muestra el resumen de nivel
    private bool PuedeUsarPausa()
    {
        return GameManager.instance == null || !GameManager.instance.nivelCompletado;
    }

    private void AlternarPausa()
    {
        pausado = !pausado;

        if (panelPausa != null)
            panelPausa.SetActive(pausado);

        Time.timeScale = pausado ? 0f : 1f;
        Cursor.visible = pausado;
        Cursor.lockState = pausado ? CursorLockMode.None : CursorLockMode.Locked;
    }

    /// <summary>Enganchar al botón "Reanudar" (OnClick, en el Inspector).</summary>
    public void Reanudar()
    {
        if (pausado) AlternarPausa();
    }
}