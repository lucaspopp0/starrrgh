using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class AlwaysUprightCanvas: MonoBehaviour {

    private void LateUpdate() {
        transform.rotation = Quaternion.identity;
    }

}