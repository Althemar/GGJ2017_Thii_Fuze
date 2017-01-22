using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trace : MonoBehaviour
{

    public enum TraceState
    {
        drawing,
        deleting,
        deleted,
    };

    private static int BURNING_TRACES = 0;
    private static int ID_TRACE = 0;
    private int _idTrace;
    private int _nbPointMax;

    public GameObject FireParticles;
    public GameObject _ashToCreate;


    public List<Vector3> _points;
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

    private Ashes _ashesFirst;
    private Ashes _ashesLast;

    private IEnumerator _coroutine;

    private GameObject _fireParticle;
    private GameObject _fireParticleLast;

    private void Awake()
    {
        _idTrace = ID_TRACE;
        ID_TRACE++;
        _points = new List<Vector3>();
        _lineRenderer = GetComponent<LineRenderer>();
        _minDistanceBetweenPoints = 1/8f;
        _deleteRate = 0.001f;// GeneralManager.GetDificulty(); // 0.001f;
        _traceState = TraceState.drawing;
        _distanceToMoveLast = _distanceToMoveFirst;
        _nbPointMax = 0;
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

    public static int getBurningTraces()
    {
        return BURNING_TRACES;
    }

    public int getNbPointsMax()
    {
        return _nbPointMax;
    }

    /*
     * Methods
     */

    public void addPoint(Vector3 newPos)
    {
        if (_traceState != TraceState.drawing)
            return;

        bool collision = transform.parent.gameObject.GetComponent<TracesHandler>().checkCollision(newPos, _minDistanceBetweenPoints, _idTrace);

        if (!collision)
        {
            if (_points.Count == 0)
            {
                _points.Add(newPos);
                _nbPointMax++;
            }
            else
            {
                Vector3 currentPos = newPos;
                Vector3 lastsPos = _points[_points.Count - 1];
                float distanceFromLast = Vector3.Distance(currentPos, lastsPos);

                while(distanceFromLast > _minDistanceBetweenPoints)
                {
                    lastsPos = Vector3.MoveTowards(lastsPos, currentPos, _minDistanceBetweenPoints);
                    _points.Add(lastsPos);
                    _nbPointMax++;
                    distanceFromLast = Vector3.Distance(currentPos, lastsPos);
                }
            }
        }
    }

    public void activateDeleting()
    {
        BURNING_TRACES++;

        GameObject ashFirst = Instantiate(_ashToCreate);
        GameObject ashLast = Instantiate(_ashToCreate);

        _ashesFirst = ashFirst.GetComponent<Ashes>();
        _ashesLast = ashLast.GetComponent<Ashes>();

        ashFirst.transform.parent = transform;
        ashLast.transform.parent = transform;

        transform.parent.gameObject.GetComponent<TracesHandler>().addAshes(ashFirst.GetComponent<Ashes>());
        transform.parent.gameObject.GetComponent<TracesHandler>().addAshes(ashLast.GetComponent<Ashes>());
        

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
                _ashesFirst.addPoint(_points[0]);
                removePoint(0);
            }
            if (_points.Count > 0 && _deleteLast)
            {
                _ashesLast.addPoint(_points[_points.Count - 1]);
                removePoint(_points.Count - 1);
            }
            if (_points.Count == 0)
            {
                _traceState = TraceState.deleted;
                if (_deleteFirst && !_deleteLast)
                    transform.parent.gameObject.GetComponent<TracesHandler>().burnFollowing(_idTrace, true);
                else if (_deleteLast && !_deleteFirst)
                    transform.parent.gameObject.GetComponent<TracesHandler>().burnFollowing(_idTrace, false);
                if (_fireParticle != null)
                    Destroy(_fireParticle);
                if (_fireParticleLast != null)
                    Destroy(_fireParticleLast);
                BURNING_TRACES--;
                StopCoroutine(_coroutine);
            }
        }      
    }

    private void removePoint(int removeIndex)
    {
        instantiateParticule(removeIndex);

        _points.RemoveAt(removeIndex);
        if (_points.Count > 1)
        {
            int neighbourIndex;
            if (removeIndex == 0)
                neighbourIndex = 1;
            else if (removeIndex == _points.Count - 1)
            {
                removeIndex--;
                neighbourIndex = removeIndex - 1;
            }
                
            else
                return;
            float distanceToNextPoint = Vector3.Distance(_points[removeIndex], _points[neighbourIndex]);
            _distanceToMoveFirst = distanceToNextPoint / (_deleteRate * 60);
        }
    }

    private void instantiateParticule(int pointIndex)
    {
        if (_fireParticleLast == null)
        {
            _fireParticleLast = Instantiate(FireParticles, transform);
        }
        _fireParticleLast.transform.position = _points[pointIndex];
    }

    
    
}
