using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformancesAccumulator
{
    private float[] _values;
    private Vector2 _startPoint;
    public PerformancesAccumulator(int testeeCount, Vector2 startPoint)
    {
        _values = new float[testeeCount];
        _startPoint = startPoint;
    }

    public void EndOfPerformance(int id, Creature c)
    {
        c.Value = _values[id];
    }

    public void NewAnalysisFrame(int id, float value)
    {
        if (_values[id] < value)
        {
            _values[id] = value;
        }
    }

    public float Vector2ToValueByDistanceFromStart(Vector2 pos)
    {
        return Vector2.Distance(_startPoint, pos);
    }

    public float FindTheMostValuableNode(float[] values)
    {
        float result = values[0];
        for (int i = 1; i < values.Length; i++)
        {
            if (result < values[i])
            {
                result = values[i];
            }
        }

        return result;
    }


}
