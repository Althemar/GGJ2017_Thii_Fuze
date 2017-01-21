using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class DirectionalAreaAnimator : MonoBehaviour {

    /*************
     * RENDERING *
     *************/
    // Renderer
    private MeshRenderer _meshRenderer = null;
    public MeshRenderer meshRenderer {
        get { return _meshRenderer ?? (_meshRenderer = GetComponent<MeshRenderer>()); }
    }

    // Material
    private Material _material;
    public Material material
    {
        get { return _material ?? (meshRenderer.material = _material = new Material(meshRenderer.material)); }
    }

    /**************
     * CONTROLLER *
     **************/
    private IDirectionalArea _controller;
    public IDirectionalArea controller
    {
        get { return _controller ?? (_controller = GetComponent<IDirectionalArea>()); }
    }

    private double _animationTime = 0;

    private void Update()
    {
        _animationTime += Time.deltaTime * -controller.GetDirection() * controller.GetVelocity();
    }
    
    public void OnRenderObject()
    {
        // Set direction
        material.SetFloat("_Direction", Mathf.Sin((-controller.GetDirection() * Mathf.PI)/2f));
        //material.SetFloat("_Direction", direction);
        // Set animation time
        material.SetFloat("_AnimationTime", (float)_animationTime);
    }

    public void Damien()
    {
        // What a memo
    }
    public void Darenn()
    {
        // What a memo
    }

}
