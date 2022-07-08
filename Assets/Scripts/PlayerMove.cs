using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    CharacterController cc;

    public int hp = 100;

    int maxHp = 100;

    public Slider hpSlider;

    public GameObject hitEffect;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        hpSlider.value = (float)hp / (float)maxHp;
    }
    public void DamageAction(int damage)
    {
        hp -= damage;

        if(hp > 0)
        {
            StartCoroutine(PlayHitEffect());
        }
    }
    IEnumerator PlayHitEffect()
    {
        hitEffect.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        hitEffect.SetActive(false);
    }
}
