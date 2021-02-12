using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleCtrl : MonoBehaviour
{
    private GameObject appleColl;
    private GameObject appleSprite;
    private GameObject appleLeaf;
    private GameObject firstHalfApple;
    private GameObject secondHalfApple;
    private float force = 100;
    private float randomDirForceX = 1f;

    private float waveFrequency = 2;
    private float waveWidth = 0.5f;
    private float waveRotY = 45;

    private float x0;
    private float birthTime;

    private bool isFallAppleLeaf = false;

    static private int randomDir = 1;
    

    private void Awake()
    {
        appleColl = transform.GetChild(0).gameObject;
        appleSprite = transform.GetChild(1).gameObject;
        appleLeaf = transform.GetChild(2).gameObject;
        firstHalfApple = transform.GetChild(3).gameObject;
        secondHalfApple = transform.GetChild(4).gameObject;
    }


    public void DestroyApple()
    {
        appleColl.SetActive(false);
        appleSprite.SetActive(false);

        DivisionApple(firstHalfApple);
        DivisionApple(secondHalfApple);

        appleLeaf.SetActive(true);

        isFallAppleLeaf = true;
        waveWidth = Random.Range(0.5f, 1) * randomDir;
        randomDir *= -1;
        x0 = appleLeaf.transform.position.x;
        birthTime = Time.time;
        transform.SetParent(null);
    }


    private void DivisionApple(GameObject halfApple)
    {
        force = Random.Range(50, 100);
        Vector2 dirForce = new Vector2(Random.Range(0, randomDirForceX), 1.5f);

        halfApple.SetActive(true);
        Rigidbody2D rb = halfApple.GetComponent<Rigidbody2D>();

        rb.AddForce(dirForce * force);
        rb.AddTorque(randomDirForceX * 500);
        rb.gravityScale = 2;

        randomDirForceX *= -1; //чтобы вторая половинка летела в противположную сторону
    }


    private void Update()
    {
        if (isFallAppleLeaf)
        {
            FallAppleLeaf();
        }
    }


    private void FallAppleLeaf()
    {
        Vector3 tempPos = appleLeaf.transform.position;
        float age = Time.time - birthTime;
        float theata = Mathf.PI * 2 * age / waveFrequency;
        float sin = Mathf.Sin(theata);
        tempPos.x = x0 + waveWidth * sin;

        Vector3 rot = new Vector3(0, sin * waveRotY, 0);
        appleLeaf.transform.rotation = Quaternion.Euler(rot);
        
        appleLeaf.transform.position = tempPos;
    }
}
