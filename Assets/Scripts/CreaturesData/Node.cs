using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private Creature _partOf;
    public Node(Creature partOf)
    {
        ConnectedWith = new List<Connection>();
        _connectedNodes = new HashSet<Node>();
        _connectedNodes.Add(this);
        _partOf = partOf;
    }

    private float _linearDrag;
    public float LinearDrag
    {
        get { return _linearDrag; }
        set { /*NodeRigitbody.drag = value;*/ _linearDrag = value; }
    }

    public List<Connection> ConnectedWith { get; private set; }
    private HashSet<Node> _connectedNodes;
    public bool ConnectedWithNode(Node n)
    {
        if (_connectedNodes.Contains(n))
            return true;
        return false;
    }

    public void AddConnection(Connection connection)
    {
        foreach(var c in ConnectedWith)
        {
            if (c.ConnectedToNode == connection.ConnectedToNode)
                return;
        }

        _connectedNodes.Add(connection.ConnectedToNode);
        ConnectedWith.Add(connection);
        if (!_partOf.Nodes.Contains(connection.ConnectedToNode))
            _partOf.Nodes.Add(connection.ConnectedToNode);
    }

    public void RemoveConnection(Connection connection)
    {
        _connectedNodes.Remove(connection.ConnectedToNode);
        ConnectedWith.Remove(connection);
        _partOf.Nodes.Remove(connection.ConnectedToNode);
    }


    public Vector2 PositionRelativeToStartPoint { get; set; } 
}
