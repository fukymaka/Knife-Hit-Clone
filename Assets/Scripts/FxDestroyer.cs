using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxDestroyer : MonoBehaviour
{
    private void Update()
    {
        if (gameObject.GetComponent<ParticleSystem>().isStopped)
        {
            StartCoroutine(DestroyFX());
        }
    }

    IEnumerator DestroyFX()
    {
        yield return null;
        Destroy(this.gameObject);
    }
}
