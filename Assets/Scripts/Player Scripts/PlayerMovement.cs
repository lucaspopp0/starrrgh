using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
public class PlayerMovement : MonoBehaviour {

    private Hud _hud;

    [SerializeField] private ScoreController _scoreController;
    [SerializeField] private Thruster _leftThruster;
    [SerializeField] private Thruster _mainThruster;
    [SerializeField] private Thruster _rightThruster;
    [SerializeField] private AudioSource _powerupSound;

    private Vector2 _lastUsableVelocity;
    
    /*
     * Planet variables
     */
    //A list of planets to calculate gravity from
    [SerializeField] private GameObject[] planets;
    //Newton's gravitational constant, just set to 1 for scale
    [SerializeField] private float G = 1;
    //The closest approach the ship can take where we still calculate the gravitational force from a planet
    //This is to decrease large forces or divide by zero errors from occuring
    [SerializeField] private float closestApproach = 0.1f;

    /*
     * Movement variables
     */
    //The rotational speed of the ship
    [SerializeField] private float rotationSpeed = 100.0f;
    //Strength of the propulsion force
    [SerializeField] private float propForce = 50.0f;
    //Strength of the stopping force
    [SerializeField] private float stoppingForce = 10.0f;
    //Drag strength
    [SerializeField] private float dragForce = 0.1f;

    [SerializeField] private float boostSpeed = 20f;

    [SerializeField] private float boostTime = 0.5f;

    //Maximum speed the ship can have
    [SerializeField] private float maxVelocity = 10.0f;
    private Vector2 velocity;

    private float speedup_duration = 5f;
    private float propulsionCoeff = 1f;
    private float speedUpTimer = 0f;

    private bool boost;

    private float boostRecharge;

    private float curBoostTime;

    public bool alive = true;
    private bool _disabled = false;

    private void Awake() {
        _hud = GameObject.FindWithTag("HUD").GetComponent<Hud>();
    }

    // Start is called before the first frame update
    void Start()
    {
        velocity = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if(alive){
            if (Input.GetKeyDown(KeyCode.Escape)) {
                if (GameState.shared.paused) {
                    _hud.Unpause
                        
                        ();
                } else {
                    _hud.Pause();
                }
            }

            if(Input.GetKeyDown(KeyCode.Space) && !boost){ //boost start
                boost = true; 
                curBoostTime = 0;
            }
            
            if(boost){
                curBoostTime += Time.deltaTime; //boost end
                if(curBoostTime > boostTime){ 
                    boost = false;
                    velocity = this.transform.up * 8;

                }
                else{
                    this.transform.position += this.transform.up * Time.deltaTime * boostSpeed;
                    _leftThruster.SetIntensity(1);
                    _rightThruster.SetIntensity(1);

                    LayerMask mask = LayerMask.GetMask("Default");
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up,10f,mask);

                    if (hit.collider != null)
                    {
                        GameObject hitObject = hit.transform.gameObject;
                        if (hitObject.GetComponent<WanderingAI>() && hit.distance < 0.05f) {
                            if(hitObject.GetComponent<WanderingAI>()._alive){
                                if(hitObject.GetComponent<WanderingAI>().running){
                                    _scoreController.AddScore(3);
                                }
                                else{
                                    _scoreController.AddScore(5);
                                }
                                hitObject.GetComponent<ReactiveTarget>().ReactToHit();
                                
                            }
                        }
                    }
                }

            }
            else{
                //Rotating the ship
                float rotation = -Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
                this.transform.Rotate(0, 0, rotation);

                var thrusterInput = Input.GetAxis("Vertical");
                if (Mathf.Abs(thrusterInput) <= 0.01f) {
                    thrusterInput = Input.GetAxis("Horizontal");
                }

                var leftProportion = Mathf.Min(2f - (-Input.GetAxis("Horizontal") + 1f), 1f);
                var rightProportion = Mathf.Min(2f - (Input.GetAxis("Horizontal") + 1f), 1f);

            //The propulsion force, in the direction the ship is pointed
            Vector2 propulsion = Vector2.zero;
            if (!isDisabled())
            {
                propulsion = transform.up * (propForce * Input.GetAxis("Vertical") * propulsionCoeff);
                _leftThruster.SetIntensity(thrusterInput * leftProportion);
                _mainThruster.SetIntensity(thrusterInput);
                _rightThruster.SetIntensity(thrusterInput * rightProportion);
            }
           

                Vector2 totalForce = Vector2.zero;

            //If the player presses shift
            if (Input.GetKey(KeyCode.LeftShift) && !isDisabled())
            {
                //Don't calculate forces from gravity or propulsion
                //Apply a force opposite to the velocity to stop the ship

                    //Should we add gravity to this? Or is that too much
                    totalForce = -(velocity.normalized) * stoppingForce;
                }
                //Otherwise, calculate gravity and propulsion like normal
                else
                {
                    Vector2 drag = 0.5f * velocity.magnitude * velocity.magnitude * dragForce * velocity.normalized;
                    totalForce = gravity(planets) + propulsion - drag;
                }

                //Applying the force to the ship
                applyForce(totalForce);
            }
        }

        if (propulsionCoeff > 1)
        {
            speedUpTimer -= Time.deltaTime;
        }

        if (speedUpTimer <= 0)
        {
            propulsionCoeff = 1;
        }
    }

    /*
     * Returns the acceleration due to gravity from a set of celestial bodies
     * (Note: This does NOT return the force, just the acceleration)
     */
    private Vector2 gravity(GameObject[] bodies)
    {
        //Called a force here for convenience, really an acceleration
        Vector2 totalForce = Vector2.zero;
        //Calculate and sum each force from each planet
        foreach (GameObject planet in bodies)
        {
            Planet p = planet.GetComponent<Planet>();
            if (p != null)
            {
                Vector2 radius = planet.transform.position - this.transform.position;
                double magRadius = Math.Sqrt(Vector2.SqrMagnitude(radius));

                //If we are sufficiently far away from the center of the planet, calculate the force
                if (magRadius >= closestApproach)
                {
                    Vector2 force = (float)(G * p.getMass() / Math.Pow(magRadius, 3)) * radius;
                    totalForce += force;
                }//else, do nothing (this keeps divide by 0 errors from happening)
            }
        }
        return totalForce;
    }

    /*
     * Applies the given force to the ship and updates the velocity and position
     */
    private void applyForce(Vector2 force)
    {
        velocity += force * Time.deltaTime;
        //Converting velocity to a Vector3
        float mag = velocity.magnitude;
        //usableVel = usableVel.normalized * Mathf.Clamp(mag, 0, maxVelocity);
        velocity = velocity.normalized * Mathf.Clamp(mag, 0, maxVelocity);
        Vector3 usableVel = velocity;
        _lastUsableVelocity = usableVel;
        this.transform.position += (usableVel * Time.deltaTime);
    }

    public void kill(){
        alive = false;
        _lastUsableVelocity = Vector2.zero;
    }

    public Vector2 GetVelocity() {
        return _lastUsableVelocity;
    }

   public void setDisabled(bool disabled)
    {
        _disabled = disabled;
    }

   public bool isDisabled()
   {
       return _disabled;
   }

   public void speedUp(float coeff,float duration)
   {
       propulsionCoeff = coeff;
       speedup_duration = duration;
       startSpeedUpTimer();
   }

   void startSpeedUpTimer()
   {
       speedUpTimer = speedup_duration;
   }

   public void ObtainPowerup() {
       _powerupSound.Play();
   }
}
