using UnityEngine;

public class TreeRandomSpawner : MonoBehaviour
{
    [Header("基础设置")]
    public Terrain targetTerrain;
    public GameObject treePrefab;
    public int treeCount = 3000;

    [Header("随机范围")]
    public float minScale = 0.8f;
    public float maxScale = 2.2f;

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
            // 随机 XZ
            float x = Random.Range(terrainPos.x, terrainPos.x + terrainSize.x);
            float z = Random.Range(terrainPos.z, terrainPos.z + terrainSize.z);

            // 地形高度
            float y = targetTerrain.SampleHeight(new Vector3(x, 0, z));

            // 实例化
            GameObject t = Instantiate(
                treePrefab,
                new Vector3(x, y, z),
                Quaternion.Euler(0, Random.Range(0, 360), 0)
            );

            //// 随机大小
            //float scale = Random.Range(minScale, maxScale);
            //t.transform.localScale = Vector3.one * scale;

            // 父物体挂到地形下（可选）
            t.transform.parent = targetTerrain.transform;
        }
    }
}