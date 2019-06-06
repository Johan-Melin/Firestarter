using UnityEngine;
using System.Collections;

public class SpawnGoblins : MonoBehaviour {

    public GameObject goblin;
    private int spawnTimer = 0;
	private float distance = 25f;
	private int spawnDelay = 300;
	
	void Update () {
        if(++spawnTimer == spawnDelay) {
            spawnTimer = 0;
            Instantiate(goblin, RandomCircle(), Quaternion.identity);
        }
	}

    Vector3 RandomVector() {
        Vector3 pos = new Vector3(RandomFloat(), 0.2f, RandomFloat());
        return pos;
    }

    float RandomFloat() {
        if (Random.Range(0, 2) == 0)
            return (Random.Range(-10f, -7f));
        return (Random.Range(7f, 10f));
    }
	
	Vector3 RandomCircle(){
         float ang = Random.value * 360;
         Vector3 pos = Vector3.zero;
		 pos.y = 0f;
         pos.x = distance * Mathf.Sin(ang * Mathf.Deg2Rad);
         pos.z = distance * Mathf.Cos(ang * Mathf.Deg2Rad);
         return pos;
     }
}
