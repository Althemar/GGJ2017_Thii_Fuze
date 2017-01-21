using System.Collections.Generic;
using UnityEngine;

public class TracesHandler : MonoBehaviour {

    public GameObject _player;
    public GameObject _traceToCreate;

    private PlayerController _playerController;
    private List<Trace> _traces;
    private Graph _graph;

    private Vector3 _lastTraceCross;

    public delegate GameObject OnIntersectionHappenedHandler(Vector3 intersectionPosition);
    public OnIntersectionHappenedHandler eOnIntersectionHappened;

    [Header("Prefabs")]
    public GameObject intersectionPrefab;

    // Use this for initialization
    void Awake() {
        _playerController = _player.GetComponent<PlayerController>();
        _traces = new List<Trace>();

        GameObject trace = Instantiate(_traceToCreate, _playerController.transform.position, Quaternion.identity);
        trace.transform.parent = transform;
        
        _traces.Add(trace.GetComponent<Trace>());
        _playerController.setTrace(trace.GetComponent<Trace>());
        

        _graph = new Graph();
        _graph.addNode();

        _lastTraceCross = _playerController.GetComponent<Transform>().position;

        Bomb.OnPlayerInitiateBomb += beginBurn;
    }

    void Start()
    {
        eOnIntersectionHappened = InstanceIntersectionIndicator;
    }
    
    void OnDestroy()
    {
        eOnIntersectionHappened -= InstanceIntersectionIndicator;
    }

    public void beginBurn()
    {
        Node previousNode = _graph.getNodes()[_graph.getNodes().Count - 1];
        Node newNode = _graph.addNode();
        _graph.createTransition(previousNode, newNode, _traces[_traces.Count - 1].getIdTrace());

        if (_traces.Count != 0)
        {
            _traces[0].activateDeleting();
            _traces[0].burnFirst(true);
        }
    }

    public void burnFollowing(int idTrace, bool deleteFirst)
    {
        Edge currentEdge = _graph.getEdgeWithId(idTrace);

        Node followingNode;
        if (deleteFirst)
            followingNode = currentEdge.getNodeLast();
        else
            followingNode = currentEdge.getNodeFirst();
        followingNode.release();

        print(_graph.printGraph());
        print(followingNode.getId());

        bool activate = false;
        bool burnFirst = false;
        bool burnLast = false;
        foreach (Edge edge in _graph.getEdges())
        {
            if (edge.getId() != idTrace)
            {
                

                if (edge.getNodeFirst() == followingNode)
                {
                    activate = true;
                    burnFirst = true;
                }
                    
                if (edge.getNodeLast() == followingNode)
                {
                    activate = true;
                    burnLast = true;
                }
                    
                if (activate)
                {
                    foreach (Trace trace in _traces)
                    {

                        if (trace.getIdTrace() == edge.getId())
                        {
                            print("Burn " + edge.getId());
                            trace.activateDeleting();
                            trace.burnFirst(burnFirst);
                            trace.burnLast(burnLast);
                            activate = false;
                            burnFirst = false;
                            burnLast = false;
                        }
                    }
                }
            }
        }
    }

    public bool checkCollision(Vector3 newPos, float minDistanceBetweenPoints, int idTrace)
    {
        for (int i = 0; i < _traces.Count; i++)
        {
            if (_traces[i].getIdTrace() == idTrace)
            {
                if (_traces[i].getPoints().Count <= 1)
                    continue;
                else if (Vector3.Distance(_traces[i].getPoints()[_traces[i].getPoints().Count - 1], newPos) < minDistanceBetweenPoints)
                    continue;
            }

            for (int j = 0; j < _traces[i].getPoints().Count; j++)
            {
                if (Vector3.Distance(_traces[i].getPoints()[j], newPos) <= minDistanceBetweenPoints && Vector3.Distance(_lastTraceCross, newPos) >= 2* minDistanceBetweenPoints)
                {
                    // Intersected
                    GameObject crossSign = null;
                    if(eOnIntersectionHappened != null)
                    {
                        crossSign = eOnIntersectionHappened(newPos);
                    }
 
                    Trace traceCrossed = _traces[i];
                    _lastTraceCross = newPos;

                    if (!_graph.traceExists(traceCrossed.getIdTrace()))
                    {
                        Trace newTrace = createNewTrace(traceCrossed, j);
                        if (newTrace == null)
                            return false;

                        Node oldNode = _graph.getNodes()[_graph.getNodes().Count - 1];
                        Node newNode = _graph.addNode();
                        newNode.setCrossSign(crossSign);

                        _graph.createTransition(oldNode, newNode, traceCrossed.getIdTrace());
                        _graph.createTransition(newNode, newNode, newTrace.getIdTrace());

                        setCurrentTrace();
                    }
                    else
                    {
                        int previousTraceId = _traces[_traces.Count - 1].getIdTrace();

                        Trace newTrace = createNewTrace(traceCrossed, j);
                        if (newTrace == null)
                            return false;

                        Edge edge = _graph.getEdgeWithId(traceCrossed.getIdTrace());

                        Node node1 = edge.getNodeFirst();
                        Node node2 = edge.getNodeLast();

                        node1.removeNeighbour(node2);
                        node2.removeNeighbour(node1);

                        _graph.deleteTransition(edge);

                        Node previousNode = _graph.getNodes()[_graph.getNodes().Count - 1];
                        Node newNode = _graph.addNode();
                        newNode.setCrossSign(crossSign);

                        _graph.createTransition(previousNode, newNode, previousTraceId);
                        _graph.createTransition(node1, newNode, traceCrossed.getIdTrace());
                        _graph.createTransition(newNode, node2, newTrace.getIdTrace());

                        setCurrentTrace();
                    }
                    return true;
                }
            }
        }
        return false;
    }

    private Trace createNewTrace(Trace traceCrossed, int j)
     {

        GameObject trace = Instantiate(_traceToCreate, _playerController.transform.position, Quaternion.identity);
        trace.transform.parent = transform;
        Trace newTrace = trace.GetComponent<Trace>();

        for (int k = j; k < traceCrossed.getPoints().Count; k++)
            newTrace.getPoints().Add(new Vector3(traceCrossed.getPoints()[k].x, traceCrossed.getPoints()[k].y));

        if (newTrace.GetComponent<LineRenderer>().numPositions == 0)
        {
            Destroy(newTrace);
            Destroy(trace);
            return null;
        }  

        _traces.Add(newTrace);
        traceCrossed.getPoints().RemoveRange(j, newTrace.getPoints().Count);

        return newTrace;
     }

    private void setCurrentTrace()
    {
        GameObject trace = Instantiate(_traceToCreate, _playerController.transform.position, Quaternion.identity);
        trace.transform.parent = transform;
        _traces.Add(trace.GetComponent<Trace>());
        _playerController.setTrace(trace.GetComponent<Trace>());
    }


    private GameObject InstanceIntersectionIndicator(Vector3 position)
    {
        GameObject intersectionGo = Instantiate(intersectionPrefab);
        intersectionGo.name = "Intersection";
        intersectionGo.transform.position = position;

        return intersectionGo;
    }
}
