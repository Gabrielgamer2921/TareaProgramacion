using UnityEngine;

/// <summary>
/// Poner en el trigger de la bandera / meta final del nivel.
/// </summary>
public class LevelGoal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.CompletarNivel();
        }
    }
}
