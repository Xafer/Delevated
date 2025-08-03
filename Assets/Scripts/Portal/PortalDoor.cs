using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalDoor : MonoBehaviour
{
    [SerializeField] private PortalDoor connectedDoor;
    [SerializeField] private MeshRenderer mr;

    public PortalDoor GetConnectedDoor()
    {
        return connectedDoor;
    }

    public void SetMaterial(Material material)
    {
        mr.material = material;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position - transform.forward * 2 + transform.up, transform.position + transform.up);
        Gizmos.DrawLine(transform.position + transform.up, connectedDoor.transform.position + transform.up);
    }
}
