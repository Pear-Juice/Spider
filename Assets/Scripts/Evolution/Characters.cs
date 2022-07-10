using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characters : MonoBehaviour
{
    [System.Serializable]
    public enum Type
    {
        Spider,
        Bat,
        Slime,
        Cat,
        Human,
        Goblin
    }
    
    public Type type;
}