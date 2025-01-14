using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{ 
    public static ParticleManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SpawnParticles(ParticleSystem particles, Vector3 position, float destroyTime)
    {
        ParticleSystem currentParticles = Instantiate(particles, position, Quaternion.identity);
        StartCoroutine(DestroyParticles(destroyTime, currentParticles));
    }

    private IEnumerator DestroyParticles(float destroyTime, ParticleSystem particles)
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(particles.gameObject);
    }
}