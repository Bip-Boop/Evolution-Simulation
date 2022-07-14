using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Test : MonoBehaviour
{
    [SerializeField] BuildCreature _builder;
    [SerializeField] EvolutionSettings _evolutionSettings;

    private void Start()
    {
        StartCoroutine(test5());

    }

    private IEnumerator test0()
    {
        yield return new WaitForSecondsRealtime(5f);
        Creature c = new Creature();
        c.CycleLenght = 2f;

        Node n1 = new Node(c);
        n1.LinearDrag = 0.5f;
        Node n2 = new Node(c);
        n2.LinearDrag = 1f;
        Node n3 = new Node(c);
        n3.LinearDrag = 0f;

        n1.PositionRelativeToStartPoint = Vector2.up * 2f;
        n2.PositionRelativeToStartPoint = Vector2.left;
        n3.PositionRelativeToStartPoint = Vector2.right;

        n1.AddConnection(new Connection(n2, 1f, 2f, 1f, 1f));
        n2.AddConnection(new Connection(n3, 0.5f, 1f, 0.5f, 1.5f));
        n3.AddConnection(new Connection(n1, 1f, 2f, 1f, 1f));

        c.Nodes.Add(n1);
        c.Nodes.Add(n2);
        c.Nodes.Add(n3);

        StartTrigger st = _builder.SetupCreature(c, Vector2.up * 5f);
        st.Invoke();
    }

    private IEnumerator test1()
    {
        yield return new WaitForSecondsRealtime(3f);
        CreatureFactory cf = new CreatureFactory(322);
        Creature c = cf.RandomCreature(1f, 3f, 3, 5, 3f, 0f, 1f, 0.5f, 2f, 0.5f, 3f, 1f, 5f);
        StartTrigger st = _builder.SetupCreature(c, Vector2.up * 3f);
        st.Invoke();
        PrintDataAboutCreature(c);
    }

    private IEnumerator test2()
    {
        yield return new WaitForSeconds(3f);

        CreatureFactory cf = new CreatureFactory();
        Creature c = cf.RandomCreature(1f, 3f, 3, 5, 3f, 0f, 1f, 0.5f, 2f, 0.5f, 3f, 1f, 5f);
        StartTrigger st = _builder.SetupCreature(c, Vector2.up * 2f);
        st.Invoke();

        yield return new WaitForSeconds(20f);
        Creature cABitMutated = cf.SmallMutation(c, 0.1f, 0.4f, 2f, 0.5f, -0.1f, 0.3f, 0.3f, -0.1f, 0.5f, 0.2f, -0.1f, 0.1f, 0.3f, -0.5f, 0.5f, 0.1f, -0.3f, 0.3f, 0.5f, -0.5f, 0.5f);
        st = _builder.SetupCreature(cABitMutated, Vector2.up * 2f);
        st.Invoke();
    }

    private IEnumerator test3()
    {
        yield return new WaitForSeconds(3f);

        CreatureFactory cf = new CreatureFactory();
        Creature c = cf.RandomCreature(1f, 3f, 3, 5, 3f, 0f, 1f, 0.5f, 2f, 0.5f, 3f, 1f, 5f);
        StartTrigger st = _builder.SetupCreature(c, Vector2.up * 2f);
        st.Invoke();
        PrintDataAboutCreature(c);

        yield return new WaitForSeconds(3f);
        cf = new CreatureFactory(_evolutionSettings);
        Creature cABitMutated = cf.BigMutation(c);
        st = _builder.SetupCreature(cABitMutated, Vector2.up * 2f);
        st.Invoke();
        PrintDataAboutCreature(cABitMutated);
    }

    private IEnumerator test4()
    {
        yield return new WaitForSeconds(3f);

        CreatureFactory cf = new CreatureFactory(_evolutionSettings);
        Creature c = cf.RandomCreature();
        var start = _builder.SetupCreature(c, Vector2.up + Vector2.right * 4f);
        start.Invoke();

        yield return new WaitForSeconds(3f);

        c = cf.SmallMutation(cf.BigMutation(c));
        start = _builder.SetupCreature(c, Vector2.up + Vector2.left * 4f);
        start.Invoke();
    }

    private IEnumerator test5()
    {
        Time.timeScale = 10f;
        yield return new WaitForSeconds(3f);

        CreatureFactory cf = new CreatureFactory(_evolutionSettings);

        for (int j = 0; j < 8; j++)
        {
            for (int i = 0; i < 128; i++)
            {
                _builder.SetupCreature(cf.RandomCreature(), Vector2.up * 0.5f).Invoke();
            }
            System.GC.Collect();
            yield return new WaitForSeconds(20f);
        }
    }

    public void PrintDataAboutCreature(Creature creature)
    {
        Debug.Log("CREATURE:");
        Debug.Log("Cycle.lengh " + creature.CycleLenght);

        foreach (var node in creature.Nodes)
        {
            Debug.Log("Linear drag " + node.LinearDrag);
            Debug.Log("Position relative to start " + node.PositionRelativeToStartPoint);
            Debug.Log("Connections: ");
            foreach (var connection in node.ConnectedWith)
            {
                Debug.Log("Normal lenght " + connection.NormalLenght);
                Debug.Log("Extended lenght " + connection.ExtendedLenght);
                Debug.Log("Extended Time Marker " + connection.ExtendedTimeMarker);
                Debug.Log("Frequency " + connection.Frequency);
            }
        }
    }
}
