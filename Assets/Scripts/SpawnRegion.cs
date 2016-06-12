using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnRegion : MonoBehaviour {

    public static SpawnRegion main;

    [System.Serializable]
	public class SpawnPath
    {
        public EnemyDataStore receipt;
        public Vector3[] points = new Vector3[0];

        public bool intersects(Rect other)
        {
            foreach (Vector3 point in points)
            {
                if (other.Contains(point))
                {
                    return true;
                }
            }

            return false;
        }

        GameObject[] instances;
        public bool stillAlive()
        {
            if (instances != null)
            {
                foreach (GameObject obj in instances)
                {
                    if (obj)
                    {
                        return true;
                    }
                }

                instances = null;
            }

            return false;
        }

        public void Summon(SpawnRegion region)
        {
            if (region.paths.Remove(this))
            {
                region.spawnedPaths.Add(this);

                List<int> order = new List<int>();
                for (int i=0; i<this.points.Length; i++)
                {
                    order.Add(i);
                }

                this.instances = new GameObject[this.points.Length];
                foreach (EnemyData enemy in receipt.enemyTypes)
                {
                    int orderId = Random.Range(0, order.Count);
                    int id = order[orderId];
                    order.RemoveAt(orderId);

                    this.instances[id] = enemy.SummonAt(this.points[id], region.transform.parent);
                }
            }
        }
    }

    public List<SpawnPath> paths = new List<SpawnPath>();
    public int initialSpawnCount = 5;
    public int maximumSpawnCount = 7;
    
    private List<SpawnPath> spawnedPaths = new List<SpawnPath>();
    private IEnumerator crSpawn;
    
    void Awake()
    {
        main = this;
    }

    void Start()
    {
        for (int i=0; i<this.initialSpawnCount && i<paths.Count; i++)
        {
            paths[Random.Range(0, paths.Count)].Summon(this);
        }

        StartCoroutine(crSpawn = EnemySpawner());
    }

    public static Vector2 GetWorld(Ray r)
    {
        return r.origin - r.direction / r.direction.z * r.origin.z;
    }

    IEnumerator EnemySpawner()
    {
        while (this)
        {
            spawnedPaths.RemoveAll((path) => {
                if (path.stillAlive())
                {
                    return false;
                }
                else
                {
                    paths.Add(path);
                    return true;
                }
            });

            if (spawnedPaths.Count < maximumSpawnCount)
            {
                Camera cam = Camera.main;
                Vector2 bottomLeft = GetWorld(cam.ViewportPointToRay(new Vector3(0, 0, 0)));
                Vector2 topRight = GetWorld(cam.ViewportPointToRay(new Vector3(1, 1, 0)));

                if (paths.Count > 0)
                {
                    SpawnPath chosenOne = null;

                    Rect cameraWorldRect = new Rect(bottomLeft, topRight - bottomLeft);
                    for (int i = 0; i < 5; i++)
                    {
                        SpawnPath path = paths[Random.Range(0, paths.Count)];
                        if (path.intersects(cameraWorldRect))
                        {
                            chosenOne = path;
                            break;
                        }
                    }

                    if (chosenOne != null)
                    {
                        chosenOne.Summon(this);
                    }
                }
            }

            yield return new WaitForSeconds(0.5f);
        }

        yield return null;
    }

}
