using UnityEngine;

public class SyncParticleSizeWithCurve : MonoBehaviour
{
    public ParticleSystem targetParticleSystem;
    private ParticleSystem.MainModule psMain;
    private ParticleSystem.SizeOverLifetimeModule psSizeOverLifetime;
    private SphereCollider sphereCollider;
    private float originalColliderSize;

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        if (sphereCollider == null)
        {
            Debug.LogError("SphereCollider component missing on this GameObject.");
            return;
        }

        originalColliderSize = sphereCollider.radius;

        if (targetParticleSystem != null)
        {
            psMain = targetParticleSystem.main;
            psSizeOverLifetime = targetParticleSystem.sizeOverLifetime;
        }
        else
        {
            Debug.LogError("ParticleSystem reference not set.");
        }
    }

    private void Update()
    {
        if (targetParticleSystem != null && sphereCollider != null)
        {
            float scaleFactor = psMain.startSize.constant * SampleSizeOverLifetimeCurve();
            sphereCollider.radius = originalColliderSize * scaleFactor;
        }
    }

    private float SampleSizeOverLifetimeCurve()
    {
        if (psSizeOverLifetime.enabled)
        {
            float lifeTime = psMain.startLifetime.constant;
            float lifeProgress = Mathf.Clamp01(targetParticleSystem.time / lifeTime);
            return psSizeOverLifetime.size.curve.Evaluate(lifeProgress);
        }

        return 1f;
    }
}
