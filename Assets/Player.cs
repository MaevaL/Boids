using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    float horizontalMove = 0;
    float verticalMove = 0;
    public float speed = 20f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    void Move() {
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        transform.Translate(horizontalMove * speed * Time.deltaTime , verticalMove * speed * Time.deltaTime , 0);
    }
}
