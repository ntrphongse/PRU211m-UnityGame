using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] Toggle music;
    [SerializeField] Dropdown difficulty;

    void Awake()
    {
        switch (AudioListener.volume)
        {
            case 0:
                music.isOn = false;
                break;
            case 1:
                music.isOn = true;
                break;
        }
        this.difficulty.value = GlobalVariables.difficulty;
    }

    public void MuteToggle(bool muted)
    {
        if (!muted)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }
    }
}
