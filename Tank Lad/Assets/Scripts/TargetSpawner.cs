using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
            List<GameObject> emptyPoints = new List<GameObject>();
            int randomPoint;

            yield return new WaitForSeconds(2f);

            foreach(GameObject s in spawnPoints)
            {
                if (s.transform.childCount == 0)
                {
                    emptyPoints.Add(s);
                }
            }

            if (emptyPoints.Count > 0)
            {
                randomPoint = Random.Range(0, emptyPoints.Count);
                Instantiate(target, emptyPoints.ElementAt(randomPoint).transform);
            }
        }
    }
}
