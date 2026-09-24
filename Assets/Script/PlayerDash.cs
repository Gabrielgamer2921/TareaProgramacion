using System.Collections;
using UnityEngine;

/// <summary>
/// Dash para el jugador con CharacterController.
/// - Impulso rápido en la dirección del input (relativa a la cámara),
///   o hacia donde mira el personaje si está quieto.
/// - Estela (TrailRenderer) que solo aparece durante el dash.
/// - Efecto de estirar/aplastar el modelo (squash & stretch).
/// </summary>
[RequireComponent(typeof(PlayerControler))]
public class PlayerDash : MonoBehaviour
{
    [Header("Dash")]
    public KeyCode teclaDash = KeyCode.LeftShift;
    [Tooltip("Distancia recorrida = velocidad x duración (20 x 0.2 = 4 unidades).")]
    public float velocidadDash = 20f;
    public float duracionDash = 0.2f;
    [Tooltip("Segundos antes de poder volver a usar el dash.")]
    public float tiempoRecarga = 0.6f;
    [Tooltip("Si está activo, solo se puede hacer un dash en el aire hasta volver a tocar el suelo.")]
    public bool unDashEnAire = true;

    [Header("Feedback visual")]
    [Tooltip("TrailRenderer que se activa solo durante el dash.")]
    public TrailRenderer estela;
    [Tooltip("Cuánto se alarga el modelo en la dirección del dash (1 = nada).")]
    public float estiramiento = 1.6f;
    [Tooltip("Cuánto se aplasta el modelo de lado (1 = nada).")]
    public float aplastamiento = 0.7f;

    [Header("Animator (opcional)")]
    [Tooltip("Nombre de un parámetro Trigger en tu Animator. Déjalo vacío si no tienes animación de dash.")]
    public string triggerAnimacion = "";

    private PlayerControler jugador;
    private Transform modelo;
    private Vector3 escalaOriginal;
    private float proximoDashPermitido;
    private bool dashAereoDisponible = true;

    private void Awake()
    {
        jugador = GetComponent<PlayerControler>();
        modelo = jugador.playerModel.transform;
        escalaOriginal = modelo.localScale;

        if (estela != null)
        {
            estela.emitting = false;
            estela.Clear();
        }
    }

    private void Update()
    {
        if (jugador.estaDasheando) return;

        if (jugador.charController.isGrounded)
            dashAereoDisponible = true;

        if (Input.GetKeyDown(teclaDash) && PuedeDashear())
        {
            if (!jugador.charController.isGrounded && unDashEnAire)
                dashAereoDisponible = false;

            StartCoroutine(RutinaDash(ObtenerDireccion()));
        }
    }

    private bool PuedeDashear()
    {
        if (Time.time < proximoDashPermitido) return false;

        bool enSuelo = jugador.charController.isGrounded;
        if (!enSuelo && unDashEnAire && !dashAereoDisponible) return false;

        return true;
    }

    private Vector3 ObtenerDireccion()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        if (input.sqrMagnitude > 0.01f)
        {
            // Misma lógica que tu movimiento: relativo a hacia dónde mira la cámara
            Quaternion giroCamara = Quaternion.Euler(0f, jugador.playerCamera.transform.eulerAngles.y, 0f);
            return (giroCamara * input).normalized;
        }

        // Sin input: dash hacia donde mira el personaje
        Vector3 adelante = modelo.forward;
        adelante.y = 0f;
        return adelante.normalized;
    }

    private IEnumerator RutinaDash(Vector3 direccion)
    {
        jugador.estaDasheando = true;

        if (estela != null) estela.emitting = true;

        if (!string.IsNullOrEmpty(triggerAnimacion) && jugador.animator != null)
            jugador.animator.SetTrigger(triggerAnimacion);

        // El modelo mira hacia donde vamos a dashear
        if (direccion.sqrMagnitude > 0.001f)
            modelo.rotation = Quaternion.LookRotation(direccion);

        float tiempo = 0f;
        while (tiempo < duracionDash)
        {
            tiempo += Time.deltaTime;

            // Squash & stretch: sube y baja suave (0 -> 1 -> 0) durante el dash
            float pulso = Mathf.Sin(Mathf.Clamp01(tiempo / duracionDash) * Mathf.PI);
            float lado = Mathf.Lerp(1f, aplastamiento, pulso);
            float largo = Mathf.Lerp(1f, estiramiento, pulso);
            modelo.localScale = new Vector3(
                escalaOriginal.x * lado,
                escalaOriginal.y * lado,
                escalaOriginal.z * largo);

            jugador.charController.Move(direccion * velocidadDash * Time.deltaTime);
            yield return null;
        }

        TerminarDash();
    }

    private void TerminarDash()
    {
        proximoDashPermitido = Time.time + tiempoRecarga;

        jugador.estaDasheando = false;
        jugador.DetenerVelocidadVertical();

        if (estela != null) estela.emitting = false;
        if (modelo != null) modelo.localScale = escalaOriginal;
    }

    // Si el jugador se desactiva a mitad del dash (muerte, cambio de escena...),
    // evitamos que los controles se queden bloqueados.
    private void OnDisable()
    {
        if (jugador != null && jugador.estaDasheando)
            TerminarDash();
    }
}
