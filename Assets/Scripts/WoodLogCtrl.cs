using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum RotateType
{
    none,
    startRotate,
    constRotate,
    stopRotate
}

public class WoodLogCtrl : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject applePrefab;
    public GameObject knifePrefab;
    public GameObject woodLog3D;
    public GameObject hitFXPrefab;

    [Header("Set in Script")]
    public LogType type;

    //value of WoodLog Scriptable Obj
    private string logName;
    private float speedRotation;
    public float durationRotate;
    public float durationStart;
    public float durationStop;
    private float chanceChangeDirRot;
    private float chanceSpawnApple;
    private int numApple;
    private float chanceSpawnKnife;
    private int numKnife;
    private Sprite skinLog;    

    private float tmpTime = 0;
    private int stage;
    private RotateType RotType = RotateType.none;
    private List<Vector3> posOnWood;
    private int minAngleBtwObj = 30; //minimum angle between objects on wood
    static public bool isExplosion = false;
    static public bool isLoseStage = false;


    private void Awake()
    {
        stage = GameCtrl.stage;

        skinLog = GameCtrl.S.woodLogs[stage].skinLog;
        GetComponent<SpriteRenderer>().sprite = skinLog;

        logName = GameCtrl.S.woodLogs[stage].bossName;
        speedRotation = GameCtrl.S.woodLogs[stage].speedRotation;
        durationRotate = GameCtrl.S.woodLogs[stage].durationRotate;
        durationStart = GameCtrl.S.woodLogs[stage].durationStart;        
        durationStop = GameCtrl.S.woodLogs[stage].durationStop;
        chanceChangeDirRot = GameCtrl.S.woodLogs[stage].chanceChangeDirRot;
        chanceSpawnApple = GameCtrl.S.woodLogs[stage].chanceSpawnApple;
        numApple = GameCtrl.S.woodLogs[stage].numApple;
        chanceSpawnKnife = GameCtrl.S.woodLogs[stage].chanceSpawnKnife;
        numKnife = GameCtrl.S.woodLogs[stage].numKnife;
        type = GameCtrl.S.woodLogs[stage].type;
    }


    private void Start()
    {
        StartCoroutine(Cor());
    }
    

    IEnumerator Cor()
    {
        yield return new WaitForEndOfFrame();
        RotType = RotateType.startRotate;

        posOnWood = new List<Vector3>();

        CreateSpawnPoints();

        if (Random.value < chanceSpawnApple / 100)
        {
            SpawnObjOnWood(applePrefab, numApple);
        }

        if (Random.value < chanceSpawnKnife / 100)
        {
            SpawnObjOnWood(knifePrefab, numKnife);
        }
    }


    private void Update()
    {
        if (GameCtrl.S.isLoseStage) return;

        if (!isExplosion)
        {
            RotateWoodLog();
        }
        else
        {
                  
            tmpTime += Time.deltaTime;     

            if (tmpTime > 0.8f)
            {
                tmpTime = 0;
                isExplosion = false;
                stage++;
                GameCtrl.S.StartNewStage(stage);                
            }
        }        
    }


    private void RotateWoodLog()
    {
        float tmpDurStart = tmpTime / durationStart;
        float tmpDurStop = tmpTime / durationStop;

        tmpTime += Time.deltaTime;

        if (tmpTime < durationStart & RotType == RotateType.startRotate)
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * speedRotation * Mathf.SmoothStep(0, 1, tmpDurStart));
        }     
        else if (RotType == RotateType.startRotate)
        {
            RotType = RotateType.constRotate;
            tmpTime = 0;            
        }

        if (tmpTime < durationRotate & RotType == RotateType.constRotate)
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * speedRotation);
        }
        else if (RotType == RotateType.constRotate)
        {
            RotType = RotateType.stopRotate;
            tmpTime = 0;
        }

        if (tmpTime < durationStop & RotType == RotateType.stopRotate)
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * speedRotation * Mathf.SmoothStep(1, 0, tmpDurStop));
        }
        else if (RotType == RotateType.stopRotate)
        {
            RotType = RotateType.startRotate;
            tmpTime = 0;

            float random = Random.value;

            if (chanceChangeDirRot / 100 >= random)
            {
                speedRotation *= -1;
            }
        }
    }


    private void CreateSpawnPoints()
    {
        Vector2 center = transform.localPosition;
        float ang = 0;
        float radius = GetComponent<CircleCollider2D>().radius; //GetComponent<CircleCollider2D>().radius

        for (int i = 0; i < 360 / minAngleBtwObj; i++)
        {
            Vector3 pos = Vector3.zero;
            pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
            pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
            posOnWood.Add(pos);
            ang += minAngleBtwObj;
        }
    }


    private void SpawnObjOnWood(GameObject prefab, int numApple)
    {
        for (int i = 0; i < numApple; i++)
        {
            int randomNumApple = Random.Range(0, posOnWood.Count);
            GameObject go;
            Quaternion rot;
            go = Instantiate(prefab, transform);

            switch (prefab.tag)
            {
                case "Knife":
                    rot = Quaternion.LookRotation(Vector3.forward, transform.localPosition - posOnWood[randomNumApple]);                    
                    go.transform.localRotation = rot;

                    go.transform.Find("Handle").tag = "inWoodKnife";
                    go.GetComponent<KnifeCtrl>().inWood = true;
                    go.transform.localPosition = posOnWood[randomNumApple];
                    break;
                case "Apple":
                    rot = Quaternion.LookRotation(Vector3.forward, posOnWood[randomNumApple] - transform.localPosition);
                    go.transform.localRotation = rot;
                    go.transform.localPosition = posOnWood[randomNumApple] * 1.18f; //поправка на размер яблока
                    break;
            }

            posOnWood.Remove(posOnWood[randomNumApple]);
        }
    }
    

    private void WoodLogExplosion()
    {
        isExplosion = true;
        tmpTime = 0;

        woodLog3D = Instantiate(woodLog3D);
        woodLog3D.GetComponent<WoodLog3D>().skinLog = skinLog.texture;
        woodLog3D.transform.position = transform.position;
        GetComponent<SpriteRenderer>().sprite = null;

        for (int i = 0; i < woodLog3D.transform.childCount; i++)
        {
            GameObject logPart = woodLog3D.transform.GetChild(i).gameObject;
            Rigidbody rb = logPart.GetComponent<Rigidbody>();
            rb.AddForce(Vector2.up * 4, ForceMode.Impulse);
            rb.AddForce((logPart.transform.localPosition - transform.localPosition) * 10, ForceMode.Impulse);
            rb.AddTorque(Random.insideUnitSphere * 200);

            logPart.layer = 9;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject go = transform.GetChild(i).gameObject;
            go.SetActive(true);
            Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
            rb.gravityScale = 4;
            rb.AddForce(Vector2.up * 400);
            rb.AddForce((transform.localPosition - go.transform.localPosition) * 300);
            rb.AddTorque(Random.Range(-300, 300));

            go.layer = 9;
        }

        transform.DetachChildren();

        if (type == LogType.boss)
        {
            GameCtrl.S.isBossDefeated = true;
        }

        long[] pattern = { 0, 50, 50, 50 };
        Vibration.Vibrate(pattern, -1);
    }


    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (GameCtrl.S.isBounce) return;
        if (isExplosion) return;

        switch (coll.tag)
        {
            case "HandleKnife":

                gameObject.GetComponent<Animator>().SetTrigger("Hit");
                                 
                GameObject hitFX = Instantiate(hitFXPrefab);
                hitFX.transform.localPosition = transform.localPosition - coll.transform.position;
                hitFX.transform.parent = null;

                GameCtrl.knifeScore++;

                UICtrl.S.SubtractKnifeIcon(GameCtrl.S.knifeCount);

                coll.tag = "inWoodKnife";

                Transform knife = coll.transform.parent;
                knife.GetComponent<KnifeCtrl>().inWood = true;

                if (GameCtrl.S.knifeCount == 0)
                {
                    WoodLogExplosion();
                    knife.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    knife.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-0.5f, 0.5f), 1) * 700);
                    knife.GetComponent<Rigidbody2D>().gravityScale = 4;
                    knife.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-300, 300));
                    return;
                }

                knife.transform.SetParent(this.transform);                
                knife.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

                Vibration.Vibrate(50);

                break;
        }
    }
}
