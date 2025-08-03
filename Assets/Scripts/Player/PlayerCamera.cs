using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;

    private void Awake()
    {
        instance = this;
    }
    /*protected void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering += CustomOnPreRender;
    }

    protected void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= CustomOnPreRender;
    }

    protected void CustomOnPreRender(ScriptableRenderContext context, Camera camera)
    {
    }*/
}
