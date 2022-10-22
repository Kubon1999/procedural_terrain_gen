using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateChunksAround : MonoBehaviour
{

    public GameObject chunkOfTerrain;
    int chunkSize = 255;
    int radius = 700;
    List<GameObject> chunks = new List<GameObject>();
    Vector3 lastPos = Vector3.zero;
    int chunksNumberVisibleInViewDistance = 0;
    // Start is called before the first frame update
    void Start()
    {
            chunksNumberVisibleInViewDistance = Mathf.RoundToInt(radius / chunkSize);
    }

    // Update is called once per frame
    void Update()
    {
    
        Vector3 mypos = transform.position;
        if (mypos != lastPos)
        {
            lastPos = mypos;
            CreateChunks(mypos);
            CleanChunks(mypos);
        }
    }

    void CreateChunks(Vector3 currentPosition){
            int currentChunkX = Mathf.RoundToInt(currentPosition.x / chunkSize);
            int currentChunkZ = Mathf.RoundToInt(currentPosition.z / chunkSize);
            //create chunk and add to the list
            Debug.Log("Creating chunk");
            for(int i = -chunksNumberVisibleInViewDistance; i <= chunksNumberVisibleInViewDistance; i ++){
                for(int j = -chunksNumberVisibleInViewDistance; j <= chunksNumberVisibleInViewDistance; j ++){
                    if(chunks.Count < 30){
                        float chunkX = (currentChunkX + i) * chunkSize;
                        float chunkZ = (currentChunkZ + j) * chunkSize;
                        if(canSpawnChunk(new Vector2(chunkX, chunkZ))){
                            GameObject newChunk = Instantiate(chunkOfTerrain, new Vector3(chunkX, 0, chunkZ), Quaternion.identity);
                            chunks.Add(newChunk);
                        }
                    }
                }
            }
    }

      void CleanChunks(Vector3 currentPosition){
        //clean chunks that are too far away
        for(int i = 0; i < chunks.Count; i ++){
            float distance = Vector3.Distance(chunks[i].transform.position, currentPosition);
            if(distance > radius){
                Debug.Log("Destroying chunk");
                Destroy(chunks[i]);
                chunks.RemoveAt(i);
            }
        }
    }

    //draws a sphere around the player and creates chunks of terrain within the sphere
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public bool canSpawnChunk(Vector2 randomPosition){
            Vector3 rayStart = new Vector3(randomPosition.x, 9999f, randomPosition.y);

            if(!Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, Mathf.Infinity)){
                 //pos ok!
                return true;
            }

            return false;
    }
}
