using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {
    public GameObject manager;
    public Vector2 location = Vector2.zero;
    public Vector2 velocity;
    Vector2 goalPos = Vector2.zero;
    Vector2 currentForce;

    // Use this for initialization
    void Start() {
        velocity = new Vector2(Random.Range(0.01f, 0.1f), Random.Range(0.01f, 0.1f));
        location = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);

    }

    // vector towards the goal location
    Vector2 seek(Vector2 target) {
        return (target - location);
    }

    void applyForce(Vector2 f) {
        Vector3 force = new Vector3(f.x, f.y, 0);
        if(force.magnitude > manager.GetComponent<BoidsManager>().maxForce) {
            force = force.normalized;
            force *= manager.GetComponent<BoidsManager>().maxForce;
        }

        this.GetComponent<Rigidbody2D>().AddForce(force);

        if(this.GetComponent<Rigidbody2D>().velocity.magnitude > manager.GetComponent<BoidsManager>().maxVelocity) {
            this.GetComponent<Rigidbody2D>().velocity = this.GetComponent<Rigidbody2D>().velocity.normalized;
            this.GetComponent<Rigidbody2D>().velocity *= manager.GetComponent<BoidsManager>().maxVelocity;
        }
        Debug.DrawRay(this.transform.position, force, Color.white);
    }


    /// <summary>
    /// flock() : apply flocking rules
    /// Obedient boolean if for applying flocking rules or not
    /// Repulsive boolean is for applying a repulsive force to boids
    /// The random is used to update flocking rules less than one by frame
    /// </summary>
    void flock() {
        location = this.transform.position;
        velocity = this.GetComponent<Rigidbody2D>().velocity;

        if(manager.GetComponent<BoidsManager>().obedient) {
            Vector2 align = alignement();
            Vector2 cohesio= cohesion();
            Vector2 repulsio = repulsion();
            Vector2 goalLocation;

            if (manager.GetComponent<BoidsManager>().seekGoal) {
                goalLocation = seek(goalPos);

                currentForce = manager.GetComponent<BoidsManager>().goalLoc * goalLocation.normalized + manager.GetComponent<BoidsManager>().align * align + manager.GetComponent<BoidsManager>().cohesion * cohesio + manager.GetComponent<BoidsManager>().repulsion * repulsio;
            } else {
                currentForce = manager.GetComponent<BoidsManager>().align * align + manager.GetComponent<BoidsManager>().cohesion * cohesio + manager.GetComponent<BoidsManager>().repulsion * repulsio; ;
            }
       
            currentForce = currentForce.normalized;
        }

        applyForce(currentForce/ manager.GetComponent<BoidsManager>().inertie);
    }

    Vector2 alignement() {
        float neighbourDistance = manager.GetComponent<BoidsManager>().neighbourDistance;
        Vector2 sum = Vector2.zero;
        int count = 0;

        foreach (GameObject boid in manager.GetComponent<BoidsManager>().boidsArray) {
            if (boid != this.gameObject) {
                float distance = Vector2.Distance(location, boid.GetComponent<Boid>().location);
                if (distance < neighbourDistance) {
                    sum += boid.GetComponent<Boid>().velocity;
                    count++;
                }
            }
            if (count > 0) {
                sum /= count;
                Vector2 steer = sum - velocity ;
                return steer;
            }
        }
        //Debug.Log(neighbourDistance);
        return Vector2.zero;
    }
    
    Vector2 cohesion() {
        float neighbourDistance = manager.GetComponent<BoidsManager>().neighbourDistance;
        Vector2 sum = Vector2.zero;
        int count = 0;

        foreach (GameObject boid in manager.GetComponent<BoidsManager>().boidsArray) {
            if (boid != this.gameObject) {
                float distance = Vector2.Distance(location, boid.GetComponent<Boid>().location);
                if (distance < neighbourDistance) {
                    sum += boid.GetComponent<Boid>().location;
                    count++;
                }
            }
            if (count > 0) {
                sum /= count;
                return seek(sum);
            }
        }
        return Vector2.zero;
    }

    Vector2 repulsion() {
        float neighbourDistance = manager.GetComponent<BoidsManager>().neighbourDistance;
        float mindist = 10000000000000000;
        GameObject boidMin = null;
        foreach (GameObject boid in manager.GetComponent<BoidsManager>().boidsArray) {
            if(boid != this.gameObject) {
                float distance = Vector2.Distance(location , boid.GetComponent<Boid>().location);
                if(distance < mindist && distance < neighbourDistance) {
                    mindist = distance;
                    boidMin = boid;        
                }
            }
        }
        if(boidMin) {
            Vector2 dir = gameObject.transform.position - boidMin.gameObject.transform.position;
            return -dir/mindist;
        }
        else {
            return Vector2.zero;
        }      
    }


    // Update is called once per frame
    void Update() {
        flock();
        goalPos = manager.transform.position;

    }

}
