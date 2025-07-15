using UnityEngine;

public class RagdollPuncher : MonoBehaviour {
	[SerializeField] private float punchForce = 500f;	
	[SerializeField] private bool enableGravity = true;
	[SerializeField] private Transform ragdollRoot; // корень ragdoll-а (например, "Hips")

	private Rigidbody[] ragdollRigidbodies;

	void Start() {
		if (ragdollRoot != null) {
			ragdollRigidbodies = ragdollRoot.GetComponentsInChildren<Rigidbody>();
			ApplyGravityToRagdoll();
		}
		else {
			Debug.LogWarning("Ragdoll root not assigned.");
		}
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit)) {
				Rigidbody hitRb = hit.collider.attachedRigidbody;
				if (hitRb != null && !hitRb.isKinematic) {
					Vector3 direction = hit.point - ray.origin;
					direction.Normalize();

					hitRb.AddForceAtPosition(direction * punchForce, hit.point, ForceMode.Impulse);
				}
			}
		}
	}

	void OnValidate() {
		// В редакторе, при переключении галки — сразу применить
		if (ragdollRoot != null) {
			ragdollRigidbodies = ragdollRoot.GetComponentsInChildren<Rigidbody>();
			ApplyGravityToRagdoll();
		}
	}

	private void ApplyGravityToRagdoll() {
		if (ragdollRigidbodies == null) return;

		foreach (var rb in ragdollRigidbodies) {
			rb.useGravity = enableGravity;
		}
	}
}
