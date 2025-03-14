using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : Item
{
    public BedItem bedItemData;
    public int SleepAmount => bedItemData.sleepAmount;

    public void Sleep()
    {
        if(Player.Instance.currentFatigue <= 100)
            StartCoroutine(SleepCoroutine());
    }

    private IEnumerator SleepCoroutine()
    {
        if(PlayerMovement.Instance.movementAudioSource.isPlaying)
            PlayerMovement.Instance.movementAudioSource.Stop();

        yield return SleepManager.Instance.FadeToBlack();
        yield return new WaitForSeconds(SleepManager.Instance.fadeDuration);

        Player.Instance.Sleep(SleepAmount);
        yield return SleepManager.Instance.FadeFromBlack();
    }
}