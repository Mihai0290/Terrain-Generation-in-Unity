using UnityEngine;

public class GenerateTerrain : MonoBehaviour
{
    public GameObject chunkPrefab;
    public int terrainSize = 4;
    public int terrainUnitSize = 5;
    public int octaves = 3;
    public float lacunarity = 2f;
    public float persistence = .5f;
    public float scale = 1f;
    public int seed = 24234;
    int verticesScale = 1; //not working anymore
    public AnimationCurve meshHeightCurve;
    public float heightMultiplier = 10f;
    float[,] noiseMap;

    Vector3[] vertices;
    int[] triangles;

    void Awake()
    {
        BuildTerrain();
    }
    void BuildTerrain()
    {
        noiseMap = TerrainNoise.GenerateTerrainNoise(terrainSize * (terrainUnitSize + 1), terrainSize * (terrainUnitSize + 1), octaves, lacunarity, persistence, scale, seed);
        for(int i = 0; i < terrainSize; i++)
        {
            for(int j = 0; j < terrainSize; j++)
            {
                Vector3 chunkPos = new Vector3(i * terrainUnitSize, 0f, j * terrainUnitSize);
                GameObject chunk = Instantiate(chunkPrefab, chunkPos, Quaternion.identity);
                chunk.GetComponent<MeshFilter>().mesh = GenerateMesh(j , i);
                chunk.AddComponent<MeshCollider>();
            }
        }
    }
    void Update()
    {

    }
    Mesh GenerateMesh(int x, int y) // for non-chunk 0, 0
    {
        Mesh mesh = new Mesh();
        vertices = new Vector3[(terrainUnitSize + 1) * (terrainUnitSize + 1)];
        int verticesIndex = 0;
        for(int i = 0; i < verticesScale * (terrainUnitSize + 1); i += verticesScale)
        {
            for (int j = 0; j < verticesScale * (terrainUnitSize + 1); j += verticesScale)
            {
                float height = heightMultiplier * meshHeightCurve.Evaluate(noiseMap[i + terrainUnitSize * x, terrainUnitSize * y + j]);
                vertices[verticesIndex] = new Vector3(j, height, i);
                Debug.Log(noiseMap[j, i]);
                verticesIndex++;
            }
        }

        triangles = new int[terrainUnitSize * terrainUnitSize * 6];
        int trianglesIndex = 0;
        int vert = 0;
        for(int i = 0; i < terrainUnitSize; i++)
        {
            for (int j = 0; j < terrainUnitSize; j++)
            {
                triangles[trianglesIndex] = vert;
                triangles[trianglesIndex + 1] = vert + terrainUnitSize + 1;
                triangles[trianglesIndex + 2] = vert + 1;
                triangles[trianglesIndex + 3] = vert + 1;
                triangles[trianglesIndex + 4] = vert + terrainUnitSize + 1;
                triangles[trianglesIndex + 5] = vert + terrainUnitSize + 2;
                trianglesIndex += 6;
                vert++;
                
            }
            vert++; 
        }
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        return mesh;
    }

    void UpdateMesh(ref Mesh mesh)
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
