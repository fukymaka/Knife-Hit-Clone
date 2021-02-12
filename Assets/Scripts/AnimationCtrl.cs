using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationCtrl : MonoBehaviour
{
    static public AnimationCtrl S;

    public GameObject startScreen;
    public GameObject stageText;
    public GameObject dotStages;
    public GameObject knifeCount;
    public GameObject bossFight;
    public GameObject bossDefeated;

    public GameObject loseScreen;
    public GameObject rewardPanel;
    public GameObject skinsShopScreen;
    public GameObject appleCount;

    public GameObject Background;

    private Animator startScreenAnim;
    private Animator stageTextAnim;
    private Animator dotStagesAnim;
    private Animator knifeCountAnim;
    private Animator bossFightAnim;
    private Animator bossDefeatedAnim;

    private Animator loseScreenAnim;
    private Animator rewardPanelAnim;
    private Animator skinsShopScreenAnim;
    private Animator appleCountAnim;

    private Animator BackgroundAnim;


    private void Awake()
    {
        S = this;

        startScreenAnim = startScreen.GetComponent<Animator>();
        stageTextAnim = stageText.GetComponent<Animator>();
        dotStagesAnim = dotStages.GetComponent<Animator>();
        knifeCountAnim = knifeCount.GetComponent<Animator>();
        bossFightAnim = bossFight.GetComponent<Animator>();
        bossDefeatedAnim = bossDefeated.GetComponent<Animator>();

        loseScreenAnim = loseScreen.GetComponent<Animator>();
        rewardPanelAnim = rewardPanel.GetComponent<Animator>();
        skinsShopScreenAnim = skinsShopScreen.GetComponent<Animator>();
        appleCountAnim = appleCount.GetComponent<Animator>();

        BackgroundAnim = Background.GetComponent<Animator>();
    }
    

    public void StartStageAnim()
    {
        dotStagesAnim.SetTrigger("DotStage_" + GameCtrl.stage % 5);
        stageTextAnim.SetTrigger("FadeIn");
        knifeCountAnim.SetTrigger("FadeIn");

        stageText.GetComponent<Text>().color = Color.white;
    }    


    public void StartBossFightAnim()
    {
        bossFightAnim.SetTrigger("StartAnim");
        dotStagesAnim.SetTrigger("BossFight");
        stageTextAnim.SetTrigger("FadeOut");
    }


    public void EndBossFightAnim()
    {
        bossFightAnim.SetTrigger("EndAnim");
        stageText.GetComponent<Text>().color = new Color(1, 0.25f, 0.2f);
        stageTextAnim.SetTrigger("FadeIn");
        knifeCountAnim.SetTrigger("FadeIn");
    }


    public void StartBossDefAnim()
    {
        bossDefeatedAnim.SetTrigger("BossDefeatedStart");
    }


    public void EndBossDefAnim()
    {
        bossDefeatedAnim.SetTrigger("BossDefeatedEnd");
    }    


    public void LoseScreenStart()
    {
        StartCoroutine(DelayedLoseScreenStart());
    }


    IEnumerator DelayedLoseScreenStart()
    {
        yield return new WaitForSeconds(1);
        stageTextAnim.SetTrigger("FadeOut");
        knifeCountAnim.SetTrigger("FadeOut");
        dotStagesAnim.SetTrigger("FadeOut");
        UICtrl.S.LoseScreenActivate();

        yield return new WaitForSeconds(0.2f);

        foreach (Animator anim in GameCtrl.S.woodLog.GetComponentsInChildren<Animator>())
        {
            anim.SetTrigger("Out");
        }

        if (GameCtrl.S.currentKnife != null)
        {
            GameCtrl.S.currentKnife.GetComponent<Animator>().SetTrigger("Out");
        }
    }


    public void LoseScreenEnd()
    {
        loseScreenAnim.SetTrigger("End");
        appleCountAnim.SetTrigger("End");

        if (rewardPanel.activeInHierarchy)
        {
            rewardPanelAnim.SetTrigger("End");
        }
    }


    public void LoseHitAnim()
    {
        BackgroundAnim.SetTrigger("LoseHit");
        knifeCountAnim.SetTrigger("LoseHit");
        dotStagesAnim.SetTrigger("LoseHit");
        GameCtrl.S.woodLog.GetComponent<Animator>().SetTrigger("LoseHit");
    }


    public void SkinsScreenStart()
    {
        startScreenAnim.SetTrigger("FadeIn");

        if (GameCtrl.S.knifeOnStartScreen != null)
        {
            GameCtrl.S.knifeOnStartScreen.GetComponent<Animator>().SetTrigger("Out");
        }

        skinsShopScreenAnim.SetTrigger("Start");
    }


    public void SkinsScreenEnd()
    {
        startScreenAnim.SetTrigger("FadeOut");

        if (GameCtrl.S.knifeOnStartScreen != null)
        {
            GameCtrl.S.knifeOnStartScreen.GetComponent<Animator>().SetTrigger("FadeIn");
        }
    }
}
