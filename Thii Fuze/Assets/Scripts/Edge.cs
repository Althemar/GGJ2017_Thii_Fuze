public class Edge
{

    private int _id;

    private Node _nodeFirst;
    private Node _nodeLast;

    public Edge(Node node1, Node node2, int id)
    {
        _id = id;

        _nodeFirst = node1;
        _nodeLast = node2;
    }

    /*
     * Getters
     */

    public int getId()
    {
        return _id;
    }

    public Node getNodeFirst()
    {
        return _nodeFirst;
    }

    public Node getNodeLast()
    {
        return _nodeLast;
    }
}
