using UnityEngine;
using UnityEngine.Audio;

public class SimpleEffectsController : MonoBehaviour
{
    [Header("Audio Routing")]
    [Tooltip("Optional AudioSource. If empty, one is created automatically.")]
    public AudioSource audioSource;
    [Tooltip("Optional mixer output group for SFX routing.")]
    public AudioMixerGroup outputMixerGroup;
    [Range(0f, 1f)]
    [Tooltip("Master multiplier for all SFX played by this component.")]
    public float masterVolume = 1f;
    [Tooltip("Slight randomization to avoid repetitive sound.")]
    public Vector2 randomPitchRange = new Vector2(0.95f, 1.05f);
    [Range(0f, 1f)]
    [Tooltip("0 = 2D, 1 = fully 3D/spatial.")]
    public float spatialBlend = 0f;

    [Header("Fallback VFX")]
    [Tooltip("Used when no ParticleSystem prefab is assigned.")]
    public Color fallbackParticleColor = new Color(0.4f, 0.8f, 1f, 1f);

    void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = spatialBlend;
        if (outputMixerGroup != null)
        {
            audioSource.outputAudioMixerGroup = outputMixerGroup;
        }
    }

    public void PlayOneShot(AudioClip clip, float volumeScale = 1f, float pitchMultiplier = 1f)
    {
        if (clip == null || audioSource == null)
        {
            return;
        }

        float minPitch = Mathf.Min(randomPitchRange.x, randomPitchRange.y);
        float maxPitch = Mathf.Max(randomPitchRange.x, randomPitchRange.y);
        audioSource.pitch = Random.Range(minPitch, maxPitch) * pitchMultiplier;
        audioSource.spatialBlend = spatialBlend;
        audioSource.PlayOneShot(clip, Mathf.Clamp01(masterVolume * volumeScale));
    }

    public void PlayDetachedAtPoint(AudioClip clip, Vector3 position, float volumeScale = 1f)
    {
        if (clip == null)
        {
            return;
        }

        GameObject tempAudio = new GameObject("TempSFX");
        tempAudio.transform.position = position;
        AudioSource source = tempAudio.AddComponent<AudioSource>();
        source.clip = clip;
        source.outputAudioMixerGroup = outputMixerGroup;
        source.spatialBlend = spatialBlend;
        source.volume = Mathf.Clamp01(masterVolume * volumeScale);
        source.pitch = Random.Range(Mathf.Min(randomPitchRange.x, randomPitchRange.y), Mathf.Max(randomPitchRange.x, randomPitchRange.y));
        source.Play();
        Destroy(tempAudio, clip.length + 0.1f);
    }

    public void SpawnEffect(ParticleSystem prefab, Vector3 position, float scaleMultiplier = 1f)
    {
        if (prefab != null)
        {
            ParticleSystem spawned = Instantiate(prefab, position, Quaternion.identity);
            spawned.transform.localScale *= Mathf.Max(0.01f, scaleMultiplier);
            float duration = spawned.main.duration + spawned.main.startLifetime.constantMax + 0.25f;
            Destroy(spawned.gameObject, duration);
            return;
        }

        SpawnProceduralBurst(position, scaleMultiplier);
    }

    void SpawnProceduralBurst(Vector3 position, float scaleMultiplier)
    {
        GameObject burst = new GameObject("FallbackBurstVFX");
        burst.transform.position = position;

        ParticleSystem ps = burst.AddComponent<ParticleSystem>();
        var main = ps.main;
        main.loop = false;
        main.duration = 0.2f;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.15f, 0.3f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(1f, 2f);
        float sizeMultiplier = Mathf.Max(0.2f, scaleMultiplier);
        main.startSize = new ParticleSystem.MinMaxCurve(0.06f * sizeMultiplier, 0.12f * sizeMultiplier);
        main.maxParticles = 24;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.startColor = fallbackParticleColor;
        main.gravityModifier = 0f;

        var emission = ps.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 10, 18) });

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.12f * Mathf.Max(0.2f, scaleMultiplier);

        var colorOverLifetime = ps.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new[] { new GradientColorKey(fallbackParticleColor, 0f), new GradientColorKey(fallbackParticleColor, 1f) },
            new[] { new GradientAlphaKey(0.9f, 0f), new GradientAlphaKey(0f, 1f) }
        );
        colorOverLifetime.color = gradient;

        ps.Play();
        Destroy(burst, 0.8f);
    }
}
