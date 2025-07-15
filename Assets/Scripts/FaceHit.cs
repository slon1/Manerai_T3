using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class HitZonePoint {
	public FaceHitZone hitZone;
	public Vector3 localPosition;
}
public class FaceHit : MonoBehaviour {
	[SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;


	private FaceDamageConfig config;
	private IFaceHitAnimator hitAnim;
	private IFaceHitClickDetector detector;
	private IBruiseSpawner bruiseAnimator;

	private Dictionary<FaceHitZone, List<FaceBlendShape>> zoneToBlendShapes;
	void Start() {
		config = Installer.GetService<FaceDamageConfig>();
		detector = Installer.GetService<IFaceHitClickDetector>();
		detector.OnClick += Detector_OnClick;
		hitAnim = Installer.GetService<IFaceHitAnimator>();
		hitAnim.SetMeshRenderer(skinnedMeshRenderer);
		bruiseAnimator = Installer.GetService<IBruiseSpawner>();
		zoneToBlendShapes = new Dictionary<FaceHitZone, List<FaceBlendShape>>();
		foreach (var entry in config.zones) {
			zoneToBlendShapes[entry.hitZone] = entry.blendShapes;
		}

	}
	public void ApplyHit(FaceHitZone zone) {
		if (zoneToBlendShapes.TryGetValue(zone, out var blendShapes)) {
			foreach (var shape in blendShapes) {
				hitAnim.AnimateBlendShape((int)shape, config.zones.Find(z => z.hitZone == zone).intensity);
			}
		}
	}

	private void Detector_OnClick(Vector3 worldPosition, Vector3 localPosition, Vector3 normal) {
		var zone = GetClosestZone(localPosition);
		ApplyHit(zone);
		bruiseAnimator.SpawnBruise(worldPosition, normal);
	}

	public FaceHitZone GetClosestZone(Vector3 localHit) {

		HitZonePoint closest = null;
		float minSqrDist = float.MaxValue;

		foreach (var point in config.points) {
			float sqrDist = (point.localPosition - localHit).sqrMagnitude;
			if (sqrDist < minSqrDist) {
				minSqrDist = sqrDist;
				closest = point;
			}
		}
		return closest != null ? closest.hitZone : default;
	}


	private void OnDestroy() {
		detector.OnClick -= Detector_OnClick;
		detector = null;
		hitAnim = null;
		bruiseAnimator = null;
		zoneToBlendShapes.Clear();
	}
}
