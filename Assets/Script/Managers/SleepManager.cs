using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepManager : MonoBehaviour
{
    public static SleepManager Instance { get; private set; }

    [SerializeField] private GameObject panel;
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] public float fadeDuration = 3f;
    [HideInInspector] public bool isSleeping = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        fadeCanvasGroup.alpha = 0f;
    }

    public IEnumerator FadeToBlack()
    {
        panel.gameObject.SetActive(true);
        isSleeping = true;
        yield return Fade(1f);
        LightingManager.Instance.SetNight();
    }

    public IEnumerator FadeFromBlack()
    {
        yield return Fade(0f);
        panel.gameObject.SetActive(false);
        isSleeping = false;
    }

    private IEnumerator Fade(float endAlpha)
    {
        float startAlpha = fadeCanvasGroup.alpha;
        float time = 0f;

        while(time < fadeDuration)
        {
            time += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = endAlpha;
    }
}