using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; 
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    private int currentScore = 0;

    [Header("プレイ中のUI")]
    public TextMeshProUGUI scoreText;

    [Header("リザルト画面のUI")]
    public GameObject resultPanel;          
    public TextMeshProUGUI resultScoreText; 

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        // ゲーム開始時はリザルト画面を非表示にしておく
        if (resultPanel != null) resultPanel.SetActive(false);
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null) scoreText.text = "SCORE: " + currentScore;
    }

    // ゲーム終了時に呼ばれる
    public void ShowResult()
    {
        if (resultPanel != null)
        {
            // リザルト画面を表示
            resultPanel.SetActive(true); 

            if (resultScoreText != null)
            {
                resultScoreText.text = "FINAL SCORE\n" + currentScore;
            }
        }
    }

   
    public void RetryGame()
    {
        // 現在のシーンを最初から読み込み直す
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}