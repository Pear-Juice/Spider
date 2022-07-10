using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;

public class Attributes : MonoBehaviour
{
    public void UpdateAspects()
    {
        string[] att = new string[0];
        att = File.ReadAllLines(Evolve.currentEvo + @"\attributes.txt");

        //set variables
        aspects.name = att[0];
        aspects.age = Int32.Parse(att[1]);
        aspects.health = Int32.Parse(att[2]);
        aspects.stamina = Int32.Parse(att[3]);
        aspects.level = Int32.Parse(att[4]);

        Debug.Log("Updated aspects");
    }

    public void EvolveAspects()
    {
        string[] att = new string[0];
        att = File.ReadAllLines(Evolve.currentEvo + @"\attributes.txt");

        //set variables
        aspects.name = att[0];
        aspects.age = Int32.Parse(att[1]);
        aspects.health += Int32.Parse(att[2]);
        aspects.stamina += Int32.Parse(att[3]);
        aspects.level = Int32.Parse(att[4]);

        Debug.Log("Updated aspects");
    }
    public Aspects aspects;

    [System.Serializable]
    public struct Aspects
    {
        public string name;
        public int age;
        public int health;
        public float stamina;
        public int level;
    }
}
