using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LogType
{
    simple,
    boss
}

[CreateAssetMenu(fileName = "WoodLog_lvl_", menuName = "WoodLog")]
public class WoodLog : ScriptableObject
{
    [Header("Имя (для боссов)")]
    public string bossName = "default";

    [Header("Скорость вращения в град/сек")]
    public float speedRotation = 100;

    [Header("Интервал вращения с постоянной скоростью в сек")]
    public float durationRotate = 1f;

    [Header("Продолжительность начала вращения в сек")]
    public float durationStart = 1f;

    [Header("Продолжительность остановки вращения в сек")]
    public float durationStop = 1f;

    [Header("Шанс смены направления вращения после остановки в %")]
    public float chanceChangeDirRot = 0;

    [Header("Шанс спавна яблок в %")]
    public float chanceSpawnApple = 25;

    [Header("Количество яблок")]
    public int numApple = 1;

    [Header("Шанс спавна ножей в %")]
    public float chanceSpawnKnife = 25;

    [Header("Количество ножей на бревне")]
    public int numKnife = 1;

    [Header("Спрайт скина бревна")]
    public Sprite skinLog;

    [Header("Префаб яблока")]
    public GameObject applePrefab;

    [Header("Количество ножей для прохождения")]
    public int knifeCount;

    [Header("Типа бревна")]
    public LogType type = LogType.simple;

    [Header("Скин за победу над боссом")]
    public Sprite knifeSkinReward = null; 
}
