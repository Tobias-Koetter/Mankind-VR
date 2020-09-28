using System.Collections.Generic;
using UnityEngine;

public class SpawnAreaTracker : MonoBehaviour
{
    public int AreasToStore = 3;
    public List<Transform> touchedSpawnAreas;

    private Collider last = null;
    private int index = -1;

    public Transform getVisitedArea()
    {
        return touchedSpawnAreas[Random.Range(0, AreasToStore - 1)];
    }
    public Transform getCurrentVisited()
    {
        return touchedSpawnAreas[AreasToStore - 1];
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("SpawnArea"))
        {
            if (!last || last != other)
            {
                if (touchedSpawnAreas.Count == 0)
                {
                    touchedSpawnAreas.Add(other.transform);
                    last = other;
                }
                else if ((index = touchedSpawnAreas.FindIndex(x => x == other.transform) )!= -1)
                {
                    touchedSpawnAreas.RemoveAt(index);
                    touchedSpawnAreas.Add(other.transform);
                    last = other;
                }
                else
                {
                    touchedSpawnAreas.Add(other.transform);
                    last = other;
                }

                if(touchedSpawnAreas.Count > AreasToStore)
                {
                    touchedSpawnAreas.RemoveAt(0);
                }
            }
        }
    }

}
