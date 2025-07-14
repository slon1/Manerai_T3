using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFaceHitClickDetector {
	event Action<Vector3, Vector3, Vector3> OnClick;
}
public class FaceHitClickDetector : MonoBehaviour, IFaceHitClickDetector {

	public event Action<Vector3, Vector3, Vector3> OnClick;

	private void Update() {
		if (Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit)) {
				OnClick?.Invoke(hit.point, hit.transform.InverseTransformPoint(hit.point), hit.normal);
			}
		}
	}
}