using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerJoinTextHandler : MonoBehaviour
{
    //TODO: Removed so much unnecessary commented/unused code
    public GameObject Player1Text;
    public GameObject Player2Text;

    public GameObject Screen3Player1Text;
    public GameObject Screen3Player2Text;
    public GameObject Screen3Logo;

    public void GetPlayers(bool pl1, bool pl2,bool screenModeDown)
    {
        Player1Text.SetActive(!pl1 && screenModeDown);
        Screen3Player1Text.SetActive(!pl1 && !screenModeDown);
        Player2Text.SetActive(!pl2 && screenModeDown);
        Screen3Player2Text.SetActive(!pl2 && !screenModeDown);
        Screen3Logo.SetActive(!(pl1 || pl2) && !screenModeDown);
    }

}
