using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
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

    private Vector2 velocity;

    // Start is called before the first frame update
    void Start()
    {
        velocity = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        //Rotating the ship
        float rotation = -Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        this.transform.Rotate(0, 0, rotation);

        //The propulsion force, in the direction the ship is pointed
        Vector2 propulsion = transform.up * propForce * Input.GetAxis("Vertical");

        Vector2 totalForce = Vector2.zero;

        //If the player presses shift
        if (Input.GetKey(KeyCode.LeftShift))
        {
            //Don't calculate forces from gravity or propulsion
            //Apply a force opposite to the velocity to stop the ship

            //Should we add gravity to this? Or is that too much
            totalForce = -(velocity.normalized) * stoppingForce;
        }
        //Otherwise, calculate gravity and propulsion like normal
        else
        {
            totalForce = gravity(planets) + propulsion;
        }

        //Applying the force to the ship
        applyForce(totalForce);
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
        Vector3 usableVel = velocity;
        this.transform.position += (usableVel * Time.deltaTime);
    }
}
