using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Could use an XRRayInteractor, but that expects an XRController. Don't have that on the head
public class GazeRaycaster : MonoBehaviour {
    public LayerMask layerMask;
    public float maxDistance;

    private void Update() {
        TryRaycast();
    }

    private void TryRaycast() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, layerMask)) {
            // Gaze menu colliders are on the child of the GazeMenuButton component. Otherwise, I'd check GetComponent() too, but that's not necessary for my use-case
            GazeMenuButton menuButton = hit.collider.GetComponentInParent<GazeMenuButton>();
            if (menuButton != null) {
                menuButton.OnHover();
            }
        }
    }
}
