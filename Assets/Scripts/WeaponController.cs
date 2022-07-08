using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class WeaponController : MonoBehaviour
{
    //public float damageAmount;
    public UiManager ui;
    public float minX, maxX;
    public float minY, maxY;
    [Space]
    public Transform camera;
    Vector3 rot;

    public float hitPower = 10;
    public float fireRate = 20f;
    public float force = 8;
    public int magazine = 30, amo, mags = 3;
    public GameObject cameraGameObject;

    ParticleSystem objps;
    ParticleSystem enmps;
    public GameObject objhitEffect;
    public GameObject enemyhitEffect;

    public AudioClip fireClip;
    public AudioClip reloadClip;
    public AudioClip readyClip;
    public AudioClip stepClip;

    public AudioSource stepSource;
    public AudioSource readySource;
    public AudioSource fireSource;
    public AudioSource reloadSource;

    private Animator animations;
    private InputManager inputs;
    public float reloadAnimationTime = 2.5f;
    private float reloadTime = 0f;
    public GameObject[] eff_Flash;
    private float readyToFire;
    private bool isReloading = false;
    private int magazineTamp;
    //private TakeDamage takeDmg;
    private void Start()
    {
        ui = GameObject.FindGameObjectWithTag("UiSystem").GetComponent<UiManager>();
        animations = gameObject.GetComponent<Animator>();
        inputs = gameObject.GetComponent<InputManager>();
        animations.SetInteger("Movement", 0);
        objps = objhitEffect.GetComponent<ParticleSystem>();
        enmps = enemyhitEffect.GetComponent<ParticleSystem>();
        amo = magazine * mags;
        magazineTamp = magazine;
        ui.setAmo(magazine + "/" + amo);
        ui.setMoney("1000");
    }
    
    private void InstantiateAudio(AudioClip clip)
    {
        fireSource = gameObject.AddComponent<AudioSource>();
        fireSource.clip = clip;
        reloadSource = gameObject.AddComponent<AudioSource>();
        reloadSource.clip = clip;
        readySource = gameObject.AddComponent<AudioSource>();
        readySource.clip = clip;
        stepSource = gameObject.AddComponent<AudioSource>();
        stepSource.clip = clip;
    }
    
    public void PlayReadySound()
    {
        readySource.Stop();
        readySource.Play();
    }

    public void PlayReloadSound()
    {
        reloadSource.Stop();
        reloadSource.Play();
    }
    public void PlayStepSound()
    {
        stepSource.Stop();
        stepSource.Play();
    }
    private void Update()
    {
        rot = camera.transform.localRotation.eulerAngles;

        if(rot.x != 0 || rot.y != 0)
        {
            camera.transform.localRotation = Quaternion.Slerp(camera.transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 3);
        }

        if(GameManager.gm.gState != GameManager.GameState.Start)
        {
            return;
        }

        if(Time.time >= readyToFire && !isReloading)
        {
            animations.SetInteger("Fire", -1);
            animations.SetInteger("Movement",(inputs.vertical == 0 && inputs.horizontal == 0)? 0 : 1);
        }

        if(CrossPlatformInputManager.GetButton("Shoot") && Time.time >= readyToFire && !isReloading && magazine > 0)
        {
            readyToFire = Time.time + 1f / fireRate;
            animations.SetInteger("Movement", -1);
            animations.SetInteger("Fire", 2);
            Fire();
        }
        if(CrossPlatformInputManager.GetButtonDown("Reload") && !isReloading && amo > 0)
        {
            if(magazine == magazineTamp)
            {
                isReloading = false;
            }
            else
            {
                isReloading = true;
                reloadTime = reloadAnimationTime;
                animations.SetInteger("Reload", 1);
            }
        }
        if(isReloading && reloadTime <= 1)
        {
            reloadTime = 0f;
            animations.SetInteger("Reload", -1);
            isReloading = false;
            amo = amo - magazineTamp + magazine;
            magazine = magazineTamp;
            if(amo < 0)
            {
                magazine += amo;
                amo = 0;
                ui.setAmo(magazine + "/" + amo);
            }
            ui.setAmo(magazine + "/" + amo);
        }
        else
        {
            reloadTime -= Time.deltaTime;
        }
        
    }
    public void Fire()
    {
        //takeDmg = null;
        readyToFire = Time.time + 1f / fireRate;
        animations.SetInteger("Fire", 2);
        animations.SetInteger("Movement", -1);
        Ray ray = new Ray(cameraGameObject.transform.position, cameraGameObject.transform.forward);
        RaycastHit hit = new RaycastHit();

        magazine -- ;

        ui.setAmo(magazine + "/" + amo);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                EnemyFSM eFSM = hit.transform.GetComponent<EnemyFSM>();

                eFSM.HitEnemy(hitPower);

                enemyhitEffect.transform.position = hit.point;

                enemyhitEffect.transform.forward = hit.normal;

                enmps.Play();
            }
            else
            {
                objhitEffect.transform.position = hit.point;

                objhitEffect.transform.forward = hit.normal;

                objps.Play();
            }
            //checkHIT(hit);
        }
        fireSource.Stop();
        fireSource.Play();
        Recoil();

        StartCoroutine(ShootEffectOn(0.005f));

        IEnumerator ShootEffectOn(float duration)
        {
            int num = Random.Range(0, eff_Flash.Length);

            eff_Flash[num].SetActive(true);

            yield return new WaitForSeconds(duration);

            eff_Flash[num].SetActive(false);
        }
    }
    /*
    private void checkHIT(RaycastHit hit)
    {
        if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            EnemyFSM eFSM = hit.transform.GetComponent<EnemyFSM>();

            eFSM.HitEnemy(hitPower);

            enemyhitEffect.transform.position = hit.point;

            enemyhitEffect.transform.forward = hit.normal;

            enmps.Play();
        }
        else
        {
            objhitEffect.transform.position = hit.point;

            objhitEffect.transform.forward = hit.normal;

            objps.Play();
        }
        try
        {
            takeDmg = hit.transform.GetComponent<TakeDamage>();
            switch(takeDmg.damageType)
            {
                case TakeDamage.collsionType.head:takeDmg.Hit(damageAmount);
                break;
                
                case TakeDamage.collsionType.body:takeDmg.Hit(damageAmount);
                break;

                case TakeDamage.collsionType.Larams:takeDmg.Hit(damageAmount);
                break;

                case TakeDamage.collsionType.Llegs:takeDmg.Hit(damageAmount);
                break;

                case TakeDamage.collsionType.Rarams:takeDmg.Hit(damageAmount);
                break;

                case TakeDamage.collsionType.Rlegs:takeDmg.Hit(damageAmount);
                break;
            }
        }
        catch
        {

        }
        
    }*/
    private void Recoil()
    {
        float recX = Random.Range(minX, maxX);
        float recY = Random.Range(minY, maxY);
        camera.transform.localRotation = Quaternion.Euler(rot.x - recY, rot.y + recX, rot.z);
    }
}
