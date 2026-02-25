using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CinemachineCamera[] camaras;

    public void ActivarCamara(CinemachineCamera cam)
    {
        foreach (var c in camaras)
        {
            c.Priority = 0;
        }

        cam.Priority = 20;
    }
}
