using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void StartTrigger();

public class BuildCreature : MonoBehaviour
{
    //debug
    [SerializeField] private UnityEngine.UI.Text _txt;

    [Header("Simulation Settings")]
    [SerializeField] private float _performanceTime;
    [SerializeField] private int _creaturesAmount;
    [SerializeField] private Vector2 _pointOfMeasuring;

    [Header("Stuff")]
    [SerializeField] private ObjectPooler _pool;
     

    private Dictionary<Node, Rigidbody2D> _virtualToRealNode;
    private List<SpringJoint2D> _connections;
    private List<Connection> _connectionsData;

    private PerformancesAccumulator _performancesAccumulator;
    private int _lastId = 0;
    private List<GameObject> _nodesObjects;

    private void Start()
    {
        _virtualToRealNode = new Dictionary<Node, Rigidbody2D>();
        _connections = new List<SpringJoint2D>();
        _connectionsData = new List<Connection>();
        _performancesAccumulator = new PerformancesAccumulator(_creaturesAmount, _pointOfMeasuring);
        _nodesObjects = new List<GameObject>();
    }


    public StartTrigger SetupCreature(Creature creature, Vector2 startPoint)
    {
        _virtualToRealNode.Clear();
        _nodesObjects.Clear();

        //setup nodes
        foreach (Node node in creature.Nodes)
        {
            var physicalNode =_pool.SpawnFromPool("node");
            physicalNode.transform.position = startPoint + node.PositionRelativeToStartPoint;

            Rigidbody2D rbOfNode = physicalNode.GetComponent<Rigidbody2D>();
            rbOfNode.drag = node.LinearDrag;
            _virtualToRealNode.Add(node, rbOfNode);
            _nodesObjects.Add(rbOfNode.gameObject);
        }

        //setup connections
        _connections.Clear();
        _connectionsData.Clear();
        foreach (Node node in creature.Nodes)
        {
            foreach (Connection connection in node.ConnectedWith)
            {
                SpringJoint2D physicalConnection = _virtualToRealNode[node].gameObject.AddComponent<SpringJoint2D>();
                physicalConnection.connectedBody = _virtualToRealNode[connection.ConnectedToNode];
                physicalConnection.frequency = connection.Frequency;

                _connections.Add(physicalConnection);
                _connectionsData.Add(connection);
            }
        }

        StartTrigger st = () => StartCoroutine(MoveCycle(creature.CycleLenght, _connections.ToArray(), _connectionsData.ToArray(), _nodesObjects.ToArray(), creature));
        return st;
    }

    private IEnumerator MoveCycle(float cycleLenght, SpringJoint2D[] connections, Connection[] connectionsData, GameObject[] nodes, Creature c)
    {
        float t = 0f;
        float wholePerfomanceTimer = 0f;
        int id = _lastId++;
        float[] values = new float[nodes.Length];

        while (wholePerfomanceTimer < _performanceTime)
        {
            for (int i = 0; i < connectionsData.Length; i++)
            {
                if (t >= connectionsData[i].ExtendedTimeMarker)
                {
                    connections[i].distance = connectionsData[i].ExtendedLenght;
                }
                else
                {
                    connections[i].distance = connectionsData[i].NormalLenght;
                }
            }

            t += Time.deltaTime;
            wholePerfomanceTimer += Time.deltaTime;

            if (t > cycleLenght)
                t = 0f;

            //end of performance frame
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = _performancesAccumulator.Vector2ToValueByDistanceFromStart(nodes[i].transform.position);
            }

            _performancesAccumulator.NewAnalysisFrame(id, _performancesAccumulator.FindTheMostValuableNode(values));
            yield return 0;
        }

        //performance ended
        _performancesAccumulator.EndOfPerformance(id, c);
        print(c.Value);

        foreach (var go in nodes)
        {
            go.gameObject.SetActive(false);
        }

        _txt.text = "Last: " + c.Value;
    }




}
