using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Collider))]
public class FloorInteractorButton : MonoBehaviour {
    private FloorInteractor floorInteractor;
    public INPUT_DIRS inputDir;
    private float timeSinceLastInput;

    public VisualEffect onStepVFX;

    private void Awake() {
        floorInteractor = GetComponentInParent<FloorInteractor>();
        if (!floorInteractor) {
            Debug.LogWarning("This floor interactor button is missing a floor interactor in its parent!");
        }
    }

    private void Update() {
        timeSinceLastInput = Mathf.Min(floorInteractor.inputCooldown, timeSinceLastInput + Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        // Cooldown
        if (timeSinceLastInput < floorInteractor.inputCooldown)  return;

        // Check that the layermasks match
        if ((floorInteractor.buttonLayerMask.value & (1 << other.gameObject.layer)) > 0) {
            OnPress();
        }
    }

    [Button]
    private void OnPress() {
        floorInteractor.SendInput(inputDir);
        onStepVFX.Play();

        timeSinceLastInput = 0f;
    }
}
