using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLessTerrainController : MonoBehaviour
{
    public int chunkSize;
    public int chunkVissibleInViewDst;

    public static float maxViewDst;
    public LODInfo[] detailLevels;

    public Transform viewer;

    public Material mapMaterial;

    public static Vector2 viewerPosition;
    Vector2 viewerPositionOld;
    static MapGenerator mapGenerator;

    const float viewerMoveThresholdForChunkUpdate = 25f;
    const float sqrviewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;
    const float scale = 5f;

    Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
    static List<TerrainChunk> terrainChunksVissibleLastUpdate = new List<TerrainChunk>();

    public void Start()
    {
        mapGenerator = FindObjectOfType<MapGenerator>();

        maxViewDst = detailLevels[detailLevels.Length - 1].visibleDstThreshold;
        chunkSize = MapGenerator.mapChunkSize - 1;
        chunkVissibleInViewDst = Mathf.RoundToInt(maxViewDst / chunkSize);

        UpdateVissibleChunks();
    }
    public void Update()
    {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z) / scale;
        if ((viewerPositionOld - viewerPosition).sqrMagnitude > sqrviewerMoveThresholdForChunkUpdate) 
        {
            viewerPositionOld = viewerPosition;
            UpdateVissibleChunks();
        }
    }
    public void UpdateVissibleChunks()
    {
        for (int i =0; i < terrainChunksVissibleLastUpdate.Count; i++) 
        {
            terrainChunksVissibleLastUpdate[i].SetVissible(false);
        }
        terrainChunksVissibleLastUpdate.Clear();

        int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);

        for (int YOffSet = -chunkVissibleInViewDst; YOffSet <= chunkVissibleInViewDst; YOffSet++)
        {
            for (int XOffSet = -chunkVissibleInViewDst; XOffSet <= chunkVissibleInViewDst; XOffSet++)
            {
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + XOffSet, currentChunkCoordY + YOffSet);

                if (terrainChunkDictionary.ContainsKey(viewedChunkCoord)) 
                {
                    terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk();
                }
                else 
                {
                    terrainChunkDictionary.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, chunkSize, detailLevels, transform, mapMaterial));
                }
            }
        }
    }
    public class TerrainChunk
    {
        GameObject meshObject;
        Vector2 position;
        Bounds bounds;
        MeshRenderer meshRenderer;
        MeshFilter meshFilter;
        MapData mapData;
        bool mapDataRecieved;
        int previousLODindex = -1;

        LODInfo[] detailLevels;
        LODMesh[] lodMeshes;

        public TerrainChunk(Vector2 coord, int size, LODInfo[] detailLevels, Transform parent, Material material) 
        {
            this.detailLevels = detailLevels;
            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);
            Vector3 positionV3 = new Vector3(position.x, 0, position.y);

            meshObject = new GameObject("Terrain Chunk");
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshFilter = meshObject.AddComponent<MeshFilter>();

            meshRenderer.material = material;
            meshObject.transform.position = positionV3 * scale;
            meshObject.transform.parent = parent;
            meshObject.transform.localScale = Vector3.one * scale;
            SetVissible(false);

            lodMeshes = new LODMesh[detailLevels.Length];
            for (int i = 0; i < detailLevels.Length; i++) 
            {
                lodMeshes[i] = new LODMesh(detailLevels[i].lod, UpdateTerrainChunk);
            }

            mapGenerator.RequestMapData(position, OnMapDataRecieved);
        }
        void OnMapDataRecieved(MapData mapData) 
        {
            //mapGenerator.RequestMeshData(mapData, OnMeshDataRecieved);
            this.mapData = mapData;
            mapDataRecieved = true;

            Texture2D texture = TextureGenerator.TextureFromColourMap(mapData.colourMap, MapGenerator.mapChunkSize, MapGenerator.mapChunkSize);
            meshRenderer.material.mainTexture = texture;

            UpdateTerrainChunk();
        }
        
        public void UpdateTerrainChunk() 
        {
            if (mapDataRecieved)
            {
                float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
                bool vissible = viewerDstFromNearestEdge <= maxViewDst;
                if (vissible)
                {
                    int lodIdex = 0;
                    for (int i = 0; i < detailLevels.Length - 1; i++)
                    {
                        if (viewerDstFromNearestEdge > detailLevels[i].visibleDstThreshold)
                        {
                            lodIdex = i + 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (lodIdex != previousLODindex)
                    {
                        LODMesh lodMesh = lodMeshes[lodIdex];
                        if (lodMesh.hasMesh)
                        {
                            previousLODindex = lodIdex;
                            meshFilter.mesh = lodMesh.mesh;
                        }
                        else if (!lodMesh.hasRequestedMesh)
                        {
                            lodMesh.RequestMesh(mapData);
                        }
                    }
                    terrainChunksVissibleLastUpdate.Add(this);
                }
                SetVissible(vissible);
            }
        }
        public void SetVissible(bool visible) 
        {
            meshObject.SetActive(visible);
        }
        public bool IsVissible() 
        {
            return meshObject.activeSelf;
        }
    }
    class LODMesh 
    {
        public Mesh mesh;
        public bool hasRequestedMesh;
        public bool hasMesh;
        public int lod;
        System.Action updateCallback;

        public LODMesh(int lod, System.Action updateCallback) 
        {
            this.lod = lod;
            this.updateCallback = updateCallback;
        }
        void OnMeshDataRecieved(MeshData meshData) 
        {
            mesh = meshData.CreateMesh();
            hasMesh = true;
            updateCallback();
        }
        public void RequestMesh(MapData mapData) 
        {
            hasRequestedMesh = true;
            mapGenerator.RequestMeshData(mapData, lod,  OnMeshDataRecieved);
        }
    }
    [System.Serializable]
    public struct LODInfo 
    {
        public int lod;
        public float visibleDstThreshold;
    }
}
