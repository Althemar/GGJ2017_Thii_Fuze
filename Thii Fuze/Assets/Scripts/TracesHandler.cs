using System.Collections.Generic;
using UnityEngine;

public class TracesHandler : MonoBehaviour {

    public GameObject _player;
    public GameObject _traceToCreate;

    private PlayerController _playerController;
    public List<Trace> _traces;
    private Graph _graph;

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
                if (Vector3.Distance(_traces[i].getPoints()[j], newPos) <= minDistanceBetweenPoints)
                {
                    Trace traceCrossed = _traces[i];

                    if (!_graph.traceExists(traceCrossed.getIdTrace()))
                    {
                        
                        // Create a new Trace and add all the points > i in the new trace;

                        GameObject trace = Instantiate(_traceToCreate, _playerController.transform.position, Quaternion.identity);
                        trace.transform.parent = transform;
                        Trace newTrace = trace.GetComponent<Trace>();

                        
                        for (int k = j; k < traceCrossed.getPoints().Count; k++)
                            newTrace.getPoints().Add(new Vector3(traceCrossed.getPoints()[k].x, traceCrossed.getPoints()[k].y));
                        _traces.Add(newTrace);

                        // Delete the points added in the new trace
                        traceCrossed.getPoints().RemoveRange(j, newTrace.getPoints().Count);

                        Node oldNode = _graph.getNodes()[_graph.getNodes().Count - 1];
                        Node newNode = _graph.addNode();

                        _graph.createTransition(oldNode, newNode, traceCrossed.getIdTrace());
                        _graph.createTransition(newNode, newNode, newTrace.getIdTrace());

                        trace = Instantiate(_traceToCreate, _playerController.transform.position, Quaternion.identity);
                        trace.transform.parent = transform;
                        newTrace = trace.GetComponent<Trace>();
                        _traces.Add(newTrace);
                        _playerController.setTrace(newTrace);

                    }
                    else
                    {

                    }
                    return true;
                }
            }

        }
        return false;
    }
}
