using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class TeleportationSkill : MonoBehaviour
{
    public Camera cam;
    public GameObject teleportVFXPrefab;
    public Material[] materialsToChange; // Array of materials to change float value
    public string materialFloatPropertyName = "_FloatProperty"; // Name of the float property to change in the material
    public ParticleSystem preTeleportParticleSystem; // Particle system to play before teleportation

    private GameObject teleportVFXInstance;
    private LayerMask layerMask;
    private Vector3 teleportPosition;
    private bool isTeleportationEnabled = false;
    private Rigidbody playerRigidbody;

    void Start()
    {
        layerMask = ~LayerMask.GetMask("Player");
        teleportVFXInstance = Instantiate(teleportVFXPrefab, Vector3.zero, Quaternion.identity);
        teleportVFXInstance.SetActive(false);
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isTeleportationEnabled)
        {
            return;
        }

        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, layerMask))
        {
            teleportPosition = hit.point + Vector3.up * 1.0f;
            teleportVFXInstance.transform.position = teleportPosition;
            teleportVFXInstance.SetActive(true);

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                StartCoroutine(TeleportSequence());
            }
        }
        else
        {
            teleportVFXInstance.SetActive(false);
        }
    }

    IEnumerator TeleportSequence()
    {
        if (preTeleportParticleSystem != null)
        {
            preTeleportParticleSystem.Play(); // Play the assigned particle system
        }

        yield return StartCoroutine(ChangeMaterialFloat(0f, 1f, 0.25f)); // Change float from 0 to 1
        PerformTeleportation();
        yield return StartCoroutine(ChangeMaterialFloat(1f, 0f, 0.25f)); // Change float from 1 to 0
    }

    IEnumerator ChangeMaterialFloat(float startValue, float endValue, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float newValue = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
            foreach (Material mat in materialsToChange)
            {
                mat.SetFloat(materialFloatPropertyName, newValue);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        foreach (Material mat in materialsToChange)
        {
            mat.SetFloat(materialFloatPropertyName, endValue);
        }
    }

    private void PerformTeleportation()
    {
        if (playerRigidbody != null)
        {
            playerRigidbody.MovePosition(teleportPosition);
        }
        else
        {
            transform.position = teleportPosition;
        }
        if (Vector3.Distance(transform.position, teleportPosition) < 1.5f)
        {
            DisableTeleportation();
        }
    }

    public void EnableTeleportation()
    {
        isTeleportationEnabled = true;
    }

    public void DisableTeleportation()
    {
        isTeleportationEnabled = false;
        teleportVFXInstance.SetActive(false);
    }
}
