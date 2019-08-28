using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnPoints;
    [SerializeField] private GameObject target;

    private bool spawning = false;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = new List<GameObject>();

        for (int i = 0; i < this.transform.childCount; i++)
        {
            spawnPoints.Add(this.transform.GetChild(i).gameObject);
        }

        StartCoroutine(CheckSpawnPoints());
    }

    IEnumerator CheckSpawnPoints()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            foreach(GameObject s in spawnPoints)
            {
                if (s.transform.childCount == 0)
                {
                    Instantiate(target, s.transform);
                }
            }
        }
    }
}
