using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeAmount = 0.7f;
    public float shakeDuration = 1.0f;

    private Vector3 originalPosition;
    private float currentShakeDuration;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    public void ShakeCamera()
    {
        currentShakeDuration = shakeDuration;
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        while (currentShakeDuration > 0)
        {
            Vector3 randomPosition = originalPosition + Random.insideUnitSphere * shakeAmount;
            transform.localPosition = new Vector3(randomPosition.x, randomPosition.y, originalPosition.z);
            currentShakeDuration -= Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
