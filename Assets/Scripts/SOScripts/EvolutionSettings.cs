using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Evolution Settings", menuName = "Evolution Settings")]
public class EvolutionSettings : ScriptableObject
{
    [Header("Borders")]
    [Range(0, 32)]
    public float minCycleLenght;
    [Range(0, 32)]
    public float maxCycleLenght;

    [Range(0, 8)]
    public float maxIndentFromStart;

    [Tooltip("From 0 to 1 000 000")]
    public float minLinearDrag, maxLinearDrag;

    [Range(0, 8)]
    public float minNormalConnectionLenght, maxNormalConnectionLenght, minExtendedConnectionLenght, maxExtendedConnectionLenght;

    [Tooltip("From 0 to 1 000 000")]
    public float minFrequency, maxFrequency;

    [Range(2, 16)]
    public int minNodeAmount, maxNodeAmount;

    [Header("Mutations")]
    [Header("Chances for small mutations")]
    [Range(0f, 1f)]
    public float scaleCycleChance;

    [Range(0f, 1f)]
    public float extendedTimeMarkerChangeChance, indentFromStartChangeChance, linearDragChangeChance, normalConnectionLenghtChangeChance, extendedConnectionLenghtChangeChance, frequencyChangeChance;

    [Header("Chances for big mutations")]
    [Range(0f, 1f)]
    public float addNodeChance;

    [Range(0f, 1f)]
    public float deleteNodeChance, addConnectionChance, deleteConnectionChance;

[Header("Mins & Maxes")]
    [Range(0f, 8f)]
    public float minScaleCycleFactor;
    [Range(0f, 8f)]
    public float maxScaleCycleFactor;

    [Range(-4f, 4f)]
    public float minExtendedTimeMarkerChange, maxExtendedTimeMarkerChange;

    [Range(-8f, 8f)]
    public float minIndentFromStartChange, maxIndentFromStartChange;

    [Range(-100000, 100000)]
    public float minLinearDragChange, maxLinearDragChange;

    [Range(-4f, 4f)]
    public float minNormalConnectionLenghtChange, maxNormalConnectionLenghtChange;

    [Range(-4f, 4f)]
    public float minExtendedConnectionLenghtChange, maxExtendedConnectionLenghtChange;

    [Range(-100000, 100000)]
    public float minFrequencyChange, maxFrequencyChange;


}
