using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct stage
{
    public int CurrentStage; // 현재 스테이지
    public int EnemyCount; // A 에너미 죽이면 다음 스테이지 가는지
    //public int BEnemyCount; // B 에너미 죽이면 다음 스테이지 가는지 (후에 구현)
}
[CreateAssetMenu(menuName = "SO/Stage")]
public class StageSO : ScriptableObject
{
    public List<stage> Stage;
}
