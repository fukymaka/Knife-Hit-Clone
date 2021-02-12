using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameCtrl : MonoBehaviour
{
    static public GameCtrl S;

    static public int knifeScore = 0;    
    static public int stageScore = 0;    

    static public List<string> newSkins = new List<string>();
    static public int stage = 0;    

    [Header("Set in Inspector")]
    public WoodLog[] woodLogs;
    public GameObject knifePrefab;
    public GameObject woodLogPrefab;
    public float speedHit = 1000;
    public float delaySpawnNewKnife = 0.2f;

    [Header("Set in Script")]
    public int knifeCount;        
    public GameObject currentKnife;
    public GameObject knifeOnStartScreen;
    public GameObject woodLog;
    public GameObject woodLogAnchor;
    public bool isBossDefeated = false;
    public bool isLoseStage = false;
    public bool isBounce = false;


    private void Awake()
    {
        S = this;
    }


    private void Start()
    {
        knifeOnStartScreen = Instantiate(knifePrefab);
        knifeOnStartScreen.transform.position = new Vector2(0, -1);

        SkinShop.S.SetCurrentSkin();

        Vibration.Init();
    }


    private void Update()
    {
        if (isLoseStage || isBounce || isBossDefeated) return;

        if (Input.GetMouseButtonDown(0))
        {
            KnifeHit();
        }
    }    

    public void StartNewStage(int currentStage)
    {
        stage = currentStage;
        isBounce = false;
        
        if (stage == 0)
        {
            knifeScore = 0;
        }

        UICtrl.S.startScreen.GetComponent<CanvasGroup>().blocksRaycasts = false; //от повторного нажатия кнопки        

        if (stage > woodLogs.Length - 1) //stages is over
        {
            LoseStage();
            return;
        }

        StartCoroutine(StartStage());
    }

    IEnumerator StartStage()
    {
        if (isBossDefeated)
        {
            isBossDefeated = false;
            AnimationCtrl.S.StartBossDefAnim();
            yield return new WaitForSeconds(2);
            AnimationCtrl.S.EndBossDefAnim();
                        
            if (SaveSkins.skins.ContainsKey(woodLogs[stage - 1].bossName) && !SaveSkins.skins[woodLogs[stage - 1].bossName])
            {
                newSkins.Add(woodLogs[stage - 1].bossName);
                SkinShop.S.OpenNewKnifeSkin(woodLogs[stage - 1].bossName);
            }           
        }
        
        if (isLoseStage == true)
        {
            UICtrl.S.loseScreen.GetComponent<CanvasGroup>().blocksRaycasts = false;
            AnimationCtrl.S.LoseScreenEnd();
            yield return new WaitForSeconds(0.5f);
            UICtrl.S.LoseScreenDeactivate();
            isLoseStage = false;            
        }

        if (woodLogs[stage].type == LogType.simple)
        {
            yield return null;
            AnimationCtrl.S.StartStageAnim();
            UICtrl.S.SetCurrentStageText("STAGE " + (stage + 1));
        }
        else if (woodLogs[stage].type == LogType.boss)
        {
            AnimationCtrl.S.StartBossFightAnim();
            yield return new WaitForSeconds(2);
            AnimationCtrl.S.EndBossFightAnim();
            UICtrl.S.SetCurrentStageText("BOSS: " + woodLogs[stage].bossName);
        }

        if (woodLog != null)
        {
            Destroy(woodLog);
        }

        woodLog = Instantiate(woodLogPrefab, woodLogAnchor.transform);        

        if (currentKnife != null)
        {
            Destroy(currentKnife);
        }        

        knifeCount = woodLogs[stage].knifeCount;
        UICtrl.S.CreateKnifeIcons(knifeCount);

        SpawnNewKnife();
    }

    
    private void KnifeHit()
    {
        if (currentKnife != null)
        {
            currentKnife.layer = 10;

            for (int i = 0; i < currentKnife.transform.childCount; i++)
            {
                currentKnife.transform.GetChild(i).gameObject.layer = 10;
            }

            currentKnife.GetComponent<Rigidbody2D>().AddForce(Vector2.up * speedHit);
            knifeCount--;

            Invoke("SpawnNewKnife", delaySpawnNewKnife);
            currentKnife = null;            
        }
        
    }

    private void SpawnNewKnife()
    {
        if (isBounce || knifeCount == 0)
        {
            return;
        }

        if (knifeOnStartScreen != null)
        {
            currentKnife = knifeOnStartScreen;            
            currentKnife.GetComponent<KnifeCtrl>().isStartKnife = true;
            knifeOnStartScreen = null;
        }
        else
        {
            currentKnife = Instantiate(knifePrefab);
            currentKnife.transform.position = new Vector2(0, -3.3f);            
        }
    }


    public void LoseStage()
    {
        isLoseStage = true;

        AnimationCtrl.S.LoseScreenStart();
        UICtrl.S.loseScreen.GetComponent<CanvasGroup>().blocksRaycasts = true;

        stageScore = stage;

        HighScore.SetKnifeHighScore(knifeScore);
        HighScore.SetStageHighScore(stageScore);
    }

    public void DelayedRestartGame()
    {
        if (UICtrl.S.loseScreen.activeInHierarchy)
        {
            AnimationCtrl.S.LoseScreenEnd();
        }
        
        Invoke("RestartGame", 0.5f);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Scene_0");
    }
}
