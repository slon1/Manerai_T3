using DG.Tweening;
using UnityEngine;

interface IFaceHitAnimator {
	void AnimateBlendShape(int index, float targetWeight);
	void SetMeshRenderer(SkinnedMeshRenderer skinnedMeshRenderer);
}

public class FaceHitAnimator : IFaceHitAnimator {
	private SkinnedMeshRenderer skinnedMeshRenderer;
	private float animationDuration;
	private float fadeOutDuration;
	private bool autoFadeOut;

	public FaceHitAnimator(float animationDuration, float fadeOutDuration, bool autoFadeOut) {
		this.animationDuration = animationDuration;
		this.fadeOutDuration = fadeOutDuration;
		this.autoFadeOut = autoFadeOut;
	}

	public void SetMeshRenderer(SkinnedMeshRenderer skinnedMeshRenderer) {
		this.skinnedMeshRenderer = skinnedMeshRenderer;
	}

	public void AnimateBlendShape(int index, float targetWeight) {
		if (skinnedMeshRenderer == null) return;

		DOTween.Kill($"BlendShape_{index}"); // Отменяем предыдущую анимацию

		DOTween.To(
			() => skinnedMeshRenderer.GetBlendShapeWeight(index),
			w => skinnedMeshRenderer.SetBlendShapeWeight(index, w),
			targetWeight,
			animationDuration
		).SetId($"BlendShape_{index}")
		.OnComplete(() => {
			if (autoFadeOut) {
				DOTween.To(
					() => skinnedMeshRenderer.GetBlendShapeWeight(index),
					w => skinnedMeshRenderer.SetBlendShapeWeight(index, w),
					0f,
					fadeOutDuration
				);
			}
		});
	}
}
