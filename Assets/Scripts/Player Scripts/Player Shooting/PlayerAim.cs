using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Animator shootAnimator;
    public Transform AimGunEndPointTransform;
 
    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs : EventArgs
    {
        public Vector3 gunEndPointPosition;//What angle the bullet will be firing from
        public Vector3 shootPosition;
    }
    //Find the mouse position in the world with Z = 0
    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }//If we want to have some sort of weapon  rotation it will need to be in respect to Z only 

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 mousePosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(mousePosition);
        return worldPosition;
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = GetMouseWorldPosition();
           // shootAnimator.SetBool("Shoot",true);
            OnShoot?.Invoke(this,new OnShootEventArgs
            {
                gunEndPointPosition = AimGunEndPointTransform.position,
                shootPosition = mousePosition,
     
            });
   
        }

    }

    private void Update()
    {
        HandleShooting();
    }
}