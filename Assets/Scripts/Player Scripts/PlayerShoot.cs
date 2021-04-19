using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private PlayerAim playerAim;
    [SerializeField] private Material projectile;

    [SerializeField] private ScoreController _scoreController;

    private void Start()
    {
        playerAim = gameObject.GetComponent<PlayerAim>();
        playerAim.OnShoot += PlayerShoot_OnShoot;
    }
    

    private void PlayerShoot_OnShoot(object sender, PlayerAim.OnShootEventArgs e)
    {
        Debug.Log("Shoot");
        Debug.DrawLine(e.gunEndPointPosition,e.shootPosition,Color.white,5,true);
        CreateWeaponTracer(e.gunEndPointPosition, e.shootPosition);
    }

    public static float GetAngleFromVectorFloat(Vector3 dir) {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    
    private void CreateWeaponTracer(Vector3 fromPosition, Vector3 targetPosition)
    {//If we want to restrict the angle of fire we can add a check for it here
        
        float distance = Vector3.Distance(fromPosition, targetPosition);
        Vector3 dir = (targetPosition - fromPosition).normalized;
        Vector3 tracerSpawnPosition = fromPosition + dir * distance * .5f;
        float zRot = GetAngleFromVectorFloat(dir) - 90;
        Material tmp = new Material(projectile);
        tmp.SetTextureScale("_MainTex",new Vector2(1f,distance/600f));
        World_Mesh worldMesh = World_Mesh.Create(tracerSpawnPosition,zRot,.5f,distance,projectile,null,10000);
         float timer = .1f;
         
         FunctionUpdater.Create(() =>
         {
             timer -= Time.deltaTime;
             if (timer <= 0)
             {
                 worldMesh.DestroySelf();
                 return true;
             }

             return false;
         });
         _scoreController.AddScore(3);
    }

}
