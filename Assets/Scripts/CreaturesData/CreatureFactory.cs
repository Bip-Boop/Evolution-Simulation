using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CreatureFactory
{
    //TODO: make method parameters private members of class
    private System.Random _random;
    public int Seed { get; private set; }
    private Vector2 _setVector;
    public EvolutionSettings CurrentEvolutionSettings { get; private set; }

    public CreatureFactory()
    {
        //auto Seed
        System.Random r = new System.Random();
        Seed = r.Next();
        Random.InitState(Seed);
    }

    public CreatureFactory(int seed)
    {
        Seed = seed;
        Random.InitState(seed);
    }

    public CreatureFactory(int seed, EvolutionSettings evolutionSettings) : this(seed)
    {
        CurrentEvolutionSettings = evolutionSettings;
    }

    public CreatureFactory(EvolutionSettings evolutionSettings) : this()
    {
        CurrentEvolutionSettings = evolutionSettings;
    }



    public Creature RandomCreature(float minCycleLenght, float maxCycleLenght, int minNodeAmount, int maxNodeAmount, float maxIndentFromStart, float minLinearDrag, float maxLinearDrag, float minNormalConnectionLenght, float maxNormalConnectionLenght, float minExtendedConnectionLenght, float maxExtendedConnectionLenght, float minFrequency, float maxFrequency)
    {
        Creature c = new Creature() { CycleLenght = Random.Range(minCycleLenght, maxCycleLenght) };

        int nodesAmount = Random.Range(minNodeAmount, maxNodeAmount + 1);
        Node[] allNodes = new Node[nodesAmount];

        for (int i = 0; i < nodesAmount; i++)
        {
            Node n = new Node(c)
            {
                LinearDrag = Random.Range(minLinearDrag, maxLinearDrag)
            };
            _setVector.Set(Random.Range(-maxIndentFromStart, maxIndentFromStart), Random.Range(0f, maxIndentFromStart));
            n.PositionRelativeToStartPoint = _setVector;
            allNodes[i] = n;
        }

        for (int i = 0; i < nodesAmount; i++)
        {
            int connectionsAmount = Random.Range(1, nodesAmount);
            int[] connections = Enumerable.Range(0, nodesAmount - 1).OrderBy(x => Random.Range(0, nodesAmount)).ToArray();

            for (int j = 0; j < connectionsAmount; j++)
            {
                if (!allNodes[i].ConnectedWithNode(allNodes[connections[j]]) && !allNodes[connections[j]].ConnectedWithNode(allNodes[i])) //weird
                {
                    allNodes[i].AddConnection(new Connection(allNodes[connections[j]],
                         Random.Range(minNormalConnectionLenght, maxNormalConnectionLenght),
                         Random.Range(minExtendedConnectionLenght, maxExtendedConnectionLenght),
                         Random.Range(minFrequency, maxFrequency),
                         Random.value * c.CycleLenght));
                }
            }
        }

        for (int i = 0; i < nodesAmount; i++)
        {
            c.AddNodeSafely(allNodes[i]);
        }

        return c;
    }

    public Creature RandomCreature()
    {
        return RandomCreature(CurrentEvolutionSettings.minCycleLenght, CurrentEvolutionSettings.maxCycleLenght, CurrentEvolutionSettings.minNodeAmount, CurrentEvolutionSettings.maxNodeAmount, CurrentEvolutionSettings.maxIndentFromStart, CurrentEvolutionSettings.minLinearDrag, CurrentEvolutionSettings.maxLinearDrag, CurrentEvolutionSettings.minNormalConnectionLenght, CurrentEvolutionSettings.maxNormalConnectionLenght, CurrentEvolutionSettings.minExtendedConnectionLenght, CurrentEvolutionSettings.maxExtendedConnectionLenght, CurrentEvolutionSettings.minFrequency, CurrentEvolutionSettings.maxFrequency);
    }

    public Creature SmallMutation(Creature creature, float scaleCycleChance, float minScaleCycleFactor, float maxScaleCycleFactor, float extendedTimeMarkerChangeChance, float minExtendedTimeMarkerChange, float maxExtendedTimeMarkerChange, float indentFromStartChangeChance, float minIndentFromStartChange, float maxIndentFromStartChange, float linearDragChangeChance, float minLinearDragChange, float maxLinearDragChange, float normalConnectionLenghtChangeChance, float minNormalConnectionLenghtChange, float maxNormalConnectionLenghtChange, float extendedConnectionLenghtChangeChance, float minExtendedConnectionLenghtChange, float maxExtendedConnectionLenghtChange, float frequencyChangeChance, float minFrequencyChange, float maxFrequencyChange)
    {
        if (Random.value < scaleCycleChance)
        {
            float factor = Random.Range(minScaleCycleFactor, maxScaleCycleFactor);
            creature.CycleLenght *= factor;

            foreach (var node in creature.Nodes)
            {
                foreach (var connection in node.ConnectedWith)
                {
                    connection.ExtendedTimeMarker *= factor;
                }
            }
        }

        foreach (var node in creature.Nodes)
        { 
            if (Random.value < indentFromStartChangeChance)
            {
                _setVector.Set(Random.Range(minIndentFromStartChange, maxIndentFromStartChange), Random.Range(minIndentFromStartChange, maxIndentFromStartChange));
                node.PositionRelativeToStartPoint += _setVector;
                if (node.PositionRelativeToStartPoint.y < 0f)
                {
                    _setVector.Set(node.PositionRelativeToStartPoint.x, 0f);
                    node.PositionRelativeToStartPoint = _setVector;
                }
            }

            if (Random.value < linearDragChangeChance)
            {
                node.LinearDrag += Random.Range(minLinearDragChange, maxLinearDragChange);

                if (node.LinearDrag < 0f)
                    node.LinearDrag = 0f;
            }

            foreach (var connection in node.ConnectedWith)
            {
                if (Random.value < extendedTimeMarkerChangeChance)
                {
                    float extendedTimeMarkerChanged = Random.Range(minExtendedTimeMarkerChange, maxExtendedTimeMarkerChange) + connection.ExtendedTimeMarker;
                    connection.ExtendedTimeMarker = extendedTimeMarkerChanged > creature.CycleLenght ? creature.CycleLenght : extendedTimeMarkerChanged;
                    connection.ExtendedTimeMarker = extendedTimeMarkerChanged < 0f ? 0f : extendedTimeMarkerChanged;
                }

                if (Random.value < extendedConnectionLenghtChangeChance)
                {
                    connection.ExtendedLenght += Random.Range(minExtendedConnectionLenghtChange, maxExtendedConnectionLenghtChange) + connection.ExtendedLenght;
                    if (connection.ExtendedLenght < 0.05f)
                        connection.ExtendedLenght = 0.05f;
                }

                if (Random.value < normalConnectionLenghtChangeChance)
                { 
                    connection.NormalLenght += Random.Range(minNormalConnectionLenghtChange, maxNormalConnectionLenghtChange) + connection.NormalLenght;
                    if (connection.NormalLenght < 0.05f)
                        connection.NormalLenght = 0.05f;
                }

                if (Random.value < frequencyChangeChance)
                {
                    connection.Frequency += Random.Range(minFrequencyChange, maxFrequencyChange);
                    if (connection.Frequency < 0.05f)
                        connection.Frequency = 0.05f;
                }
            }
        }

        return creature;
    }

    //float scaleCycleChance, float minScaleCycleFactor, float maxScaleCycleFactor, float extendedTimeMarkerChangeChance, float minExtendedTimeMarkerChange, float maxExtendedTimeMarkerChange, float indentFromStartChangeChance, float minIndentFromStartChange, float maxIndentFromStartChange, float linearDragChangeChance, float minLinearDragChange, float maxLinearDragChange, float normalConnectionLenghtChangeChance, float minNormalConnectionLenghtChange, float maxNormalConnectionLenghtChange, float extendedConnectionLenghtChangeChance, float minExtendedConnectionLenghtChange, float maxExtendedConnectionLenghtChange, float frequencyChangeChance, float minFrequencyChange, float maxFrequencyChange
    public Creature SmallMutation(Creature creature)
    {
        return SmallMutation(creature, CurrentEvolutionSettings.scaleCycleChance, CurrentEvolutionSettings.minScaleCycleFactor, CurrentEvolutionSettings.maxScaleCycleFactor, CurrentEvolutionSettings.extendedConnectionLenghtChangeChance, CurrentEvolutionSettings.minExtendedConnectionLenghtChange, CurrentEvolutionSettings.maxExtendedConnectionLenghtChange, CurrentEvolutionSettings.indentFromStartChangeChance, CurrentEvolutionSettings.minIndentFromStartChange, CurrentEvolutionSettings.maxIndentFromStartChange, CurrentEvolutionSettings.linearDragChangeChance, CurrentEvolutionSettings.minLinearDragChange, CurrentEvolutionSettings.maxLinearDragChange, CurrentEvolutionSettings.normalConnectionLenghtChangeChance, CurrentEvolutionSettings.minNormalConnectionLenghtChange, CurrentEvolutionSettings.maxNormalConnectionLenghtChange, CurrentEvolutionSettings.extendedConnectionLenghtChangeChance, CurrentEvolutionSettings.minExtendedConnectionLenghtChange, CurrentEvolutionSettings.maxExtendedConnectionLenghtChange, CurrentEvolutionSettings.frequencyChangeChance, CurrentEvolutionSettings.minFrequencyChange, CurrentEvolutionSettings.maxFrequencyChange);
    }

    public Creature BigMutation(Creature creature, float addNodeChance, float deleteNodeChance, float addConnectionChance, float deleteConnectionChance, float minLinearDrag, float maxLinearDrag, float maxIndentFromStart, float minNormalConnectionLenght, float maxNormalConnectionLenght, float minExtendedConnectionLenght, float maxExtendedConnectionLenght, float minFrequency, float maxFrequency, float cycleLenght)
    {
        if (Random.value < addNodeChance)
        {
            Node node = CreateNewRandomNode(creature, minLinearDrag, maxLinearDrag, maxIndentFromStart);
            creature.Nodes[Random.Range(0, creature.Nodes.Count)].AddConnection(CreateNewRandomConnectionWithNode(node, minNormalConnectionLenght, maxNormalConnectionLenght, minExtendedConnectionLenght, maxExtendedConnectionLenght, minFrequency, maxFrequency, creature.CycleLenght));
        }

        if (Random.value < deleteNodeChance && creature.Nodes.Count > 0)
        {
            Node deleteNode = creature.Nodes[Random.Range(0, creature.Nodes.Count)];

            foreach (var connection in deleteNode.ConnectedWith)
            {
                deleteNode.RemoveConnection(connection);
            }

            foreach (var node in creature.Nodes)
            {
                foreach (var connection in node.ConnectedWith)
                {
                    if (connection.ConnectedToNode == deleteNode)
                        node.RemoveConnection(connection);
                }
            }

            creature.Nodes.Remove(deleteNode);
        }

        if (Random.value < addConnectionChance && creature.Nodes.Count > 1)
        {
            Node selected = creature.Nodes[Random.Range(0, creature.Nodes.Count)];
            creature.Nodes.FindAll(x => x != selected)[Random.Range(0, creature.Nodes.Count - 1)].AddConnection(CreateNewRandomConnectionWithNode(selected, minNormalConnectionLenght, maxNormalConnectionLenght, minExtendedConnectionLenght, maxExtendedConnectionLenght, minFrequency, maxFrequency, creature.CycleLenght));
        }

        if (Random.value < deleteConnectionChance && creature.HasConnections())
        {
            List<Connection> allConnections = new List<Connection>();
            List<Node> NodesFromWhichConnectionsGoes = new List<Node>();

            foreach (var node in creature.Nodes)
            {
                foreach (var connection in node.ConnectedWith)
                {
                    allConnections.Add(connection);
                    NodesFromWhichConnectionsGoes.Add(node);
                }
            }

            int randIndex = Random.Range(0, allConnections.Count);
            NodesFromWhichConnectionsGoes[randIndex].RemoveConnection(allConnections[randIndex]);
        }



        return creature;
    }

    public Creature BigMutation(Creature creature)
    {
        return BigMutation(creature, CurrentEvolutionSettings.addNodeChance, CurrentEvolutionSettings.deleteNodeChance, CurrentEvolutionSettings.addConnectionChance, CurrentEvolutionSettings.deleteConnectionChance, CurrentEvolutionSettings.minLinearDrag, CurrentEvolutionSettings.maxLinearDrag, CurrentEvolutionSettings.maxIndentFromStart, CurrentEvolutionSettings.minNormalConnectionLenght, CurrentEvolutionSettings.maxNormalConnectionLenght, CurrentEvolutionSettings.minExtendedConnectionLenght, CurrentEvolutionSettings.maxExtendedConnectionLenght, CurrentEvolutionSettings.minFrequency, CurrentEvolutionSettings.maxFrequency, creature.CycleLenght);
    }

    private Node CreateNewRandomNode(Creature c, float minLinearDrag, float maxLinearDrag, float maxIndentFromStart)
    {
        Node n = new Node(c)
        {
            LinearDrag = Random.Range(minLinearDrag, maxLinearDrag)
        };
        _setVector.Set(Random.Range(-maxIndentFromStart, maxIndentFromStart), Random.Range(0f, maxIndentFromStart));
        n.PositionRelativeToStartPoint = _setVector;

        return n;
    }

    private Connection CreateNewRandomConnectionWithNode(Node node, float minNormalConnectionLenght, float maxNormalConnectionLenght, float minExtendedConnectionLenght, float maxExtendedConnectionLenght, float minFrequency, float maxFrequency, float cycleLenght)
    {
        Connection connection = new Connection(node,
                         Random.Range(minNormalConnectionLenght, maxNormalConnectionLenght),
                         Random.Range(minExtendedConnectionLenght, maxExtendedConnectionLenght),
                         Random.Range(minFrequency, maxFrequency),
                         Random.value * cycleLenght);

        return connection;
    }


}
