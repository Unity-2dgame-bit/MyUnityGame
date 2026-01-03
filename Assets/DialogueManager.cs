using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class Dialogue
{
    public string name;

    [TextArea(2, 4)]
    public string sentence;

    public bool isLeft; // 左のキャラが話しているか
}

public class DialogueManager : MonoBehaviour
{
    // 他スクリプトと共有（会話中フラグ）
    public static bool isDialoguePlaying = false;

    [Header("UI")]
    public GameObject dialogueUI;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    [Header("Character Images")]
    public Image leftImage;
    public Image rightImage;

    [Header("Color Settings")]
    public Color activeColor = Color.white;
    public Color inactiveColor = new Color(0.4f, 0.4f, 0.4f, 1f);

    [Header("Dialogue Data")]
    public Dialogue[] dialogues;

    int index = 0;
    Coroutine typingCoroutine;
    bool isTyping = false;

    void Start()
    {
        isDialoguePlaying = true;
        dialogueUI.SetActive(true);
        ShowDialogue();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            OnNextInput();
        }
    }

    void OnNextInput()
    {
        // 文字送り中なら即表示
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = dialogues[index].sentence;
            isTyping = false;
            return;
        }

        // 次のセリフへ
        index++;

        if (index < dialogues.Length)
        {
            ShowDialogue();
        }
        else
        {
            EndDialogue();
        }
    }

    void ShowDialogue()
    {
        Dialogue current = dialogues[index];

        nameText.text = current.name;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeSentence(current.sentence));

        UpdateCharacterBrightness(current.isLeft);
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in sentence)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.03f);
        }

        isTyping = false;
    }

    void UpdateCharacterBrightness(bool leftSpeaking)
    {
        if (leftSpeaking)
        {
            leftImage.color = activeColor;
            rightImage.color = inactiveColor;
        }
        else
        {
            rightImage.color = activeColor;
            leftImage.color = inactiveColor;
        }
    }

    void EndDialogue()
    {
        isDialoguePlaying = false;
        dialogueUI.SetActive(false);

        // 会話終了後の処理
        SceneManager.LoadScene("SampleScene");
    }
}
