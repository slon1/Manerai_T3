using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering.Universal;
public interface IBruiseSpawner {
	void SpawnBruise(Vector3 worldPosition, Vector3 normal);
}


public class BruiseAnimator : IBruiseSpawner {
	private readonly ObjectPool<PooledDecal> pool;
	private readonly float lifetime;
	private readonly float fadeDuration;

	public BruiseAnimator(GameObject prefab, float lifetime = 2f, float fadeDuration = 2f) {
		this.lifetime = lifetime;
		this.fadeDuration = fadeDuration;

		pool = new ObjectPool<PooledDecal>(
			createFunc: () => {
				var instance = Object.Instantiate(prefab);				
				return new PooledDecal(instance);
			},
			actionOnGet: decal => {
				decal.GameObject.SetActive(true);
				decal.Projector.fadeFactor = 1f;
			},
			actionOnRelease: decal => {
				decal.GameObject.SetActive(false);
			},
			actionOnDestroy: decal => {
				Object.Destroy(decal.GameObject);
			},
			collectionCheck: false,
			defaultCapacity: 10,
			maxSize: 30
		);
	}

	public void SpawnBruise(Vector3 worldPosition, Vector3 normal) {
		var decal = pool.Get();
		var go = decal.GameObject;
		var projector = decal.Projector;

		go.transform.position = worldPosition + normal * 0.01f;
		go.transform.rotation = Quaternion.LookRotation(-normal);
		go.transform.localScale = Vector3.one * 0.1f;

		float delay = Mathf.Max(0f, lifetime - fadeDuration);
		DOVirtual.DelayedCall(delay, () => {
			DOTween.To(
				() => projector.fadeFactor,
				f => projector.fadeFactor = f,
				0f,
				fadeDuration
			).OnComplete(() => {
				pool.Release(decal);
			});
		});
	}
}
public class PooledDecal {
	public GameObject GameObject { get; private set; }
	public DecalProjector Projector { get; private set; }

	public PooledDecal(GameObject instance) {
		GameObject = instance;
		Projector = instance.GetComponent<DecalProjector>();

		if (Projector == null) {
			Debug.LogError("PooledDecal: prefab must have DecalProjector component");
		}
	}
}