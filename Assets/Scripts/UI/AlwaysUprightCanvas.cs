using System;
using UnityEngine;
using UnityEngine.UI;

public class AlwaysUprightCanvas: MonoBehaviour {

    private void LateUpdate() {
        transform.rotation = Quaternion.identity;
    }

}