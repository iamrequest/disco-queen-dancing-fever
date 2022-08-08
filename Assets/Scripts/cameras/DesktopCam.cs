using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DesktopCam : MonoBehaviour {
    private Camera cam;
    public Transform targetCam;

    [Range(0f, 1f)]
    public float posLerpSpeed, rotLerpSpeed;

    private void Awake() {
        cam = GetComponent<Camera>();
        cam.depth = 50;
    }

    private void FixedUpdate() {
        cam.transform.position = Vector3.Lerp(cam.transform.position, targetCam.position, posLerpSpeed);
        cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, targetCam.rotation, rotLerpSpeed);
    }
}
