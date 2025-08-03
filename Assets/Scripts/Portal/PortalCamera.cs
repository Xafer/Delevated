using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    [SerializeField] private PortalDoor assignedDoor;

    private Vector3 relativePosition;

    [SerializeField] private float nearClipLimit = 0.2f;
    [SerializeField] private float nearClipOffset = 0.05f;
    [SerializeField] private float ClippingOffset = -0.02f;

    public void MoveCamera()
    {
        //Position of the player camera relative to the door' position
        relativePosition = assignedDoor.transform.InverseTransformPoint(PlayerCamera.instance.transform.position);

        relativePosition = new Vector3(-relativePosition.x, relativePosition.y, -relativePosition.z);

        Quaternion relativeRotation = Quaternion.Inverse(assignedDoor.transform.rotation) * PlayerCamera.instance.transform.rotation;

        transform.position = assignedDoor.GetConnectedDoor().transform.TransformPoint(relativePosition);
        transform.rotation =  assignedDoor.GetConnectedDoor().transform.rotation * Quaternion.Euler(0, 180, 0) * relativeRotation ;
        //transform.rotation = assignedDoor.GetConnectedDoor().transform.rotation * relativeRotation;
        SetNearClipPlane();
    }
    private void SetNearClipPlane()
    {
        Camera portalCam = GetComponent<Camera>();
        Camera playerCam = PlayerCamera.instance.GetComponent<Camera>();

        //portalCam.nearClipPlane = Vector3.Distance(assignedDoor.transform.position, playerCam.transform.position);

        // Learning resource:
        // http://www.terathon.com/lengyel/Lengyel-Oblique.pdf
        Transform clipPlane = assignedDoor.GetConnectedDoor().transform;
        int dot = System.Math.Sign(Vector3.Dot(clipPlane.forward, assignedDoor.GetConnectedDoor().transform.position - portalCam.transform.position));

        Vector3 camSpacePos = portalCam.worldToCameraMatrix.MultiplyPoint(clipPlane.position + clipPlane.forward* ClippingOffset);
        Vector3 camSpaceNormal = portalCam.worldToCameraMatrix.MultiplyVector(clipPlane.forward) * dot;
        float camSpaceDst = -Vector3.Dot(camSpacePos, camSpaceNormal) + nearClipOffset;

        // Don't use oblique clip plane if very close to portal as it seems this can cause some visual artifacts
        if (Mathf.Abs(camSpaceDst) > nearClipLimit)
        {
            Vector4 clipPlaneCameraSpace = new Vector4(camSpaceNormal.x, camSpaceNormal.y, camSpaceNormal.z, camSpaceDst);

            // Update projection based on new clip plane
            // Calculate matrix with player cam so that player camera settings (fov, etc) are used
            portalCam.projectionMatrix = playerCam.CalculateObliqueMatrix(clipPlaneCameraSpace);
        }
        else
        {
            portalCam.projectionMatrix = playerCam.projectionMatrix;
        }
    }

    public void SetAssignedDoor(PortalDoor assignedDoor)
    {
        this.assignedDoor = assignedDoor;
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(relativePosition, 0.5f);
    }*/
}
