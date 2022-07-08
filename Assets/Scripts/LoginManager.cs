using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public InputField id;
    public InputField password;
    public Text notify;
    private void Start() 
    {
        notify.text = "";
    }
    public void SaveUserData()
    {
        if(CheckInputField(id.text, password.text))
        {
            notify.text = "아이디 또는 패스워드를 입력해주세요.";
            return;
        }
        if(!PlayerPrefs.HasKey(id.text))
        {
            PlayerPrefs.SetString(id.text, password.text);
            notify.text = "아이디 생성이 완료됐습니다.";
        }
        else
        {
            notify.text = "이미 존재하는 아이디 입니다.";
        }
    }
    public void CheckUserData()
    {
        if(CheckInputField(id.text, password.text))
        {
            notify.text = "아이디 또는 패스워드를 입력해주세요.";
            return;
        }
        string pass = PlayerPrefs.GetString(id.text);

        if(pass == password.text)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            notify.text = "아이디 또는 패스워드가 일치하지 않습니다.";
        }
    }
    bool CheckInputField(string id, string pwd)
    {
        if(id == "" || pwd == "")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
}
