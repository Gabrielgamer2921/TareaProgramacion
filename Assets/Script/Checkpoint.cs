using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public GameObject cpOn, cpOff;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.instance.SetSpawnPoint(transform.position);

            cpOff.SetActive(false);

            cpOn.SetActive(true);
        }
    }
}
