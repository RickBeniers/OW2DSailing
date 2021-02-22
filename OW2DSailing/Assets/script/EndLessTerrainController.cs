using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLessTerrainController : MonoBehaviour
{
    public int chunkSize;
    public int chunkVissibleInViewDst;

    public const float maxViewDst = 450;

    public Transform viewer;

    public static Vector2 viewerPosition;

    Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
    List<TerrainChunk> terrainChunksVissibleLastUpdate = new List<TerrainChunk>();

    public void Start()
    {
        chunkSize = MapGenerator.mapChunkSize - 1;
        chunkVissibleInViewDst = Mathf.RoundToInt(maxViewDst / chunkSize);
    }
    public void Update()
    {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z);
        UpdateVissibleChunks();
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
                    if (terrainChunkDictionary[viewedChunkCoord].IsVissible()) 
                    { 
                        terrainChunksVissibleLastUpdate.Add(terrainChunkDictionary[viewedChunkCoord]);
                    }
                }
                else 
                {
                    terrainChunkDictionary.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, chunkSize, transform));
                }
            }
        }
    }
    public class TerrainChunk
    {
        GameObject meshObject;
        Vector2 position;
        Bounds bounds;

        public TerrainChunk(Vector2 coord, int size, Transform parent) 
        {
            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);
            Vector3 positionV3 = new Vector3(position.x, 0, position.y);

            meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            meshObject.transform.position = positionV3;
            meshObject.transform.localScale = Vector3.one * size / 10f;
            meshObject.transform.parent = parent;
            SetVissible(false);
        }
        public void UpdateTerrainChunk() 
        {
            float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
            bool vissible = viewerDstFromNearestEdge <= maxViewDst;
            SetVissible(vissible);
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
}
