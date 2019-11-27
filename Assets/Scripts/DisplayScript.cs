using UnityEngine;
using System.Collections;

public class DisplayScript : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;
    public Camera camera3;

    PlayerManager manager;

    void Start()
    {
        manager = FindObjectOfType<PlayerManager>();
        Screen.SetResolution(1920, 1080, true);
        if(manager != null)
        {
            if (manager.screenlayingDown)
            {
                if (Display.displays.Length > 1)
                Display.displays[1].Activate();
                if (Display.displays.Length > 2)
                Display.displays[2].Activate();
            }
            else
            {
                camera3.targetDisplay = 0;
                Display.displays[0].Activate();
            }
        }
        else
        {

        }
    }
}