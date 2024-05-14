using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public GameObject[] platforms; // array of the platforms
    private int currentIndex = 0; // index of the platforms to move up
    private float timer = 0.0f;
    public float interval = 15.0f; // interval of platforms spawn 
    private float fadeDuration = 2f;
    private bool allPlatformsHidden = false;

    private void Update()
    {
        int destroyedCount = CrystalAnimation.destroyedCrystalCount; 

        if (destroyedCount > 0)
        {
            if (!allPlatformsHidden)
            {
                Debug.Log("hiding all platforms");
                HideAllPlatforms();
                allPlatformsHidden = true;
            }

            timer += Time.deltaTime;

            if (timer >= interval)
            {
                TogglePlatforms();
                timer = 0.0f;
                currentIndex = (currentIndex + 1) % platforms.Length;  // get index of next platforms in array
            }
        }
    }

    private void HideAllPlatforms()
    {
        for (int i = 0; i < platforms.Length; i++)
        {
            StartCoroutine(HidePlatformCoroutine(platforms[i]));
        }
    }

    private void TogglePlatforms()
    {
        for (int i = 0; i < platforms.Length; i++)
        {
            if (i == currentIndex) // show current platform
            {
                ShowPlatform(platforms[i]);
            }
            else // hide other platforms
            {
                HidePlatforms(platforms[i]);
            }
        }
    }

    private void ShowPlatform(GameObject platform)
    {
        platform.SetActive(true);
        foreach (Transform child in platform.transform)
        {
            MeshRenderer renderer = child.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                StartCoroutine(FadeRendererCoroutine(renderer, 0f, 1f)); // fade in from alpha 0 to 1
            }
        }
    }

     private void HidePlatforms(GameObject platform)
    {
        StartCoroutine(HidePlatformCoroutine(platform));
    }

    private IEnumerator HidePlatformCoroutine(GameObject platform)
    {
        bool allChildrenDoneFading = false;

        foreach (Transform child in platform.transform)
        {
            MeshRenderer renderer = child.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                StartCoroutine(FadeRendererCoroutine(renderer, 1f, 0f, () =>
                {
                    allChildrenDoneFading = true;
                })); // fade out from alpha 1 to 0
            }
        }

        while (!allChildrenDoneFading)
        {
            yield return null;
        }

        platform.SetActive(false);
    }

   private IEnumerator FadeRendererCoroutine(MeshRenderer renderer, float startAlpha, float endAlpha, System.Action onComplete = null)
    {
        Material material = renderer.material;
        Color color = material.color;
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeDuration)
        {
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            material.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        color.a = endAlpha;
        material.color = color;

        onComplete?.Invoke();
    }
}
