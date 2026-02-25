using JetBrains.Annotations;
using Unity.Cinemachine;
using UnityEngine;

public class CamaraB : MonoBehaviour
{
    public CameraManager camManager;
    public CinemachineCamera cam;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            camManager.ActivarCamara(cam);
        }
    }

}
