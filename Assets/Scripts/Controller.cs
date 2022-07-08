using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Controller : MonoBehaviour
{
    private InputManager inputManager;
    private CharacterController cc;
    public GameObject camObject;
    public TextMeshProUGUI countfS;
    public TextMeshProUGUI countS;
    public float hp = 100;

    public float maxHp = 100;

    public Slider hpSlider;

    public GameObject hitEffect;
    public float jumpForce = 200f, movementSpeed = 12, gravityForce = -9.81f;
    //[Range(0.1f, 5f)]
    public float mouseSensitivity;
    public float fireSensitivity;

    private Vector3 movementVector, gravity;

    // 배고픔
    [SerializeField]
    private int hungry;
    private int currentHungry;

    // 배고픔이 줄어드는 속도.
    [SerializeField]
    private int hungryDecreaseTime;
    private int currentHungryDecreaseTime;

    // 목마름.
    [SerializeField]
    private int thirsty;
    private int currentThirsty;

    // 목마름이 줄어드는 속도.
    [SerializeField]
    private int thirstyDecreaseTime;
    private int currentThirstyDecreaseTime;

    private const int HP = 0, HUNGRY = 1, THIRSTY = 2;

    public void changeSensitivity(float _sensitivity)
    {
        mouseSensitivity = _sensitivity;
    }
    public void changeFireSensitivity(float _sensitivity)
    {
        fireSensitivity = _sensitivity;
    }

    private void Start()
    {
        inputManager = GetComponent<InputManager>();
        cc = GetComponent<CharacterController>();
        currentHungry = hungry;
        currentThirsty = thirsty;
    }
    private void FixedUpdate()
    {
        if (GameManager.gm.gState != GameManager.GameState.Start)
        {
            return;
        }
        movementVector = transform.right * inputManager.horizontal + transform.forward * inputManager.vertical;

        cc.Move(movementVector * movementSpeed * Time.deltaTime);

        if (IsGrounded() && gravity.y < 0)
        {
            gravity.y = -2;
        }
        gravity.y += gravityForce * Time.deltaTime;
        cc.Move(gravity * Time.deltaTime);

        transform.localRotation *= Quaternion.Euler(0f, inputManager.yValue * fireSensitivity, 0f);

        if (inputManager.xValue > 0 && camObject.transform.localRotation.x > -0.7f)
        {
            camObject.transform.localRotation *= Quaternion.Euler(-inputManager.xValue * fireSensitivity, 0f, 0f);
        }
        if (inputManager.xValue < 0 && camObject.transform.localRotation.x < 0.7f)
        {
            camObject.transform.localRotation *= Quaternion.Euler(-inputManager.xValue * fireSensitivity, 0f, 0f);
        }

        hpSlider.value = (float)hp / (float)maxHp;
    }
    void Update()
    {
        countfS.text = fireSensitivity.ToString("N1");
        countS.text = mouseSensitivity.ToString("N1");
        if (GameManager.gm.gState != GameManager.GameState.Start)
        {
            return;
        }

        float mouseX = 0;
        float mouseY = 0;

        if (Touchscreen.current.touches.Count == 0)
            return;

        if (EventSystem.current.IsPointerOverGameObject(Touchscreen.current.touches[0].touchId.ReadValue()))
        {
            if (Touchscreen.current.touches.Count > 1 && Touchscreen.current.touches[1].isInProgress)
            {
                if (EventSystem.current.IsPointerOverGameObject(Touchscreen.current.touches[1].touchId.ReadValue()))
                    return;

                Vector2 touchDeltaPosition = Touchscreen.current.touches[1].delta.ReadValue();
                mouseX = touchDeltaPosition.x;
                mouseY = touchDeltaPosition.y;
            }
        }
        else
        {
            if (Touchscreen.current.touches.Count > 0 && Touchscreen.current.touches[0].isInProgress)
            {
                if (EventSystem.current.IsPointerOverGameObject(Touchscreen.current.touches[0].touchId.ReadValue()))
                    return;

                Vector2 touchDeltaPosition = Touchscreen.current.touches[0].delta.ReadValue();
                mouseX = touchDeltaPosition.x;
                mouseY = touchDeltaPosition.y;
            }

        }

        mouseX *= mouseSensitivity;
        mouseY *= mouseSensitivity;

        transform.localRotation *= Quaternion.Euler(0f, mouseX * mouseSensitivity, 0f);

        if (mouseY > 0 && camObject.transform.localRotation.x > -0.7f)
        {
            camObject.transform.localRotation *= Quaternion.Euler(-mouseY * mouseSensitivity, 0f, 0f);
        }
        if (mouseY < 0 && camObject.transform.localRotation.x < 0.7f)
        {
            camObject.transform.localRotation *= Quaternion.Euler(-mouseY * mouseSensitivity, 0f, 0f);
        }

        Hungry();
        Thirsty();
    }
    private void Hungry()
    {
        if (currentHungry > 0)
        {
            if (currentHungryDecreaseTime <= hungryDecreaseTime)
                currentHungryDecreaseTime++;
            else
            {
                currentHungry--;
                currentHungryDecreaseTime = 0;
            }
        }
        /*else
            Debug.Log("배고픔 수치가 0이 되었습니다");*/
    }
    private void Thirsty()
    {
        if (currentThirsty > 0)
        {
            if (currentThirstyDecreaseTime <= thirstyDecreaseTime)
                currentThirstyDecreaseTime++;
            else
            {
                currentThirsty--;
                currentThirstyDecreaseTime = 0;
            }
        }
        /*else
            Debug.Log("목마름 수치가 0이 되었습니다");*/
    }
    public void IncreaseHP(int _count)
    {
        if (hp + _count < hp)
        {
            hp += _count;
        }
        else
        {
            hp = maxHp;
        }
    }
    public void IncreaseHungry(int _count)
    {
        if (currentHungry + _count < hungry)
            currentHungry += _count;
        else
            currentHungry = hungry;
    }

    public void DecreaseHungry(int _count)
    {
        if (currentHungry - _count < 0)
            currentHungry = 0;
        else
            currentHungry -= _count;
    }

    public void IncreaseThirsty(int _count)
    {
        if (currentThirsty + _count < thirsty)
            currentThirsty += _count;
        else
            currentThirsty = thirsty;
    }

    public void DecreaseThirsty(int _count)
    {
        if (currentThirsty - _count < 0)
            currentThirsty = 0;
        else
            currentThirsty -= _count;
    }

    public void DamageAction(int damage)
    {
        hp -= damage;

        if (hp > 0)
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
    public void Jump()
    {
        if (IsGrounded())
        {
            gravity.y = Mathf.Sqrt(jumpForce * -2 * gravityForce);
        }
    }
    private bool IsGrounded()
    {
        RaycastHit raycastHit;
        if (Physics.SphereCast(transform.position, cc.radius * (1.0f - 0), Vector3.down, out raycastHit, ((cc.height / 2f) - cc.radius) + 0.15f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
