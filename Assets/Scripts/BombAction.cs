using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    public GameObject bombEffect;

    public int attackPower = 100;

    public float explosionRadius = 10f;

    private void OnCollisionEnter(Collision other) 
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 10);

        for(int i = 0; i < cols.Length; i++)
        {
            cols[i].GetComponent<EnemyFSM>().HitEnemy(attackPower);
        }
        GameObject eff = Instantiate(bombEffect);

        eff.transform.position = transform.position;
        
        Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
