using UnityEngine;

public class TreeRandomSpawner : MonoBehaviour
{
    [Header("基础设置")]
    public Terrain targetTerrain;
    public GameObject treePrefab;
    public int treeCount = 3000;

    [Header("检测禁区")]
    public string noTreeTag = "NoTreeZone"; // 禁区标签
    public float checkRadius = 10f;         // 检测半径，大点更安全

    void Start()
    {
        SpawnTrees();
    }

    void SpawnTrees()
    {
        if (targetTerrain == null || treePrefab == null) return;

        TerrainData td = targetTerrain.terrainData;
        Vector3 terrainPos = targetTerrain.transform.position;
        Vector3 terrainSize = td.size;

        for (int i = 0; i < treeCount; i++)
        {
            // 随机位置
            float x = Random.Range(terrainPos.x, terrainPos.x + terrainSize.x);
            float z = Random.Range(terrainPos.z, terrainPos.z + terrainSize.z);
            float y = targetTerrain.SampleHeight(new Vector3(x, 0, z));
            Vector3 spawnPos = new Vector3(x, y, z);

            // 关键：检查是否在禁区里
            if (Physics.CheckSphere(spawnPos, checkRadius, LayerMask.GetMask("Default")))
            {
                Collider[] hitColliders = Physics.OverlapSphere(spawnPos, checkRadius);
                bool inNoTreeZone = false;

                foreach (var col in hitColliders)
                {
                    if (col.CompareTag(noTreeTag))
                    {
                        inNoTreeZone = true;
                        break;
                    }
                }

                if (inNoTreeZone)
                    continue; // 在禁区 → 跳过，不生成树
            }

            // 生成树
            GameObject t = Instantiate(
                treePrefab,
                spawnPos,
                Quaternion.Euler(0, Random.Range(0, 360), 0)
            );

            t.transform.parent = targetTerrain.transform;
        }
    }
}