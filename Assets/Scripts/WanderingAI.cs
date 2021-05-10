using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingAI : MonoBehaviour {

	[SerializeField] private Thruster _leftThrust;
	[SerializeField] private Thruster _rightThrust;
    [SerializeField] private Siren siren;

    [SerializeField] private GameObject lazer;

     [SerializeField] public AudioClip lazerShot;

     [SerializeField] public bool isPirate;
	
	public const float baseSpeed = 1f;

	public float speed;
	
	private GameObject playerObject;

    public bool running;

    public float visibility;

    public float maxDistance;

    public float closeToPlayer;

    public float tooClose;
    private float obstacleRange = 3.5f;
	
	public bool _alive;

    public float timeBetweenshot;
	private int _animState;
	private float _multiplier;

    public int health;

    bool waiting;

    bool playerCaught = false;

    bool chasing;

    float waitFor;

    float reloadTime;

    float shouldTurn = 0;

    private bool canLoot = false;
    float beingLooted = 0;

    private ScoreController _scoreController;

    public int getHealth(){
        return health;
    }

    public void looseHealth(int h){
        health = health - h;
    }

	void Start() {
		_alive = true;
		_scoreController = GameObject.Find("Score Controller").GetComponent<ScoreController>();
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
			_multiplier = 7.0f;
            //go towards player
		}

	}
	
	void Update() {
		if (_alive && chasing) {

			//if very far away zombie speed = 0: idle animation
			//if somewhat close zombie speed = walking speed: walking animation
			//if near zombie speed = 8.0*walking speed:running animation
             Quaternion initial = playerObject.transform.rotation;

			Vector3 diff = playerObject.transform.position - transform.position;
			float range = diff.magnitude;
			if (range > maxDistance && !waiting) { //very far away and we want to redirect toward player
				//anim.SetInteger("zombieToState", 0);
                transform.rotation = Quaternion.LookRotation( Vector3.forward, diff);
				_multiplier = 0.1f;
                //playerCaught = false;
			}
            else if (range > closeToPlayer && range <= maxDistance && !waiting){ //off screen so can just stay still
                _multiplier = 0f;
                playerCaught = false;
                if (siren != null) siren.TurnOff();
                
            }
            else if(range <= closeToPlayer && !waiting){ //on screen so have wander
                _multiplier = 4f; 
                if(range <= visibility && (playerObject.GetComponent<PlayerMovement>().isBoost() || playerCaught || playerObject.GetComponent<PlayerMovement>().isLooting() || isPirate )){
                    if(!playerCaught) playerCaught = true;
                    
                    transform.rotation = Quaternion.LookRotation( Vector3.forward, diff);
                    if(running){
                        transform.Rotate(new Vector3(0,0,180)); 
                    }

				    if(range <= tooClose) {
                        _multiplier = 0.0f;
                    }
                    else{
                        _multiplier = 7.0f;
                    }
                }
                else{
                    playerCaught = false;
                     if (siren != null) siren.TurnOff();
                    if(shouldTurn >= 4){
                        shouldTurn = 0;
                        //float angle = Random.Range(0, 360);
                        //Debug.Log("randomTurn");
                        transform.Rotate(new Vector3(0,0,Random.Range(0, 360)));
                    }
                    else{
                        shouldTurn += Time.deltaTime;
                    }
                }
                
            }
            else {
				//normal distance away, let wander
                if(range <= visibility && !running && !isPirate && (playerObject.GetComponent<PlayerMovement>().isBoost() || playerCaught || playerObject.GetComponent<PlayerMovement>().isLooting() )){
                    if (siren != null) siren.TurnOn();
                    playerCaught = true;
                }
				_multiplier = 4.0f; //dont move if not close to player
			}

            //cargoship is within range and player has not yet looted it
            if (running && !playerObject.GetComponent<PlayerMovement>().isLooting()) {
	            if (!canLoot && range <= tooClose) {
		            canLoot = true;
		            gameObject.GetComponent<CargoShip>().CanLoot();
	            } else if (canLoot && range > tooClose) {
		            canLoot = false;
		            gameObject.GetComponent<CargoShip>().ClearLootState();
	            }
            }

            //cargoship was being looted, but player stopped, so reset progress
            if (canLoot && !(playerObject.GetComponent<PlayerMovement>().isLooting()) && beingLooted > 0){
                Debug.Log("Resetloot timer");
                beingLooted = 0;
                gameObject.GetComponent<CargoShip>().StartLooting();
            }

            //Looting cargo ship
            if(running && range <= tooClose && playerObject.GetComponent<PlayerMovement>().isLooting()){
                //playerObject.transform.rotation = initial;
                Debug.Log("Cargoship being looted");
                playerCaught = true;
                _multiplier = 0.0f;

                if (beingLooted < Time.deltaTime) {
	                GetComponent<CargoShip>().StartLooting();
                }
                
                if (beingLooted > 2) {
                    GetComponent<ReactiveTarget>().ReactToHit();
                    _scoreController.AddScore(1000);
                    RunStats.Current.CargoShipsLooted++;
                }
                else{
                    beingLooted += Time.deltaTime;
                    GetComponent<CargoShip>().SetLootProgress(beingLooted / 2f);
                }
                //add different particle effect?
            }
			
			_leftThrust.SetIntensity(_multiplier / 7f);
			_rightThrust.SetIntensity(_multiplier / 7f);

            transform.Translate(0, 0.5f * Time.deltaTime * _multiplier * speed, 0);
            
            //Debug.DrawRay(transform.position + transform.up*0.57f , transform.up * 10f, Color.red);
            // If it hits something...
            int grnd = 1 << LayerMask.NameToLayer("Default");
            int fly = 1 << LayerMask.NameToLayer("Player");
            int mask = grnd | fly;

            RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up*0.57f , transform.up,8f,mask);


            if (hit.collider != null)
            {
                GameObject hitObject = hit.transform.gameObject;
			 	if (hitObject.GetComponent<PlayerMovement>()) {
                     if(!running && hitObject.GetComponent<PlayerMovement>().alive && playerCaught){
                         if (siren != null && !isPirate) siren.TurnOn();
                        // if(hit.distance < 0.05f){
                        //     hitObject.GetComponent<PlayerHealth>().Die();
                        //     Debug.Log("caught charcter");
                        //     chasing = false;
                        // }
                        //else{ //shoot at player
                            if(reloadTime >= timeBetweenshot){
                                reloadTime = 0;
                                AudioSource.PlayClipAtPoint (lazerShot, Camera.main.transform.position);
                                Instantiate(lazer, 
                                new Vector3(transform.position.x,transform.position.y, 0), 
                                transform.rotation);
                            }
                            else{
                                reloadTime += Time.deltaTime * 50;
                            }
                        //}
                     }
			 	}
                else if(hitObject.GetComponent<EnemyLazer>() || hitObject.GetComponent<RepairStation>() ||hitObject.GetComponent<FuelStation>()){
                    Debug.Log("saw collier that should be ignored");
                }
                else
                {
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
                        waitFor = 200f* Time.deltaTime;
                        waiting = true;
                        //extraTurn = true;
                     }
                 }
            }
            else{
	            _leftThrust.SetIntensity(0f);
	            _rightThrust.SetIntensity(0f);
	            
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
