using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class DirectionalAreaAnimator : MonoBehaviour {

    private MeshRenderer _meshRenderer = null;
    public MeshRenderer meshRenderer {
        get { return _meshRenderer ?? (_meshRenderer = GetComponent<MeshRenderer>()); }
    }
    private Material _material;
    public Material material
    {
        get { return _material ?? (meshRenderer.material = _material = new Material(meshRenderer.material)); }
    }

    [Header("Configuration")]
    [Range(-1,1)]
    public float direction;

    public float speed = 1;

    private double _animationTime = 0;

    private void Start()
    {
        direction = Random.Range(-1f, 1f);
    }

    private void Update()
    {
        _animationTime += Time.deltaTime * -direction * speed;
    }
    
    public void OnRenderObject()
    {
        // Set direction
        material.SetFloat("_Direction", Mathf.Sin((-direction * Mathf.PI)/2f));
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
