using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{

    public float lifeTime;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, lifeTime);
        
    }
}
