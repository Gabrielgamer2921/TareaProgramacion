using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1; 
    public float rotateSpeed = 90f; 

    void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.AddCoin(coinValue);

            Destroy(gameObject);
        }
    }
}
