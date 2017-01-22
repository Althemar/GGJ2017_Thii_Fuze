using System.Collections.Generic;
using UnityEngine;

public class Node
{

    private int _id;
    private List<Node> _neighbours;
    private GameObject _crossSign;

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

    public GameObject getCrossSign()
    {
        return _crossSign;
    }

    public void setCrossSign(GameObject crossSign)
    {
        _crossSign = crossSign;
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

    public void release()
    {
        GameObject.Destroy(_crossSign);
    }
}
