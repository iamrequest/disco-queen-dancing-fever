using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FloorInteractorButton : MonoBehaviour {
    private FloorInteractor floorInteractor;
    public INPUT_DIRS inputDir;

    private void Awake() {
        floorInteractor = GetComponentInParent<FloorInteractor>();
        if (!floorInteractor) {
            Debug.LogWarning("This floor interactor button is missing a floor interactor in its parent!");
        }
    }
    private void OnTriggerEnter(Collider other) {
        floorInteractor.SendInput(inputDir);
    }
}
