using UnityEngine;
using System.Collections;

public class KC_Collision : MonoBehaviour
{
    public ParticleSystem triggerEffect;  // Assign a particle effect in the inspector
    public AudioClip triggerSound;        // Assign a sound effect in the inspector
    private AudioSource audioSource;

    private void Start()
    {
        // Ensure the object has an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TriggerObject"))  // Ensure correct tag
        {
            Debug.Log("Trigger Entered: " + other.name);

            // Play sound effect if assigned
            if (triggerSound) audioSource.PlayOneShot(triggerSound);

            // Play particle effect if assigned
            if (triggerEffect)
            {
                ParticleSystem effectInstance = Instantiate(triggerEffect, other.transform.position, Quaternion.identity);
                effectInstance.Play();
                Destroy(effectInstance.gameObject, effectInstance.main.duration);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TriggerObject")) 
        {
            Debug.Log("Trigger Exit: " + other.name);

            // Start fading the object before destroying
            StartCoroutine(FadeAndDestroy(other.gameObject));
        }
    }

    private IEnumerator FadeAndDestroy(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer)
        {
            Color originalColor = renderer.material.color;
            float fadeDuration = 1f;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
                renderer.material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }
        }
        Destroy(obj);
    }
}