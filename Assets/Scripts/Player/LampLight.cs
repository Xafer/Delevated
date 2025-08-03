using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampLight : MonoBehaviour
{
    private Light light;
    [SerializeField] private RenderTexture rt;

    [SerializeField] private Light masterLight;

    void Start()
    {
        light = GetComponent<Light>();
    }

    private void Update()
    {
        light.cookie = rt;
        light.intensity = masterLight.intensity;
    }
}
