using System.Collections.Generic;
using UnityEngine;

public class Trace : MonoBehaviour
{

    public enum TraceState
    {
        drawing,
        deleting
    };

    private static int ID_TRACE = 0;
    private int _idTrace;

    private List<Vector3> _points;
    private LineRenderer _lineRenderer;

    private TraceState _traceState;
    private float _minDistanceBetweenPoints;

    // Time between each point delete
    private float _deleteRate;

    // The distance used to move the first point when deleting. It makes it smoother
    private float _distanceToMoveFirst;
    private float _distanceToMoveLast;


    private bool _deleteFirst;
    private bool _deleteLast;

    private void Awake()
    {
        _idTrace = ID_TRACE;
        ID_TRACE++;
        _points = new List<Vector3>();
        _lineRenderer = GetComponent<LineRenderer>();
        _minDistanceBetweenPoints = 0.02f;
        _deleteRate = 0.5f;
        _traceState = TraceState.drawing;
    }

    private void Update()
    {
        switch (_traceState)
        {
            case TraceState.deleting:
                if (_deleteFirst && _points.Count > 1)
                {
                    Vector3 newPosPoint = Vector3.MoveTowards(_points[0], _points[1], _distanceToMoveFirst);
                    _points[0] = newPosPoint;
                }
                if (_deleteLast && _points.Count > 1)
                {
                    Vector3 newPosPoint = Vector3.MoveTowards(_points[_points.Count - 1], _points[_points.Count - 2], _distanceToMoveLast);
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

    /*
     * Getters
     */

    public int getIdTrace()
    {
        return _idTrace;
    }

    public List<Vector3> getPoints()
    {
        return _points;
    }

    public TraceState getTraceState()
    {
        return _traceState;
    }

    public void setPoints(List<Vector3> newList){
        _points = newList;
    }

    /*
     * Methods
     */

    public void addPoint(Vector3 newPos)
    {
        bool collision = transform.parent.gameObject.GetComponent<TracesHandler>().checkCollision(newPos, _minDistanceBetweenPoints, _idTrace);

        if (!collision)
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

    public void activateDeleting()
    {
        _traceState = TraceState.deleting;
        InvokeRepeating("burn", 0f, _deleteRate);
    }

    public void burnFirst(bool burn)
    {
        _deleteFirst = burn;
    }

    public void burnLast(bool burn)
    {
        _deleteLast = burn;
    }

    private void burn()
    {
        if (_points.Count > 0 && _deleteFirst)
            _distanceToMoveFirst = removePoint(0, 1);

        if (_points.Count > 0 && _deleteLast)
            _distanceToMoveLast = removePoint(_points.Count - 1, _points.Count - 2);

        if (_points.Count == 0)
        {
            CancelInvoke();
            Destroy(gameObject);
        }       
    }

    private float removePoint(int removeIndex, int neighbourIndex)
    {
        _points.RemoveAt(removeIndex);
        if (_points.Count > 1)
        {
            float distanceToNextPoint = Vector3.Distance(_points[removeIndex], _points[neighbourIndex]);
            return _distanceToMoveFirst = distanceToNextPoint / (_deleteRate * 60);
        }
        return 0;
    }
}
