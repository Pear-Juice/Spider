using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public GameObject home;
    public GameObject classSelection;
    bool isFirstTime = true;

    public void Start()
    {
        if (PlayerPrefs.GetInt("firstTimePlaying") == 0)
        {
            Debug.Log("is first time");
            PlayerPrefs.SetFloat("firstTimePlaying",1);
        }
        else
        {
            isFirstTime = false;
            Debug.Log("Is not first time");
        }
    }

    public void play()
    {
        home.SetActive(false);
        if (isFirstTime)
        {
            classSelection.SetActive(true);
        }
    }
}
