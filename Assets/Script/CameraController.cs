using Unity.Cinemachine;
using Unity.Scripting.LifecycleManagement;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public static CameraController instance;

    public CinemachineBrain callbrain;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
