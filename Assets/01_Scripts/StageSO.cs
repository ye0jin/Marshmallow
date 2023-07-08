using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct stage
{
    public int CurrentStage; // ���� ��������
    public int EnemyCount; // A ���ʹ� ���̸� ���� �������� ������
    //public int BEnemyCount; // B ���ʹ� ���̸� ���� �������� ������ (�Ŀ� ����)
}
[CreateAssetMenu(menuName = "SO/Stage")]
public class StageSO : ScriptableObject
{
    public List<stage> Stage;
}
