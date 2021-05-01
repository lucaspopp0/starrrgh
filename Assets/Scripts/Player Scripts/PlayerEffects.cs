using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerEffects : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float aberrationReleaseTime = 1.0f;
    [SerializeField] private float aberrationChargeSpeed = 20.0f;
    private PostProcessVolume volume;
    private ChromaticAberration aberration;

    private float intensity = 0.0f;
    private bool releaseChromatic = false;
    private bool chargeChromatic = false;

    private float cameraSize;
    private float targetZoom = 4.0f;
    private float zoomSpeed = 1.0f;
    private float time = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        volume = mainCamera.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings<ChromaticAberration>(out aberration);
        cameraSize = mainCamera.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        intensity = Mathf.Clamp(intensity, 0, 1);
        if (Input.GetKey(KeyCode.Space) && intensity != 1)
        {
            mainCamera.orthographicSize = cameraSize + 0.5f - sigmoid(time, (cameraSize - targetZoom) * 2, zoomSpeed);
            time += Time.deltaTime;
            intensity += Time.deltaTime * aberrationChargeSpeed;
            aberration.intensity.value = intensity;
        }
        else if(intensity != 0)
        {
            intensity -= Time.deltaTime / aberrationReleaseTime;
            aberration.intensity.value = intensity;
        }
        
        if(Input.GetKeyUp(KeyCode.Space))
        {
            time = 0.0f;
            mainCamera.orthographicSize = cameraSize;
        }
    }

    private float sigmoid(float t, float a, float b)
    {
        return b / (1 + Mathf.Exp(-a * t));
    }
}
