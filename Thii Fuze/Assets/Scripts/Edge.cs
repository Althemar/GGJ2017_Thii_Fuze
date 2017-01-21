using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{

    private int _id;
    private Node _node1;
    private Node _node2;

    public Edge(Node node1, Node node2, int id)
    {
        _id = id;
        _node1 = node1;
        _node2 = node2;
    }

    /*
     * Getters
     */

    public int getId()
    {
        return _id;
    }

    public Node getNode1()
    {
        return _node1;
    }

    public Node getNode2()
    {
        return _node2;
    }

}
