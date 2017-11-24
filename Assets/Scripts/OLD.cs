using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OLD : MonoBehaviour {
    public GameObject boidPrefab;
    public GameObject[] boidsArray;
    public int nbBoids;
    public int borders;

	// Use this for initialization
	void Start () {
        boidsArray = new GameObject[nbBoids];
        initialisePositions();
    }
	
	// Update is called once per frame
	void Update () {
        moveAllBoidsToNewPositions();

    }

    private void initialisePositions() {
        
        for (int i = 0; i < nbBoids; i++) {
            Vector2 position = new Vector2(Random.Range(-borders, borders), Random.Range(-borders, borders));
            boidsArray[i] = Instantiate(boidPrefab, position, Quaternion.identity);
        }
    }

    private void moveAllBoidsToNewPositions() {
        Vector2 v1, v2, v3;
        
        foreach(GameObject b in boidsArray) {
            v1 = perceivedCenter(b);
            v2 = rule2(b);
            v3 = rule3(b);

            b.GetComponent<Rigidbody2D>().velocity += v1 + v2 + v3;
            b.GetComponent<Rigidbody2D>().velocity = b.GetComponent<Rigidbody2D>().velocity.normalized;

            Debug.Log(v2);
            b.transform.position = new Vector3((b.transform.position.x + b.GetComponent<Rigidbody2D>().velocity.x) , (b.transform.position.y + b.GetComponent<Rigidbody2D>().velocity.y));            
        }
    }


    Vector2 perceivedCenter(GameObject bj) {
        Vector2 pcj = new Vector2(0,0);
 
        foreach (GameObject boid in boidsArray) { 
            if (boid != bj) {
                pcj = new Vector2((pcj.x + boid.transform.position.x), (pcj.y + boid.transform.position.y));
            }
        }
        pcj /= (nbBoids - 1);
        return new Vector2(pcj.x - bj.transform.position.x, pcj.y - bj.transform.position.y) / 100;
    }

    Vector2 rule2(GameObject bj) {
        Vector3 near = new Vector2(0,0);
        foreach(GameObject boid in boidsArray) {
            if(boid != bj) {
                if(Mathf.Abs(Vector3.Distance(boid.transform.position , bj.transform.position)) < 2) {
                    near = near - (boid.transform.position - bj.transform.position);
                }
            }
        }

        return (Vector2)near;
    }

    Vector2 rule3(GameObject bj) {
        Vector2 pvj = new Vector2(0, 0);

        foreach(GameObject boid in boidsArray) {
            if(boid != bj) {
                pvj = pvj + boid.GetComponent<Rigidbody2D>().velocity;
            }
        }

        pvj /= (nbBoids - 1);

        return (pvj - bj.GetComponent<Rigidbody2D>().velocity) / 8;
    }
}
