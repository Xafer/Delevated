using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuaternionRotator : MonoBehaviour
{
    [SerializeField] private Vector3 euler;

    private void Update()
    {
        transform.rotation = Quaternion.Euler(euler.x, euler.y, euler.z) * transform.rotation;
    }
}
