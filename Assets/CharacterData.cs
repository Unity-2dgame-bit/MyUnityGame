using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Character")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public Sprite defaultSprite;
    public Color inactiveColor = new Color(0.5f, 0.5f, 0.5f, 1f);
}
