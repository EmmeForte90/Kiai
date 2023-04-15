using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VibrateCinemachine : MonoBehaviour
{
    public CinemachineVirtualCamera vCam;
    private Transform originalFollow;

    private void Start()
    {
        originalFollow = vCam.Follow;
    }

    public void Vibrate(float amplitude, float duration)
    {
        StartCoroutine(DoVibration(amplitude, duration));
    }

    IEnumerator DoVibration(float amplitude, float duration)
    {
        vCam.Follow = null;

        Vector3 originalPos = vCam.transform.position;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float percentComplete = elapsed / duration;
            float damper = 1f - Mathf.Clamp(4f * percentComplete - 3f, 0f, 1f);

            float x = Random.value * 2f - 1f;
            float y = Random.value * 2f - 1f;
            x *= amplitude * damper;
            y *= amplitude * damper;

            vCam.transform.position = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            yield return null;
        }

        vCam.transform.position = originalPos;
        vCam.Follow = originalFollow;
    }
}
