using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ashes : MonoBehaviour {

    private List<Vector3> _points;
    private LineRenderer _lineRenderer;


    void Awake () {
        _points = new List<Vector3>();
        _lineRenderer = GetComponent<LineRenderer>();
    }
	
	void Update () {
        _lineRenderer.numPositions = _points.Count;
        for (int i = 0; i < _points.Count; i++)
            _lineRenderer.SetPosition(i, _points[i]);
    }

    public List<Vector3> getPoints()
    {
        return _points;
    }

    public void addPoint(Vector3 point)
    {
        _points.Add(point);
    }
}
