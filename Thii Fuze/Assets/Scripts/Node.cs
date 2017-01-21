using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{

    private int _id;
    private List<Node> _neighbours;

    public Node(int id)
    {
        _neighbours = new List<Node>();
        _id = id;
    }

    /*
     * Getters
     */

    public int getId()
    {
        return _id;
    }

    public List<Node> getNeighbours()
    {
        return _neighbours;
    }

    /*
     * Methods
     */

    public void addNeighbour(Node node)
    {
        _neighbours.Add(node);
    }

    public void removeNeighbour(Node node)
    {
        _neighbours.Remove(node);
    }
}
