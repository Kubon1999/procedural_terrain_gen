using System.Collections;

using System.Collections.Generic;

using UnityEngine;

public class Spawner : MonoBehaviour

{

    [SerializeField] public GameObject prefab;

    [SerializeField] public int amount;

    [SerializeField] public Vector2 xSpawnRange;
    [SerializeField] public Vector2 zSpawnRange;

    [SerializeField] public float minHeight;
    [SerializeField] public float maxHeight;

    public Vector3 lastRay = Vector3.zero;

     Stack<GameObject> objectStack = new Stack<GameObject>();

    void Update(){
        if(lastRay != Vector3.zero){

        Debug.DrawRay(Vector3.zero, Vector3.down * 100, Color.red);
        }
    }


    public void Generate()
    {
        RemoveAll();
        for(int i = 0; i < amount; i++){
            Vector2 randomPosition = new Vector2(Random.Range(xSpawnRange.x, xSpawnRange.y), Random.Range(zSpawnRange.x, zSpawnRange.y));
            if(canSpawn(randomPosition)){
                float spawnHeight = getSpawnHeight(randomPosition);
                if(spawnHeight != 0){
                spawn(new Vector3(randomPosition.x, spawnHeight, randomPosition.y));
                }
            }
        }

    }

    public float getSpawnHeight(Vector2 position){
        Vector3 rayStart = new Vector3(position.x, maxHeight, position.y);
        if(Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, Mathf.Infinity)){
            return hit.point.y;
        }
        return 0;
    }

    public bool canSpawn(Vector2 randomPosition){
        Debug.Log(randomPosition);
            Vector3 rayStart = new Vector3(randomPosition.x, maxHeight, randomPosition.y);

            if(!Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, Mathf.Infinity)){
                lastRay = rayStart;
                return false;
            }

            if(hit.point.y < minHeight){
                return false;
            }

            //pos ok!
            return true;
    }

    

    public void spawn(Vector3 pos){
        GameObject newObject = Instantiate(prefab, pos, Quaternion.identity);
        newObject.transform.parent = gameObject.transform;
        objectStack.Push(newObject);
    }

    public void RemoveAll(){
            while(objectStack.Count > 0)
             {
                 GameObject temp = objectStack.Pop();
                 DestroyImmediate(temp);
             }


    }

}