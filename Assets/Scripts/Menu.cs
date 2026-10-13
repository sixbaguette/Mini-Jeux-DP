using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject panelStart;

    [SerializeField] private TypewriterEffect typewriter;
    private string message;

    private void Start()
    {
        message = "Votre bute et de récupéré le plus de pièce possible pour cela il vous suffit juste de frapper les monstre vous pouvez également courir avec shift, les monstre vous donne des pièce une fois mort, différent monstre sont présent et possède des capacité différente, enfin les petit point jaune sur votre écran vous montre des tour d'ascencion qui au sommet vont pouvoir vous permettre de récupéré des bonus de grande valeur qui vous aiderons a récupéré le cota de pièce impartie. \r\nBonne chance !";
    }

    public void StartButton()
    {
        StartCoroutine(LaunchGame());
    }

    private IEnumerator LaunchGame()
    {
        panelStart.SetActive(true);

        typewriter.ShowText(message);

        yield return new WaitForSeconds(30);

        SceneManager.LoadScene("SampleScene");
    }
}
