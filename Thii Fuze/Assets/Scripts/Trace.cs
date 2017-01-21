using System.Collections.Generic;
using UnityEngine;

public class Trace : MonoBehaviour
{

    public enum TraceState
    {
        drawing,
        deleting
    };

    private List<Vector3> _points;
    private LineRenderer _lineRenderer;

    private TraceState _traceState;
    private float _minDistanceBetweenPoints;

    // Time between each point delete
    private float _deleteRate;

    // The distance used to move the first point when deleting. It makes it smoother
    private float _distanceToMove;


    private void Start()
    {
        _points = new List<Vector3>();
        _lineRenderer = GetComponent<LineRenderer>();
        _minDistanceBetweenPoints = 0.1f;
        _deleteRate = 0.5f;
        _traceState = TraceState.drawing;
    }

    private void Update()
    {
        switch (_traceState)
        {
            case TraceState.deleting:
                if (_points.Count > 1)
                {
                    Vector3 newPosPoint = Vector3.MoveTowards(_points[0], _points[1], _distanceToMove);
                    _points[0] = newPosPoint;
                }
                break;
            case TraceState.drawing:
                break;
        }

        _lineRenderer.numPositions = _points.Count;
        for (int i = 0; i < _points.Count; i++)
            _lineRenderer.SetPosition(i, _points[i]);
    }

    public TraceState getTraceState()
    {
        return _traceState;
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

    public void activateDeleting()
    {
        _traceState = TraceState.deleting;
        InvokeRepeating("deleteFirstPoint", 0f, _deleteRate);
    }

    private void deleteFirstPoint()
    {
        if (_points.Count > 0)
        {
            _points.RemoveAt(0);
            if (_points.Count > 1)
            {
                float distanceToNextPoint = Vector3.Distance(_points[0], _points[1]);
                _distanceToMove = distanceToNextPoint / (_deleteRate * 60);
            }
        }
        else
            CancelInvoke();
    }
}
