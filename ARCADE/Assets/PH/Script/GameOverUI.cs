using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [Header("Refer�ncias da UI")]
    public TMP_Text scoreText;
    public TMP_InputField nameInputField;
    public Button saveButton;

    private int finalScore;

    // Chave para a pontua��o tempor�ria. Usar uma constante evita erros de digita��o.
    private const string TempScoreKey = "TempFinalScore";

    void Start()
    {
        // --- MUDAN�A PRINCIPAL AQUI ---
        // 1. Pega a pontua��o dos PlayerPrefs. O segundo par�metro (0) � o valor
        //    padr�o caso a chave n�o seja encontrada.
        finalScore = PlayerPrefs.GetInt(TempScoreKey, 0);
        // ---------------------------------

        scoreText.text = "SUA PONTUA��O: " + finalScore.ToString();

        // (BOA PR�TICA) Limpa a chave tempor�ria para n�o ser usada por engano depois.
        PlayerPrefs.DeleteKey(TempScoreKey);

        // Garante que o cursor do mouse esteja vis�vel e desbloqueado.
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (saveButton != null)
        {
            saveButton.onClick.AddListener(SaveScoreAndReturnToMenu);
        }
        else
        {
            Debug.LogError("O 'Save Button' n�o foi atribu�do no Inspector do GameOverUI!");
        }

        // Foca no campo de texto para o jogador poder digitar o nome imediatamente.
        if (nameInputField != null)
        {
            nameInputField.Select();
            nameInputField.ActivateInputField();
        }
    }

    public void SaveScoreAndReturnToMenu()
    {
        string playerName = nameInputField.text;

        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "JOGADOR";
        }

        // A l�gica de salvar no RankingManager continua a mesma.
        // Ele vai pegar a pontua��o e adicionar � sua lista permanente.
        if (RankingManager.Instance != null)
        {
            RankingManager.Instance.AddScore(playerName.ToUpper(), finalScore);
        }
        else
        {
            Debug.LogError("RankingManager.Instance n�o encontrado! A pontua��o n�o foi salva.");
        }

        SceneManager.LoadScene("MENU"); // Certifique-se que o nome da cena est� correto.
    }
}