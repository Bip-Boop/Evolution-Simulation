using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature
{
    public float Value { get; set; }

    public Creature()
    {
        Nodes = new List<Node>();
    }

    public bool HasConnections()
    {
        foreach (var node in Nodes)
        {
            if (node.ConnectedWith.Count != 0)
                return true;
        }

        return false;
    }

    public void AddNodeSafely(Node node)
    {
        if (!Nodes.Contains(node))
        {
            Nodes.Add(node);
        }
    }

    public List<Node> Nodes { get; set; } 
    public float CycleLenght { get; set; }
}
