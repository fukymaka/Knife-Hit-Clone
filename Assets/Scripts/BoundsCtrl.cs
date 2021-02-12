using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsCtrl : MonoBehaviour
{
    private bool isDestroy = false;


    private void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).position.y < -12)
            {
                isDestroy = true;
            }
        }

        if (transform.position.y < -12 | isDestroy)
        {
            StartCoroutine(DestroyObj());
        }
    }


    IEnumerator DestroyObj()
    {
        Destroy(gameObject);
        yield return 0;
    }
}
