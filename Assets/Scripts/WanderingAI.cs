﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingAI : MonoBehaviour
{
public const float baseSpeed = 1f;

	public float speed;
	
	private GameObject playerObject;

    public bool running;

    public float visibility;

    public float maxDistance;
    private float obstacleRange = 3.5f;
	
	private bool _alive;
	private int _animState;
	private float _multiplier;


    bool waiting;

    bool chasing;

    float waitFor;

	void Start() {
		_alive = true;
		
		//anim = GetComponent<Animator>();
		playerObject = GameObject.Find("Player");
        waitFor = 0;
        waiting = false;
        chasing = true;

		Vector3 diff = playerObject.transform.position - this.transform.position;
		float range = diff.magnitude;
		
		if (range > maxDistance) {
			//_animState = 0;
			_multiplier = 0.0f; //0 = dont move
            //just wander in any direction (continue if already on a path)
		}
		else if (range <= visibility) { 
			//_animState = 3;
			_multiplier = 8.0f;
            //go towards player

		}

	}
	
	void Update() {
		if (_alive && chasing) {

			//if very far away zombie speed = 0: idle animation
			//if somewhat close zombie speed = walking speed: walking animation
			//if near zombie speed = 8.0*walking speed:running animation

			Vector3 diff = playerObject.transform.position - transform.position;
			float range = diff.magnitude;
			if (range > maxDistance && !waiting) { //very far away and we want to redirect toward player
				//anim.SetInteger("zombieToState", 0);
                transform.rotation = Quaternion.LookRotation( Vector3.forward, diff);
				_multiplier = 4.0f;
			}
			else if (range <= visibility && !waiting) {  //"sees player, and moves toward him
                transform.rotation = Quaternion.LookRotation( Vector3.forward, diff);
                if(running){
                    transform.Rotate(new Vector3(0,0,180));
                }
				_multiplier = 7.0f;
			}
            else {
				//normal distance away, let wander

				_multiplier = 4.0f; //dont move if not close to player
			}

            transform.Translate(0, 0.5f * Time.deltaTime * _multiplier * speed, 0);
            RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up*0.57f , transform.up,10f);
            Debug.DrawRay(transform.position + transform.up*0.57f , transform.up * 10f, Color.red);
            // If it hits something...
            
            if (hit.collider != null)
            {
                GameObject hitObject = hit.transform.gameObject;
			 	if (hitObject.GetComponent<PlayerCharacter>()) {
			 		Debug.Log("saw charcter");
                     if(!running){
                        if(hit.distance < 0.05f){
                            hitObject.GetComponent<PlayerMovement>().kill();
                            Debug.Log("caught charcter");
                            chasing = false;
                        }
                     }
			 	}
                else{
                     Debug.Log("saw else"); //about to crash into planet, adjust path and move in that dir for some time
                     if(hit.distance < 0.2){
                         GetComponent<ReactiveTarget>().ReactToHit();
                     }
                     else if(hit.distance < obstacleRange){
                          if(goLeft(hit)){
                              transform.Rotate(0,0,10);
                          }
                          else{
                             transform.Rotate(0,0,-10);
                         }
                        waitFor = 500f* Time.deltaTime;
                        waiting = true;
                        //extraTurn = true;
                     }
                 }
            }
            else{
                if(waitFor > 0){
                    // if(extraTurn){
                    //     transform.Rotate(0,0,10);
                    //     extraTurn = false;
                    // }
                    waitFor -= 1f*Time.deltaTime;
                    }
                    else if(waiting){

                        waiting = false;
                }

            }
        }
	}

    public bool goLeft(RaycastHit2D hit){

        var rotateDegree = 10f; 
        bool left = true;
        while(true){
            transform.Rotate(0,0,rotateDegree);
            RaycastHit2D testHit = Physics2D.Raycast(transform.position + transform.up*0.57f , transform.up,10f);
            if (testHit.collider == null){ 
                return left;
            }
            else{
                rotateDegree = rotateDegree*(-1.1f);
                left = !left;
            }
        }

        // Vector3 normal = ((new Vector3(hit.point.x,hit.point.y,0)) - transform.position).normalized;
        // Vector3 pos = Vector3.Project(hit.transform.position, normal);


        // Vector3 vA = transform.position;
        // Vector3 vB = (new Vector3(hit.point.x,hit.point.y,0));
        // Vector3 p = hit.transform.position;
        // Vector3 v1 = p - vA;
        // Vector3 v2 = (vB-vA).normalized;
        // var d = Vector3.Distance(vA, vB);
        // var t = Vector3.Dot(v2, v1);

        // Vector3 closest;
    
        // if (t <= 0){
        //     closest = vA;
        // }
        // else if (t >= d){
        //     closest =  vB;
        // }
        // else{
        //     Vector3 v3 = v2 * t;
        //     closest = vA + v3;
        // }

        // if(!printed){
        //     Debug.Log("P" + closest);
        //     printed = true;
        // }

        // if(closest.x < p.x && closest.y < p.y){
        //     return true;
        // }
        // else if(closest.x > p.x && closest.y < p.y){ //works
        //     return(transform.position.x > p.x && transform.position.y > p.y);
        //     //return false;
        // }
        // else if(closest.x < p.x && closest.y > p.y){
        //     return false;
        // }
        // else{
        //     return false; //works?
        // }

    }

	public void SetAlive(bool alive) {
		_alive = alive;
	}

	private void OnSpeedChanged(float value) {
		speed = baseSpeed * value;
	}
}