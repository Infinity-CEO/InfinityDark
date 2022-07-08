using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEvent : MonoBehaviour
{
    public EnemyFSM efsm;
    //audio
    public AudioClip attackClip;
    public AudioClip growlClip;
    public AudioClip growl2Clip;
    public AudioClip moveClip;
    public AudioClip move2Clip;

    public AudioSource attackSource;
    public AudioSource growlSource;
    public AudioSource growl2Source;
    public AudioSource moveSource;
    public AudioSource move2Source;

    public void PlayerHit()
    {
        efsm.AttackAction();
    }
    private void InstantiateAudio(AudioClip clip)
    {
        attackSource = gameObject.AddComponent<AudioSource>();
        attackSource.clip = clip;
        growlSource = gameObject.AddComponent<AudioSource>();
        growlSource.clip = clip;
        growl2Source = gameObject.AddComponent<AudioSource>();
        growl2Source.clip = clip;
        moveSource = gameObject.AddComponent<AudioSource>();
        moveSource.clip = clip;
        move2Source = gameObject.AddComponent<AudioSource>();
        move2Source.clip = clip;
        /*bloodSource.clip = clip;
        damageSource = gameObject.AddComponent<AudioSource>();
        damageSource.clip = clip;
        stepSource = gameObject.AddComponent<AudioSource>();
        stepSource.clip = clip;*/
    }
    public void playmove()
    {
        moveSource.Stop();
        moveSource.Play();
    }
    public void playmove2()
    {
        if (GameManager.gm.gState != GameManager.GameState.Start)
        {
            return;
        }
        move2Source.Stop();
        move2Source.Play();
    }
    public void playattack()
    {
        if (GameManager.gm.gState != GameManager.GameState.Start)
        {
            return;
        }
        attackSource.Stop();
        attackSource.Play();
    }
    public void playgrowl()
    {
        if (GameManager.gm.gState != GameManager.GameState.Start)
        {
            return;
        }
        growlSource.Stop();
        growlSource.Play();
    }
    public void playgrowl2()
    {
        if (GameManager.gm.gState != GameManager.GameState.Start)
        {
            return;
        }
        growl2Source.Stop();
        growl2Source.Play();
    }
    /*public void playdamage()
    {
        damageSource.Stop();
        damageSource.Play();
    }
    public void playstep()
    {
        stepSource.Stop();
        stepSource.Play();
    }*/
}
