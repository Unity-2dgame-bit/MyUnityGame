using UnityEditor.U2D.Animation;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public CharacterData speaker;
    [TextArea(2, 4)]
    public string text;
    public bool isLeft;
}
