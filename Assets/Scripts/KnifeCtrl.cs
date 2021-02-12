using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeCtrl : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject loseHitFXPrefab;

    [Header("Set in Script")]
    public bool inWood = false;
    public bool isStartKnife = false;

    private Rigidbody2D rb;
    static private int dirForceTorkue = 1; //for random dir torkue

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        if (isStartKnife)
        {
            transform.Translate(new Vector2(0, -Time.deltaTime * 10));
        }

        if (transform.position.y < -3.3f || gameObject.layer == 10)
        {
            isStartKnife = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (GameCtrl.S.isBounce) return;
        if (WoodLogCtrl.isExplosion) return;
        if (inWood) return;

        switch (coll.tag)
        {
            case "Apple":
                coll.transform.parent.GetComponent<AppleCtrl>().DestroyApple();
                SaveSkins.appleCoins += 2;
                break;

            case "inWoodKnife":
                AnimationCtrl.S.LoseHitAnim();

                GameObject loseHitFX = Instantiate(loseHitFXPrefab);
                loseHitFX.transform.localPosition = (transform.localPosition - coll.transform.position) * 0.5f;
                loseHitFX.transform.parent = null;

                rb.velocity = Vector2.zero;
                rb.gravityScale = 4;

                int torkue = Random.Range(700, 1500);                
                rb.AddTorque(torkue * dirForceTorkue);
                GameCtrl.S.isBounce = true;
                GameCtrl.S.LoseStage();

                SaveSkins.Save();

                dirForceTorkue *= -1;

                Vibration.Vibrate();

                break;
        }
    }
}
