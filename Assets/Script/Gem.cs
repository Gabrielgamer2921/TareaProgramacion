using UnityEngine;

public class Gem : MonoBehaviour
{
    [Header("Valor")]
    public int gemValue = 1;

    [Header("Rotación")]
    public float rotateSpeed = 90f;
    [Tooltip("Eje sobre el que gira, en espacio del MUNDO (no depende de cómo esté rotado el modelo). (0,1,0) = como un trompo, horizontal. (1,0,0) o (0,0,1) = voltereta.")]
    public Vector3 ejeRotacion = Vector3.up;

    [Header("Flotación (opcional)")]
    [Tooltip("Deja en 0 si no quieres que suba y baje.")]
    public float alturaFlote = 0.25f;
    public float velocidadFlote = 2f;

    private Vector3 posicionInicial;

    private void Start()
    {
        posicionInicial = transform.position;
    }

    void Update()
    {
        
        transform.Rotate(ejeRotacion.normalized * rotateSpeed * Time.deltaTime, Space.World);

        if (alturaFlote > 0f)
        {
            float offsetY = Mathf.Sin(Time.time * velocidadFlote) * alturaFlote;
            transform.position = posicionInicial + Vector3.up * offsetY;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            GameManager.instance.AddGem(gemValue);

            Destroy(gameObject);
        }
    }
}