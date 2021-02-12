using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICtrl : MonoBehaviour
{
    static public UICtrl S;

    public GameObject startScreen;
    public GameObject stageScreen;
    public GameObject dotStages;
    public GameObject knifePrefab;

    public Text knifeScoreTextInLoseScreen;
    public Text knifeHighScoreTextInStartScreen;
    public Text stageScoreInLoseScreenText;
    public Text stageHichScoreInStartScreenText;    
    public Text appleCountText;

    public Text currentStageText;

    public GameObject loseScreen;
    public GameObject skinScreen;
    public GameObject rewardPanel;


    public GameObject knifeCountAnchor;
    public GameObject knifeIconPrefab;
    public List<GameObject> knifeCountIcons;

    private int newSkinIndex = 0;


    private void Awake()
    {
        S = this;
    }


    private void Start()
    {
        knifeHighScoreTextInStartScreen.text = "SCORE " + HighScore.knifeHighScore;
        stageHichScoreInStartScreenText.text = "STAGE " + (HighScore.stageHighScore + 1);
    }


    private void Update()
    {
        appleCountText.text = SaveSkins.appleCoins.ToString();
    }


    public void SetCurrentStageText(string text)
    {
        currentStageText.text = text;
    }


    public void CreateKnifeIcons(int knifeCount)
    {        
        if (knifeCountIcons != null)
        {
            foreach (GameObject knifeIcon in knifeCountIcons)
            {
                Destroy(knifeIcon);
            }
        }

        knifeCountIcons = new List<GameObject>();

        for (int i = 0; i < knifeCount; i++)
        {
            knifeCountIcons.Add(Instantiate(knifeIconPrefab, knifeCountAnchor.transform));
            knifeCountIcons[i].transform.localPosition += new Vector3(0, i * 60, 0);
        }        
    }


    public void SubtractKnifeIcon(int knifeCount)
    {    
        knifeCountIcons[knifeCount].GetComponent<Image>().color = Color.black;
    }


    public void LoseScreenActivate()
    {
        loseScreen.SetActive(true);
        knifeScoreTextInLoseScreen.text = GameCtrl.knifeScore.ToString();
        stageScoreInLoseScreenText.text = "STAGE " + (GameCtrl.stageScore + 1);

        if (GameCtrl.stage > GameCtrl.S.woodLogs.Length - 1)
        {
            stageScoreInLoseScreenText.text = "You have completed all stages";
        }

        if (GameCtrl.newSkins.Count > 0)
        {
            RewardPanelStart();
        }
    }


    public void LoseScreenDeactivate()
    {
        rewardPanel.SetActive(false);
        loseScreen.SetActive(false);

        GameCtrl.newSkins = new List<string>();
    }


    public void RewardPanelStart()
    {
        rewardPanel.SetActive(true);

        rewardPanel.transform.GetChild(0).GetComponent<Image>().sprite = SkinShop.S.GetKnifeSkinByName(GameCtrl.newSkins[0]);  
        
        if (GameCtrl.newSkins.Count > 1)
        {
            rewardPanel.transform.GetChild(2).gameObject.SetActive(true);
        }
    }


    public void NextRewardButton()
    {
        newSkinIndex++;

        if (newSkinIndex == GameCtrl.newSkins.Count)
        {
            newSkinIndex = 0;
        }

        rewardPanel.transform.GetChild(0).GetComponent<Image>().sprite = SkinShop.S.GetKnifeSkinByName(GameCtrl.newSkins[newSkinIndex]);
    }


    public void SetRewardCurrentSkinButton()
    {
        SaveSkins.currentSkin = GameCtrl.newSkins[newSkinIndex];
        SkinShop.S.SetCurrentSkin();
        SaveSkins.Save();
    }


    public void ResetHighScore()
    {
        HighScore.ResetScore();
        SaveSkins.ResetData();
        GameCtrl.S.RestartGame();

        knifeHighScoreTextInStartScreen.text = "SCORE " + HighScore.knifeHighScore;
        stageHichScoreInStartScreenText.text = "STAGE " + (HighScore.stageHighScore + 1);
    }


    public void SkinsScreenOpen()
    {
        skinScreen.transform.GetChild(0).gameObject.SetActive(true);
        AnimationCtrl.S.SkinsScreenStart();

        SkinShop.S.nameSkinText.text = SaveSkins.currentSkin;
        SkinShop.S.CheckNewSkin();
    }


    public void SkinsScreenClose()
    {
        skinScreen.transform.GetChild(0).gameObject.SetActive(false);
        SkinShop.S.buyNowBtn.gameObject.SetActive(false);
        SkinShop.S.noCoinsText.gameObject.SetActive(false);

        AnimationCtrl.S.SkinsScreenEnd();
    }
}
