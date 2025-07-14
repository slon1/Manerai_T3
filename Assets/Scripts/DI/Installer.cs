using System;
using System.Collections;
using UnityEngine;
public class Installer : MonoBehaviour {
	
	private static DIContainer container;
	[SerializeField] FaceDamageConfig config;
	
	
	void Awake() {		
		container = new DIContainer();
		container.Register(config);
		container.Register<IFaceHitClickDetector>(GetComponent<FaceHitClickDetector>());
		container.Register<IFaceHitAnimator>(new FaceHitAnimator(config.animationDuration, config.fadeOutDuration, config.autoFadeOut));
		container.Register<IBruiseSpawner>(new BruiseAnimator(config.bruisePrefab, config.bruiseLifetime, config.bruiseFadeDuration));
	
	}
	
	
	public static T GetService<T>() => container.Resolve<T>();
	private void OnDestroy() {
		container.Dispose();
	}
}
