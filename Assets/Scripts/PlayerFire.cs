using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{
    private RotateToMouse rotateToMouse;
    
    public GameObject firePosition;

    public GameObject bombFactory;

    public float throwPower = 30f;

    public GameObject bulletEffect;

    public int attackPower = 6;

    public GameObject[] eff_Flash;

    ParticleSystem ps;

    Animator anim;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        rotateToMouse = GetComponent<RotateToMouse>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ps = bulletEffect.GetComponent<ParticleSystem>();

        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateRotate();

        if(Input.GetMouseButton(0))
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            RaycastHit hitInfo = new RaycastHit();

            if(Physics.Raycast(ray, out hitInfo))
            {
                if(hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                    eFSM.HitEnemy(attackPower);
                }
                bulletEffect.transform.position = hitInfo.point;

                bulletEffect.transform.forward = hitInfo.normal;

                ps.Play();
            }
            StartCoroutine(ShootEffectOn(0.005f));
        }
        IEnumerator ShootEffectOn(float duration)
        {
            int num = Random.Range(0, eff_Flash.Length);

            eff_Flash[num].SetActive(true);

            yield return new WaitForSeconds(duration);

            eff_Flash[num].SetActive(false);
        }

        if(Input.GetMouseButtonDown(1))
        {
            GameObject bomb = Instantiate(bombFactory);
            bomb.transform.position = firePosition.transform.position;

            Rigidbody rb = bomb.GetComponent<Rigidbody>();

            rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
        }
    }
    private void UpdateRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotateToMouse.UpdateRotate(mouseX, mouseY);
    }
}