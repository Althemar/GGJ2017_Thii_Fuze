using System.Collections.Generic;
using UnityEngine;

public class Trace : MonoBehaviour
{
    private List<Vector3> _points;
    private LineRenderer _lineRenderer;
    private float _minDistanceBetweenPoints;

    private void Start()
    {
        _points = new List<Vector3>();
        _lineRenderer = GetComponent<LineRenderer>();
        _minDistanceBetweenPoints = 0.1f;
    }

    private void Update()
    {
        _lineRenderer.numPositions = _points.Count;
        for (int i = 0; i < _points.Count; i++)
            _lineRenderer.SetPosition(i, _points[i]);
    }

    public void addPoint(Vector3 newPos)
    {
        if (_points.Count == 0)
            _points.Add(newPos);
        else
        {
            float distanceFromLast = Vector3.Distance(newPos, _points[_points.Count - 1]);
            if (distanceFromLast >= _minDistanceBetweenPoints)
                _points.Add(newPos);
        }
    }
}
