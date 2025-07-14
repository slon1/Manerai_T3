using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "FaceDamageConfig", menuName = "Face/Face Damage Config")]
public class FaceDamageConfig : ScriptableObject {
	[Header("BlendShape Zones")]
	public List<FaceZoneEntry> zones = new();

	[Header("Hit Points")]
	public List<HitZonePoint> points = new();

	[Header("BlendShape Animation")]
	[Range(0.01f, 1f)] public float animationDuration = 0.15f;
	[Range(0.01f, 2f)] public float fadeOutDuration = 0.4f;
	public bool autoFadeOut = true;

	[Header("Bruise Settings")]
	public GameObject bruisePrefab;
	public float bruiseLifetime = 2f;
	public float bruiseFadeDuration = 2f;
}

[System.Serializable]
public class FaceZoneEntry {
	public FaceHitZone hitZone;
	public List<FaceBlendShape> blendShapes;
	public float intensity = 100f;
}
