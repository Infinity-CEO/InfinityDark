using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public Transform[] m_SpawnPoints;
    public GameObject m_EnemyPrefab;

    private void Start()
    {
        SpawnNewEnemy();
    }
    void OnEnable()
    {
        EnemyFSM.OnEnemyKilled += SpawnNewEnemy;
    }
    void SpawnNewEnemy()
    {

        int randomNumber = Mathf.RoundToInt(Random.Range(0f, m_SpawnPoints.Length - 1));
        Instantiate(m_EnemyPrefab, m_SpawnPoints[randomNumber].transform.position, Quaternion.identity);
    }
}
