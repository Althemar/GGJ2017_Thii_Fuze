using System.Collections.Generic;
using UnityEngine;

public class TracesHandler : MonoBehaviour {

    public GameObject _player;
    public GameObject _traceToCreate;

    private PlayerController _playerController;
    public List<Trace> _traces;
    private Graph _graph;

    private Vector3 _lastTraceCross;

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
    }

    // Update is called once per frame
    void Update() {

    }

    public void beginBurn()
    {
        if (_traces.Count != 0)
        {
            _traces[0].activateDeleting();
            _traces[0].burnFirst(true);
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
                if (Vector3.Distance(_traces[i].getPoints()[j], newPos) <= minDistanceBetweenPoints && Vector3.Distance(_lastTraceCross, newPos) >= minDistanceBetweenPoints)
                {

                    Trace traceCrossed = _traces[i];
                    _lastTraceCross = newPos;

                    if (!_graph.traceExists(traceCrossed.getIdTrace()))
                    {
                        Trace newTrace = createNewTrace(traceCrossed, j);
                        if (newTrace == null)
                            return false;

                        Node oldNode = _graph.getNodes()[_graph.getNodes().Count - 1];
                        Node newNode = _graph.addNode();

                        _graph.createTransition(oldNode, newNode, traceCrossed.getIdTrace());
                        _graph.createTransition(newNode, newNode, newTrace.getIdTrace());

                        setCurrentTrace();
                    }
                    else
                    {
                        Trace newTrace = createNewTrace(traceCrossed, j);
                        if (newTrace == null)
                            return false;

                        Edge edge = _graph.getEdgeWithId(traceCrossed.getIdTrace());
                        
                        Node node1 = edge.getNode1();
                        Node node2 = edge.getNode2();

                        Node newNode = _graph.addNode();

                        _graph.createTransition(node1, newNode, traceCrossed.getIdTrace());
                        _graph.createTransition(node2, newNode, newTrace.getIdTrace());

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
        Trace newTrace = trace.GetComponent<Trace>();
        _traces.Add(newTrace);
        _playerController.setTrace(newTrace);
    }

}
