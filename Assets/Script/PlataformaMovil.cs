using UnityEngine;

/// <summary>
/// Plataforma móvil 3D que va de un punto A (posición inicial) a un punto B.
/// Compatible con personajes que usan CharacterController: los lleva
/// consigo aplicándoles el mismo desplazamiento que hace la plataforma.
/// Se puede usar en bucle (ida y vuelta) o controlarla desde otros scripts
/// (palancas, botones, triggers, etc.).
/// </summary>
[RequireComponent(typeof(Collider))]
public class PlataformaMovil : MonoBehaviour
{
    public enum TipoMovimiento { Horizontal, Vertical, Personalizado }

    [Header("Movimiento")]
    [Tooltip("Horizontal = eje X, Vertical = eje Y, Personalizado = usa la dirección de abajo.")]
    [SerializeField] private TipoMovimiento tipoMovimiento = TipoMovimiento.Horizontal;

    [Tooltip("Distancia entre A y B. Un valor negativo invierte el sentido (izquierda / abajo).")]
    [SerializeField] private float distancia = 4f;

    [Tooltip("Solo se usa si el tipo es Personalizado (por ejemplo (0,0,1) para el eje Z, o (1,1,0) para diagonal).")]
    [SerializeField] private Vector3 direccionPersonalizada = new Vector3(0f, 0f, 1f);

    [SerializeField] private float velocidad = 2f;

    [Tooltip("Segundos que espera al llegar a cada extremo.")]
    [SerializeField] private float tiempoEspera = 0.5f;

    [Header("Comportamiento")]
    [Tooltip("Si está activo, empieza a moverse en cuanto arranca el nivel.")]
    [SerializeField] private bool moverAlIniciar = true;

    [Tooltip("Activo = va y vuelve sin parar. Desactivado = se mueve al otro punto y se detiene (tipo ascensor).")]
    [SerializeField] private bool repetirIdaYVuelta = true;

    [Header("Pasajeros (CharacterController)")]
    [SerializeField] private bool llevarPasajeros = true;

    [Tooltip("Altura de la zona de detección sobre la plataforma.")]
    [SerializeField] private float alturaDeteccion = 0.3f;

    private Vector3 puntoA;
    private Vector3 puntoB;
    private Vector3 destino;
    private bool moviendo;
    private float esperaRestante;

    private Collider col;
    private readonly Collider[] resultados = new Collider[16];

    private void Awake()
    {
        col = GetComponent<Collider>();
    }

    private void Start()
    {
        puntoA = transform.position;
        puntoB = puntoA + ObtenerDireccion() * distancia;
        destino = puntoB;
        moviendo = moverAlIniciar;
    }

    private void Update()
    {
        if (!moviendo) return;

        if (esperaRestante > 0f)
        {
            esperaRestante -= Time.deltaTime;
            return;
        }

        Vector3 posAnterior = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
        Vector3 desplazamiento = transform.position - posAnterior;

        if (llevarPasajeros && desplazamiento != Vector3.zero)
            LlevarPasajeros(desplazamiento);

        // ¿Llegamos al destino?
        if ((transform.position - destino).sqrMagnitude < 0.0001f)
        {
            transform.position = destino;
            destino = (destino == puntoB) ? puntoA : puntoB;

            if (repetirIdaYVuelta)
                esperaRestante = tiempoEspera;
            else
                moviendo = false;
        }
    }

    private Vector3 ObtenerDireccion()
    {
        switch (tipoMovimiento)
        {
            case TipoMovimiento.Horizontal: return Vector3.right;
            case TipoMovimiento.Vertical: return Vector3.up;
            default: return direccionPersonalizada.normalized;
        }
    }

    /// <summary>
    /// Busca CharacterControllers apoyados sobre la plataforma y los mueve
    /// exactamente lo mismo que se movió ella.
    /// </summary>
    private void LlevarPasajeros(Vector3 desplazamiento)
    {
        // Asegura que los bounds del collider reflejen la nueva posición
        Physics.SyncTransforms();

        Bounds b = col.bounds;
        Vector3 centro = new Vector3(b.center.x, b.max.y + alturaDeteccion * 0.3f, b.center.z);
        Vector3 mitad = new Vector3(b.extents.x, alturaDeteccion * 0.7f, b.extents.z);

        int n = Physics.OverlapBoxNonAlloc(centro, mitad, resultados, Quaternion.identity,
                                           ~0, QueryTriggerInteraction.Ignore);

        for (int i = 0; i < n; i++)
        {
            if (resultados[i].TryGetComponent(out CharacterController cc) && cc.enabled)
                cc.Move(desplazamiento);
        }
    }

    // ------------------------------------------------------------------
    // MÉTODOS PÚBLICOS: llámalos desde palancas, botones, eventos, etc.
    // ------------------------------------------------------------------

    /// <summary>Empieza / reanuda el movimiento hacia el destino actual.</summary>
    public void Activar() => moviendo = true;

    /// <summary>Detiene la plataforma donde esté.</summary>
    public void Detener() => moviendo = false;

    /// <summary>Si se mueve la detiene; si está parada la activa.</summary>
    public void Alternar() => moviendo = !moviendo;

    /// <summary>Manda la plataforma al punto A y la mueve.</summary>
    public void IrAPuntoA() { destino = puntoA; esperaRestante = 0f; moviendo = true; }

    /// <summary>Manda la plataforma al punto B y la mueve.</summary>
    public void IrAPuntoB() { destino = puntoB; esperaRestante = 0f; moviendo = true; }

    // ------------------------------------------------------------------
    // AYUDA VISUAL EN EL EDITOR: A (verde), B (rojo)
    // ------------------------------------------------------------------

    private void OnDrawGizmos()
    {
        Vector3 a = Application.isPlaying ? puntoA : transform.position;
        Vector3 b = Application.isPlaying ? puntoB : a + ObtenerDireccion() * distancia;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(a, b);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(a, 0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(b, 0.2f);
    }
}
