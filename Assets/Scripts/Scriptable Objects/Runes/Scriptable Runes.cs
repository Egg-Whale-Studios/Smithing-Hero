using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Runes", menuName = "Runes")]
public class ScriptableRunes : ScriptableObject
{
    
}

public class Rune_Data
{
    public ScriptableRunes data;
    public int rune_level;
    
    public Rune_Data(ScriptableRunes rune, int level)
    {
        data = rune;
        rune_level = level;
    }
}
