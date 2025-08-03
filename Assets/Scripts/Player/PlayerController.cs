using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float mouseSpeed = 0.05f;
    [SerializeField] private float movementSpeed = 1;

    [SerializeField] private CharacterController cc;

    [SerializeField] private float gravityStrength = 9.81f;
    [SerializeField] private float jumpStrenth = 15;

    bool grounded = false;

    [SerializeField] private float verticalVelocity = 0;

    [SerializeField] private GameObject groundedDisplay;

    private Vector3 movement;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void UpdateGrounded()
    {
        int layerMask = ~LayerMask.GetMask("Player", "Portal");
        grounded = Physics.CheckSphere(transform.position + Vector3.up * (cc.radius - 0.02f - cc.skinWidth), cc.radius - 0.01f, layerMask);
        groundedDisplay.SetActive(grounded);


        if (!grounded)
            verticalVelocity -= Time.fixedDeltaTime * gravityStrength;
        else if(verticalVelocity < 0)
            verticalVelocity = 0;

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + Vector3.up * (cc.radius - 0.02f - cc.skinWidth), cc.radius - 0.01f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.None ? CursorLockMode.Locked : CursorLockMode.None;
        }

        if (Cursor.lockState != CursorLockMode.None)
        {
            float horizontal = Input.GetAxis("Mouse X") * mouseSpeed;
            float vertical = Input.GetAxis("Mouse Y") * mouseSpeed;

            float corrected = PlayerCamera.instance.transform.localEulerAngles.x - vertical;

            if (corrected >= 180)
                corrected -= 360;

            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + horizontal, 0);

            PlayerCamera.instance.transform.localEulerAngles = new Vector3(Mathf.Clamp(corrected, -85, 85), 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
            verticalVelocity += jumpStrenth;

        PortalManager.instance.MoveCameras();

        movement = new Vector3();

        if (Input.GetKey(KeyCode.A))
            movement += Vector3.left;
        else if (Input.GetKey(KeyCode.D))
            movement += Vector3.right;

        if (Input.GetKey(KeyCode.W))
            movement += Vector3.forward;
        else if (Input.GetKey(KeyCode.S))
            movement += Vector3.back;

        movement.Normalize();

        movement = new Vector3(movement.x, 0, movement.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 nextPosition = transform.rotation * movement * Time.deltaTime * movementSpeed;

        RaycastHit hit;

        if(nextPosition.magnitude > 0 && Physics.Raycast(PlayerCamera.instance.transform.position,nextPosition, out hit,nextPosition.magnitude))
        {
            PortalDoor portal = hit.transform.parent.GetComponent<PortalDoor>();
            if(portal != null)
            {
                Vector3 relativePosition = portal.transform.InverseTransformPoint(transform.position + nextPosition);

                relativePosition = new Vector3(-relativePosition.x, relativePosition.y, -relativePosition.z);

                Quaternion relativeRotation = Quaternion.Inverse(portal.transform.rotation) * transform.rotation;

                transform.position = portal.GetConnectedDoor().transform.TransformPoint(relativePosition);

                //Quaternion outputRotation = portal.GetConnectedDoor().transform.rotation * Quaternion.Euler(0, 180, 0) * relativeRotation;

                //transform.rotation = outputRotation;
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y - portal.transform.eulerAngles.y + portal.GetConnectedDoor().transform.eulerAngles.y + 180, 0);
                //PortalManager.instance.MoveCameras();
                UpdateGrounded();
                //portal.GetConnectedDoor().transform.parent.GetComponent<Room>().EnterRoom();
                PortalManager.instance.MoveCameras();
                return;
            }
        }

        if (!grounded || verticalVelocity > 0)
            nextPosition += Vector3.up * verticalVelocity * Time.deltaTime;

        cc.Move(nextPosition);
        //transform.position += nextPosition;
        UpdateGrounded();
    }
}
