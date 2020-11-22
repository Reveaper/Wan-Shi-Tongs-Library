using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FloorSet
{
    Floor1In, Floor1Out, Floor2In, Floor2Out, Floor3In, Floor3Out
}

public class PerformanceHandler : MonoBehaviour
{
    public List<GameObject> Floor1In;
    public List<GameObject> Floor1Out;
    public List<GameObject> Floor2In;
    public List<GameObject> Floor2Out;
    public List<GameObject> Floor3In;
    public List<GameObject> Floor3Out;

    public void ChangeObjectStates(List<GameObject> objects, bool state)
    {
        foreach (GameObject gameObject in objects)
            gameObject.SetActive(state);
    }

    public void ChangeFloorSetState(FloorSet floorSet, bool state)
    {
        if (floorSet == FloorSet.Floor1In)
            ChangeObjectStates(Floor1In, state);
        if (floorSet == FloorSet.Floor1Out)
            ChangeObjectStates(Floor1Out, state);
        if (floorSet == FloorSet.Floor2In)
            ChangeObjectStates(Floor2In, state);
        if (floorSet == FloorSet.Floor2Out)
            ChangeObjectStates(Floor2Out, state);
        if (floorSet == FloorSet.Floor3In)
            ChangeObjectStates(Floor3In, state);
        if (floorSet == FloorSet.Floor3Out)
            ChangeObjectStates(Floor3Out, state);
    }
}
