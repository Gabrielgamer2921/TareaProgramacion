using Unity.Collections;
using UnityEngine;

public class PlayerMovimiento : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform camara;
    private CharacterController controlador;

    [Header("Movimiento")]
    [SerializeField] private bool UsaGetAxisRaw;
    [SerializeField] private float velocidadMovimiento = 5f;

    public bool puedeMoverse = true;

    [Header("Gravedad")]
    [SerializeField] private float Gravedad = -9f;
    private Vector3 velocidadVertical;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Awake()
    {
        controlador = GetComponent<CharacterController>();

        if (camara == null && Camera.main != null)
            camara = Camera.main.transform;
    }


    void Update()
    {
        // NUEVO: Solo nos movemos si el interruptor está encendido
        if (puedeMoverse)
        {
            MoverJugadorEnPlano();
        }

        // La gravedad la dejamos fuera del 'if' para que Spencer 
        // no se quede flotando si interactúa mientras cae de un escalón.
        AplicarGravedad();
    }

    private void MoverJugadorEnPlano()
    {
        float ValorHorizontal = UsaGetAxisRaw ? Input.GetAxisRaw("Horizontal") : Input.GetAxis("Horizontal");
        float ValorVertical = UsaGetAxisRaw ? Input.GetAxisRaw("Vertical") : Input.GetAxis("Vertical");

        Vector3 adelantecamara = camara.forward;
        Vector3 derechacamara = camara.right;

        adelantecamara.y = 0f;
        derechacamara.y = 0f;

        adelantecamara.Normalize();
        derechacamara.Normalize();

        Vector3 direccionplano = (derechacamara * ValorHorizontal + adelantecamara * ValorVertical);

        if (direccionplano.sqrMagnitude > 0.0001f)
            direccionplano.Normalize();

        Vector3 desplazamientoXZ = direccionplano * (velocidadMovimiento * Time.deltaTime);

        controlador.Move(desplazamientoXZ);
    }

    private void AplicarGravedad()
    {
        velocidadVertical.y += Gravedad * Time.deltaTime;
        controlador.Move(velocidadVertical * Time.deltaTime);

        if (controlador.isGrounded && velocidadVertical.y < 0)
        {
            velocidadVertical.y = -2f;
        }
    }

    // NUEVO: Este método será llamado por los NPCs y Puzzles para congelar al jugador
    public void CambiarEstadoMovimiento(bool estado, bool liberarCursor = true)
    {
        puedeMoverse = estado;

        if (!estado)
        {
            if (liberarCursor)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
