using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [Header("Referências da UI")]
    public TMP_Text scoreText;
    public TMP_InputField nameInputField;
    public Button saveButton;

    private int finalScore;

    // Chave para a pontuação temporária. Usar uma constante evita erros de digitação.
    private const string TempScoreKey = "TempFinalScore";

    void Start()
    {
        // --- MUDANÇA PRINCIPAL AQUI ---
        // 1. Pega a pontuação dos PlayerPrefs. O segundo parâmetro (0) é o valor
        //    padrão caso a chave não seja encontrada.
        finalScore = PlayerPrefs.GetInt(TempScoreKey, 0);
        // ---------------------------------

        scoreText.text = "SUA PONTUAÇÃO: " + finalScore.ToString();

        // (BOA PRÁTICA) Limpa a chave temporária para não ser usada por engano depois.
        PlayerPrefs.DeleteKey(TempScoreKey);

        // Garante que o cursor do mouse esteja visível e desbloqueado.
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (saveButton != null)
        {
            saveButton.onClick.AddListener(SaveScoreAndReturnToMenu);
        }
        else
        {
            Debug.LogError("O 'Save Button' não foi atribuído no Inspector do GameOverUI!");
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

        // A lógica de salvar no RankingManager continua a mesma.
        // Ele vai pegar a pontuação e adicionar à sua lista permanente.
        if (RankingManager.Instance != null)
        {
            RankingManager.Instance.AddScore(playerName.ToUpper(), finalScore);
        }
        else
        {
            Debug.LogError("RankingManager.Instance não encontrado! A pontuação não foi salva.");
        }

        SceneManager.LoadScene("MENU"); // Certifique-se que o nome da cena está correto.
    }
}