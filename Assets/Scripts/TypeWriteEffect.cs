using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    [Header("Références UI")]
    [SerializeField] private TextMeshProUGUI textComponent;

    [Header("Réglages")]
    [Tooltip("Délai en secondes entre chaque lettre")]
    [SerializeField] private float timeBetweenCharacters = 0.05f;

    private Coroutine typingCoroutine;

    public void ShowText(string fullText)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeTextRoutine(fullText));
    }

    private IEnumerator TypeTextRoutine(string textToType)
    {
        textComponent.text = "";

        foreach (char letter in textToType.ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(timeBetweenCharacters);
        }

        typingCoroutine = null;
    }
}