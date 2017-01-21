using System.Collections.Generic;

public class Graph
{

    private List<Node> _nodes;
    private List<Edge> _edges;

    public Graph()
    {
        _nodes = new List<Node>();
        _edges = new List<Edge>();
    }

    /*
     * Getters
     */

    public List<Node> getNodes()
    {
        return _nodes;
    }

    public List<Edge> getEdges()
    {
        return _edges;
    }

    /*
     *  Methods
     */

    public Node addNode()
    {
        Node node = new Node(_nodes.Count);
        _nodes.Add(node);
        return node;
    }

    public void createTransition(Node node1, Node node2, int idTransition)
    {
        Edge edge = new Edge(node1, node2, idTransition);
        _edges.Add(edge);

        node1.addNeighbour(node2);
        node2.addNeighbour(node1);
    }

    public void deleteTransition(Edge edge)
    {
        _edges.Remove(edge);
        Node node1 = edge.getNode1();
        Node node2 = edge.getNode2();

        node1.removeNeighbour(node2);
        node2.removeNeighbour(node1);
    }

    public bool traceExists(int idTrace)
    {
        foreach (Edge edge in _edges)
        {
            if (edge.getId() == idTrace)
                return true;
        }
        return false;
    }

    public Edge getEdgeWithId(int id)
    {
        for (int i = 0; i < _edges.Count; i++)
        {
            if (_edges[i].getId() == id)
            {
                return _edges[i];
            }
        }
        return null;
    }

    public string printGraph()
    {
        string nodeString = "";
        foreach (Node node in _nodes)
        {
            nodeString += node.getId().ToString() + " : ";
            foreach (Node neighbour in node.getNeighbours())
            {
                nodeString += neighbour.getId() + ", ";
            }
            nodeString += "\n";
        }
        return nodeString;
    }
}
