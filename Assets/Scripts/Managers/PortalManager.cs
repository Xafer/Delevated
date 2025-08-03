using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public static PortalManager instance;

    private void Awake()
    {
        instance = this;
    }

    [System.Serializable]
    public struct CameraData
    {
        public PortalCamera camera;
        public Material cameraRenderMaterial;
    }

    [SerializeField] private List<CameraData> cameras;

    public void AssignPortalDoors(List<PortalDoor> doors)
    {
        int i = 0;
        foreach(CameraData data in cameras)
        {
            cameras[i].camera.SetAssignedDoor(doors[i]);
            doors[i].SetMaterial(data.cameraRenderMaterial);

            i++;
        }
    }

    public void MoveCameras()
    {
        foreach (CameraData data in cameras)
            data.camera.MoveCamera();
    }

}
