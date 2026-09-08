using UnityEngine;
using Unity.Collections;


public class KillPlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.instance.Respawn();

        }
    }
}
