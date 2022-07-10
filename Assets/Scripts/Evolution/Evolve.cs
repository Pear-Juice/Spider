using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;


public class Evolve : MonoBehaviour
{
    public GameObject evolveUi;
    public static string currentEvo;
    string[] options;
    public void Start()
    {
        currentEvo = File.ReadAllText(@"Assets/Evolutions/CurrentEvo.txt");
        Debug.Log(currentEvo);
    }

    public void InitiateEvolve()
    {
        options = Directory.GetDirectories(currentEvo);
        if (options.Length == 2)
        {
            evolveUi.SetActive(true);
            try { ShowAspects(); }
            catch {}
        }
        else
        {
            try 
            { currentEvo = options[0]; 
            StartCoroutine(FinishEvolve()); 
            } catch { Debug.Log("No options were found at path: " + currentEvo + " finishing."); }
            
        }
    }

    public void ShowAspects()
    {
        options = Directory.GetDirectories(currentEvo);
        TextMeshProUGUI[] aspectText = evolveUi.GetComponentsInChildren<TextMeshProUGUI>();
        string[] optionA = File.ReadAllLines(options[0] + @"\attributes.txt");
        string[] optionB = File.ReadAllLines(options[1] + @"\attributes.txt");

        aspectText[1].text = "";
        aspectText[3].text = "";

        aspectText[1].text += (optionA[0]);
        aspectText[1].text += ("\n" + "Health: +" + optionA[2]);
        aspectText[1].text += ("\n" + "Stamina: +" + optionA[3]);

        aspectText[3].text += (optionB[0]);
        aspectText[3].text += ("\n" + "Health: +" + optionB[2]);
        aspectText[3].text += ("\n" + "Stamina: +" + optionB[3]);
    }
    public void ChooseEvolve(bool _decision)
    {
        var character = this.GetComponent<Characters>();
        var attributes = this.GetComponent<Attributes>();
        attributes.aspects.level ++;
        
        if (!_decision)
            currentEvo = options[0];
        else
            currentEvo = options[1];

        StartCoroutine(FinishEvolve());
    }

    IEnumerator FinishEvolve()
    {
        var attributes = this.GetComponent<Attributes>();

        yield return new WaitForSeconds(0.01f);
        attributes.EvolveAspects();
        //File.WriteAllText(@"Assets/Evolutions/CurrentEvo.txt",currentEvo);
        Debug.Log(currentEvo);
        evolveUi.SetActive(false);
    }
}
