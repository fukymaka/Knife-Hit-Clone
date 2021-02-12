using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SkinShop : MonoBehaviour
{
    [System.Serializable] class Skins
    {        
        public Sprite knifeSkinSprite;
        public int price = 100;
        public bool isPurchased = false;
        [Header("Name skin/boss name")]
        public string nameSkin;

        [HideInInspector]
        public GameObject knifeSkin;
    }

    static public SkinShop S;

    [Header("Set in Inspector")]
    [SerializeField] private List<Skins> skinList;
    public GameObject currentSkinAnchor;
    public Button buyNowBtn;
    public Text nameSkinText;
    public Text noCoinsText;
    public int unlockRandomSkinPrice = 20;

    [SerializeField] private Transform shopScrollView;
    private GameObject skinTemplate;   
    private Button skinBtn;
    private Color skinBtnColor = new Color(0, 0.35f, 0.35f, 1);


    private void Awake()
    {
        S = this;
    }

    private void Start()
    {
        skinTemplate = shopScrollView.GetChild(0).gameObject;

        for (int i = 0; i < skinList.Count; i++)
        {
            skinList[i].knifeSkin = Instantiate(skinTemplate, shopScrollView);
            skinList[i].knifeSkin.transform.GetChild(1).GetComponent<Image>().sprite = skinList[i].knifeSkinSprite;

            skinBtn = skinList[i].knifeSkin.transform.GetChild(2).GetComponent<Button>();
            skinBtn.AddEventListener(i, SkinButtonClick);            

            SaveSkins.WriteSkins(skinList[i].nameSkin, skinList[i].isPurchased);
        }

        SaveSkins.LoadGame();

        CheckNewSkin();
        
        Destroy(skinTemplate);
    }


    public void CheckNewSkin()
    {
        SetCurrentSkin();

        for (int i = 0; i < skinList.Count; i++)
        {
            if (SaveSkins.skins.ContainsKey(skinList[i].nameSkin))
            {
                skinList[i].isPurchased = SaveSkins.skins[skinList[i].nameSkin];
            }
            else
            {
                Debug.LogError("Skin name rewritten. Please Reset Save");   
            }

            if (!skinList[i].isPurchased)
            {
                skinList[i].knifeSkin.transform.GetChild(1).GetComponent<Image>().color = Color.black;
            }
            else
            {
                skinList[i].knifeSkin.transform.GetChild(1).GetComponent<Image>().color = Color.white;
            }            
        }
    }


    public void OpenNewKnifeSkin(string bossName)
    {
        if (!SaveSkins.skins[bossName])
        {            
            SaveSkins.skins[bossName] = true;
            
            SaveSkins.Save();

            CheckNewSkin();            
        }
    }


    public void SetCurrentSkin()
    {
        for (int i = 0; i < skinList.Count; i++)
        {
            skinList[i].knifeSkin.transform.GetChild(0).GetComponent<Image>().color = Color.clear;

            if (skinList[i].nameSkin == SaveSkins.currentSkin)
            {
                currentSkinAnchor.transform.GetChild(0).GetComponent<Image>().sprite = skinList[i].knifeSkinSprite;
                currentSkinAnchor.transform.GetChild(0).GetComponent<Image>().color = Color.white;

                skinList[i].knifeSkin.transform.GetChild(0).GetComponent<Image>().color = skinBtnColor;

                GameCtrl.S.knifePrefab.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = skinList[i].knifeSkinSprite;

                if (GameCtrl.S.knifeOnStartScreen != null)
                {
                    GameCtrl.S.knifeOnStartScreen.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = skinList[i].knifeSkinSprite;
                }
            }
        }

        nameSkinText.text = SaveSkins.currentSkin;
        SaveSkins.Save();
    }


    public void SkinButtonClick(int skinIndex)
    {     
        buyNowBtn.gameObject.SetActive(false);
        noCoinsText.gameObject.SetActive(false);
        nameSkinText.text = skinList[skinIndex].nameSkin;

        for (int i = 0; i < skinList.Count; i++)
        {
            skinList[i].knifeSkin.transform.GetChild(0).GetComponent<Image>().color = Color.clear;
        }

        skinList[skinIndex].knifeSkin.transform.GetChild(0).GetComponent<Image>().color = skinBtnColor;

        if (skinList[skinIndex].isPurchased)
        {
            SaveSkins.currentSkin = skinList[skinIndex].nameSkin;
            SetCurrentSkin();
            SaveSkins.Save();
            return;
        }
        else
        {
            buyNowBtn.gameObject.SetActive(true);
            buyNowBtn.transform.GetChild(0).GetComponent<Text>().text = skinList[skinIndex].price.ToString();

            buyNowBtn.onClick.RemoveAllListeners();
            buyNowBtn.onClick.AddListener(() => BuyNowButtonClick(skinIndex));

            currentSkinAnchor.transform.GetChild(0).GetComponent<Image>().sprite = skinList[skinIndex].knifeSkinSprite;
            currentSkinAnchor.transform.GetChild(0).GetComponent<Image>().color = Color.black;
        }
    }    
    

    public void BuyNowButtonClick(int skinIndex)
    {
        buyNowBtn.gameObject.SetActive(false);

        if (SaveSkins.appleCoins >= skinList[skinIndex].price)
        {            
            SaveSkins.appleCoins -= skinList[skinIndex].price;
            SaveSkins.currentSkin = skinList[skinIndex].nameSkin;
            SaveSkins.skins[skinList[skinIndex].nameSkin] = true;
            skinList[skinIndex].isPurchased = true;
            SetCurrentSkin();            
            SaveSkins.Save();

            CheckNewSkin();
        }   
        else
        {
            noCoinsText.gameObject.SetActive(true);
            noCoinsText.text = "No coins!";
        }
    }


    public void UnlockRandomSkin()
    {
        List<string> lockedSkins = new List<string>();

        foreach (string s in SaveSkins.skins.Keys)
        {
            if (SaveSkins.skins[s] == false)
            {
                lockedSkins.Add(s);
            }            
        }

        if (lockedSkins.Count == 0)
        {
            noCoinsText.gameObject.SetActive(true);            
            noCoinsText.text = "All skins are open!";
            buyNowBtn.gameObject.SetActive(false);
            return;
        }
        else
        {
            noCoinsText.text = "No coins!";
        }

        int randomNum = Random.Range(0, lockedSkins.Count);

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].nameSkin == lockedSkins[randomNum])
            {
                if (SaveSkins.appleCoins >= unlockRandomSkinPrice)
                {
                    OpenNewKnifeSkin(skinList[i].nameSkin);
                    SaveSkins.currentSkin = skinList[i].nameSkin;
                    SetCurrentSkin();
                    SaveSkins.appleCoins -= unlockRandomSkinPrice;
                }
                else
                {
                    noCoinsText.gameObject.SetActive(true);
                }
            }
        }
    }


    public Sprite GetKnifeSkinByName(string name)
    {
        Sprite getSkin = null;

        for (int i = 0; i < skinList.Count; i++)
        {
            if (skinList[i].nameSkin == name)
            {
                getSkin = skinList[i].knifeSkinSprite;
            }
        }

        return getSkin;
    }

}
