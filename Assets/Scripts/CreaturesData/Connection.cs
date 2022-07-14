using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection
{
    public Connection(Node connectedToNode, float normalLenght, float extendedLenght, float frequency, float extendedTimeMarker)
    {
        ConnectedToNode = connectedToNode;
        NormalLenght = normalLenght;
        ExtendedLenght = extendedLenght;
        Frequency = frequency;
        ExtendedTimeMarker = extendedTimeMarker;
    }

    public Node ConnectedToNode { get; private set; }

    public float NormalLenght { get; set; }
    public float ExtendedLenght { get; set; }
    public float Frequency{ get; set; }

    public float ExtendedTimeMarker { get; set; }

}
