using System.Collections;
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

    public GameObject FireParticles;


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

    private IEnumerator _coroutine;

    GameObject fireParticle;
    GameObject fireParticleLast;

    private void Awake()
    {
        _idTrace = ID_TRACE;
        ID_TRACE++;
        _points = new List<Vector3>();
        _lineRenderer = GetComponent<LineRenderer>();
        _minDistanceBetweenPoints = 0.05f;
        _deleteRate = 0.1f;
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
                    _points[_points.Count - 1] = newPosPoint;
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
                Vector3 currentPos = newPos;
                Vector3 lastsPos = _points[_points.Count - 1];
                float distanceFromLast = Vector3.Distance(currentPos, lastsPos);

                while(distanceFromLast > _minDistanceBetweenPoints)
                {
                    lastsPos = Vector3.MoveTowards(lastsPos, currentPos, _minDistanceBetweenPoints);
                    _points.Add(lastsPos);
                    distanceFromLast = Vector3.Distance(currentPos, lastsPos);
                }
            }
        }
    }

    public void activateDeleting()
    {
        if (_traceState != TraceState.deleting)
        {
            _traceState = TraceState.deleting;
            _coroutine = burn(_deleteRate);
            StartCoroutine(_coroutine);
        }
       
    }

    public void burnFirst(bool burn)
    {
        _deleteFirst = burn;
    }

    public void burnLast(bool burn)
    {
        _deleteLast = burn;
    }


    private IEnumerator burn(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);

            if (_points.Count > 0 && _deleteFirst)
            {
                if (fireParticle == null)
                {
                    fireParticle = Instantiate(FireParticles, transform);
                }
                fireParticle.transform.position = _points[0];
                _points.RemoveAt(0);
                if (_points.Count > 1)
                {
                    float distanceToNextPoint = Vector3.Distance(_points[0], _points[1]);
                    _distanceToMoveFirst = distanceToNextPoint / (_deleteRate * 60);
                }
            }
            if (_points.Count > 0 && _deleteLast)
            {
                if (fireParticleLast == null)
                {
                    fireParticleLast = Instantiate(FireParticles, transform);
                }
                fireParticleLast.transform.position = _points[_points.Count - 1];
                _points.RemoveAt(_points.Count - 1);
                if (_points.Count > 1)
                {
                    float distanceToNextPoint = Vector3.Distance(_points[_points.Count - 1], _points[_points.Count - 2]);
                    _distanceToMoveLast = distanceToNextPoint / (_deleteRate * 60);
                }
            }

            if (_points.Count == 0)
            {
                if (_deleteFirst && !_deleteLast)
                    transform.parent.gameObject.GetComponent<TracesHandler>().burnFollowing(_idTrace, true);
                else if (_deleteLast && !_deleteFirst)
                    transform.parent.gameObject.GetComponent<TracesHandler>().burnFollowing(_idTrace, false);
                Destroy(gameObject);
            }
        }      
    }

    private float removePoint(int removeIndex, int neighbourIndex)
    {
        
        return 0;
    }
}
