using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsManager : MonoBehaviour {

    public GameObject[] boidsArray;
    public GameObject boidPrefab;
    public int nbBoids;
    public Vector3 range = new Vector3(5,5,5);

    public bool seekGoal = true; // if off no more center
    public bool obedient = true; // if off no following flocking rules anymore
    public bool repulsive = false; // if true they run from each other

    float horizontalMove = 0;
    float verticalMove = 0;
    public float speed = 20f;


    [Range(0, 200)]
    public int neighbourDistance = 20;
    [Range(0, 2)]
    public float maxForce = 0.5f;
    [Range(0, 2)]
    public float maxVelocity = 2.0f;

    // Use this for initialization
    void Start () {
        boidsArray = new GameObject[nbBoids];
        for(int i = 0; i < nbBoids; i++) {
            Vector3 boidPosition = new Vector3(Random.Range(-range.x, range.x), Random.Range(-range.y, range.y), 0);
            boidsArray[i] = Instantiate(boidPrefab, boidPosition, Quaternion.identity) as GameObject;
            boidsArray[i].GetComponent<Boid>().manager = this.gameObject;
        }
	}

    private void Update() {
        Move();
    }

    void Move() {
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        transform.Translate(horizontalMove * speed * Time.deltaTime , verticalMove * speed * Time.deltaTime , 0);
    }
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(this.transform.position, range * 2);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, 0.2f);
    }
}
