using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
public class PlayerMovement : MonoBehaviour
{

    private Hud _hud;
    [SerializeField] private Thruster _leftThruster;
    [SerializeField] private Thruster _mainThruster;
    [SerializeField] private Thruster _rightThruster;
    [SerializeField] private AudioSource _powerupSound;
    [SerializeField] private BoostEffect boostEffect;
    [SerializeField] private ParticleSystem speedEffect;
    [SerializeField] private SpriteRenderer dashIndicator;

    private Vector2 _lastUsableVelocity;

    /*
     * Planet variables
     */
    //A list of planets to calculate gravity from
    [SerializeField] private GameObject[] planetList;
    private HashSet<GameObject> planets = new HashSet<GameObject>();
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

    private float _minBoost = 0.05f;
    private float _maxBoost = 0.2f;
    private bool _chargingBoost;
    private bool boost;
    private float _boostAmount;
    private float _chargeRate = 0.5f;

    private float boostRecharge;

    private float curBoostTime;

    public bool alive = true;
    private bool _disabled = false;

    private bool _looting = false;

    private ScoreController _scoreController;

    public Vector2 getVelocity()
    {
        return velocity;
    }

    public bool isBoost(){
        return boost;
    }

    public bool isLooting(){
        return _looting;
    }

    private void Awake() {
        _hud = GameObject.FindWithTag("HUD").GetComponent<Hud>();
        RunStats.ResetCurrent();
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject p in planetList)
        {
            planets.Add(p);
        }
        velocity = Vector2.zero;
        _scoreController = GameObject.Find("Score Controller").GetComponent<ScoreController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                if (GameState.shared.paused) _hud.Unpause();
                else _hud.Pause();
            }
            
            if(isDisabled()){
                _leftThruster.SetIntensity(0);
                _rightThruster.SetIntensity(0);
            } else {
                RunStats.Current.Duration += Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.Space) && !boost && !isDisabled()) {
                // Start charging boost
                _chargingBoost = true;
                _boostAmount = _minBoost;
                dashIndicator.size = DashIndicatorSize();
            } else if (!isDisabled() && _chargingBoost && Input.GetKey(KeyCode.Space)) {
                // Continue charging boost
                _boostAmount += Time.deltaTime * _chargeRate;
                
                if (_boostAmount > _maxBoost) {
                    _boostAmount = _maxBoost;
                }

                dashIndicator.size = DashIndicatorSize();
            } else if (_chargingBoost && (Input.GetKeyUp(KeyCode.Space) || _boostAmount > _maxBoost)) {
                // End charging and actually boost
                _chargingBoost = false;
                dashIndicator.size = DashIndicatorSize();
                boostTime = _boostAmount;
                boost = true; 
                curBoostTime = 0;
                boostEffect.Play();
                _boostAmount = 0f;
            }

            if (boost) {
                curBoostTime += Time.deltaTime;
                
                if (curBoostTime > boostTime) //boost end
                {
                    boost = false;
                    velocity = this.transform.up * 8;
                    boostEffect.Stop();
                }
                else {
                    var BOOST_HIT_RADIUS = 0.4f;
                    this.transform.position += this.transform.up * Time.deltaTime * boostSpeed;
                    _leftThruster.SetIntensity(1);
                    _rightThruster.SetIntensity(1);

                    LayerMask mask = LayerMask.GetMask("Default");
                    var overlappedColliders = Physics2D.OverlapCircleAll(transform.position, BOOST_HIT_RADIUS, mask);

                    foreach (var collider in overlappedColliders) {
                        var hitObject = collider.gameObject;
                        var ai = hitObject.GetComponent<WanderingAI>();

                        if (ai != null && ai._alive) {
                            var isCargoship = ai.running;
                            hitObject.GetComponent<ReactiveTarget>().ReactToHit();

                            if (isCargoship) _scoreController.AddScore(200);
                            else _scoreController.AddScore(50);

                            if (isCargoship) RunStats.Current.CargoShipsDestroyed++;
                            else RunStats.Current.PoliceShipsDestroyed++;
                        }
                    }
                }

            }
            else {
                var hInput = Input.GetAxis("Horizontal");
                var vInput = Input.GetAxis("Vertical");
                
                //Rotating the ship
                float rotation = -hInput * rotationSpeed * Time.deltaTime;
                this.transform.Rotate(0, 0, rotation);

                var leftThrust = vInput;
                var rightThrust = vInput;

                if (Mathf.Abs(hInput) > 0.1f) {
                    if (Mathf.Abs(vInput) < 0.1f) {
                        leftThrust = hInput;
                        rightThrust = -hInput;
                    } else {
                        leftThrust += hInput * 0.1f;
                        rightThrust -= hInput * 0.1f;
                    }
                }

                //The propulsion force, in the direction the ship is pointed
                Vector2 propulsion = Vector2.zero;
                if (!isDisabled())
                {
                    propulsion = transform.up * (propForce * vInput * propulsionCoeff);
                    _leftThruster.SetIntensity(leftThrust);
                    _mainThruster.SetIntensity(vInput);
                    _rightThruster.SetIntensity(rightThrust);
                }


                Vector2 totalForce = Vector2.zero;

                //If the player presses shift
                if ((Input.GetKey(KeyCode.LeftShift) && !isDisabled()) )
                {
                    //Don't calculate forces from gravity or propulsion
                    //Apply a force opposite to the velocity to stop the ship
                    //Should we add gravity to this? Or is that too much
                    totalForce = -(velocity.normalized) * stoppingForce;
                }
                else if(Input.GetKey(KeyCode.L)){
                    totalForce = -(velocity.normalized) * stoppingForce;
                }
                //Otherwise, calculate gravity and propulsion like normal
                else
                {
                    GameObject[] cachedPlanets = new GameObject[planets.Count];
                    planets.CopyTo(cachedPlanets);
                    Vector2 drag = 0.5f * velocity.magnitude * velocity.magnitude * dragForce * velocity.normalized;
                    totalForce = gravity(cachedPlanets) + propulsion - drag;
                }

                if(Input.GetKeyDown(KeyCode.L)){
                    _looting = true;
                    setDisabled(true);
                    //want to apply some force here to slow down so stays in range, similar to leftshift
                }

                if(Input.GetKeyUp(KeyCode.L)){
                    _looting = false;
                    setDisabled(false);
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
            speedEffect.Stop();
        }
    }

    private Vector2 DashIndicatorSize() {
        var originalSize = dashIndicator.size;
        if (!_chargingBoost) {
            originalSize.y = 0;
            return originalSize;
        }
        
        var dist = _boostAmount * boostSpeed;
        originalSize.y = dist - dashIndicator.transform.localPosition.y;
        return originalSize;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        
        if (_chargingBoost) {
            var dist = _boostAmount * boostSpeed;
            Debug.Log(dist);
            Gizmos.DrawLine(transform.position, transform.position + (transform.up * dist));
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

    public void kill()
    {
        alive = false;
        _lastUsableVelocity = Vector2.zero;
    }

    public Vector2 GetVelocity()
    {
        return _lastUsableVelocity;
    }

    /*
     * Adds a planet to calculate gravity from
     */
    public bool addPlanet(GameObject planet)
    {
        Planet p = planet.GetComponent<Planet>();
        if (p != null)
        {
            return planets.Add(planet);
        }
        return false;
    }

    public bool removePlanet(GameObject planet)
    {
        return planets.Remove(planet);
    }

    public void setDisabled(bool disabled)
    {
        _disabled = disabled;
    }

    public bool isDisabled()
    {
        return _disabled;
    }

    public void speedUp(float coeff, float duration)
    {
        propulsionCoeff = coeff;
        speedup_duration = duration;
        startSpeedUpTimer();
        speedEffect.Play();
    }

    void startSpeedUpTimer()
    {
        speedUpTimer = speedup_duration;
    }

    public void ObtainPowerup(Hud.PowerupId id, int amountGained = 1)
    {
        _hud.GainPowerup(id, amountGained);
        _powerupSound.Play();
    }
}
