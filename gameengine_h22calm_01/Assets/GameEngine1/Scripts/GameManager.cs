using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;  // Scene 관리용!

public class GameManager : MonoBehaviour
{
    [Header("UI 참조")]
    public GameObject titleScreenPanel;
	public GameObject hudPanel;
    public GameObject gameOverPanel;  // Game Over 패널 추가!
    public GameObject gameClearPanel;  // Game Clear 패널!
   
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;  // 시간 텍스트 추가
    public TextMeshProUGUI healthText;  // Health 텍스트 추가!
    public TextMeshProUGUI finalScoreText;  // 최종 점수 텍스트!
    [Header("게임 상태")]
    private int score = 0;
    private float playTime = 0f;  // 게임 진행 시간
    private bool isPlaying = false;  // 게임 진행 중인지
    private int health = 3;  // 생명 추가! 기본값 3

    public TextMeshProUGUI clearScoreText;  // 클리어 점수!
	public TextMeshProUGUI clearTimeText;  // 클리어 시간!
	
    void Start()
    {
        ShowTitleScreen();
        UpdateScoreUI();
        UpdateTimeUI();
        UpdateHealthUI();  // Health UI 업데이트 추가!
    }
    
    void Update()
    {
        // 게임 진행 중일 때만 시간 증가
        if (isPlaying)
        {
            playTime += Time.deltaTime;
            UpdateTimeUI();
        }
    }
    
    void ShowTitleScreen()
    {
        titleScreenPanel.SetActive(true);
        gameOverPanel.SetActive(false);  // Game Over 숨기기!
        gameClearPanel.SetActive(false);  // Game Clear 숨기기!
        Time.timeScale = 0f;
        isPlaying = false;
    }
    
    public void StartGame()
    {
        titleScreenPanel.SetActive(false);
        hudPanel.SetActive(true);
        gameOverPanel.SetActive(false);  // Game Over 숨기기!
        gameClearPanel.SetActive(false);  // Game Clear 숨기기!
        Time.timeScale = 1f;
        score = 0;
        playTime = 0f;  // 시간 초기화
        isPlaying = true;  // 게임 시작
        health = 3;
        UpdateScoreUI();
        UpdateTimeUI();
        UpdateHealthUI();  // Health UI 업데이트!
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }
    
    	// 생명 감소 함수 - 새로 추가!
	public void TakeDamage(int damage)
	{
		health -= damage;
		UpdateHealthUI();
		if (health <= 0)
		{
			GameOver();  // 생명이 0 이하면 게임 오버!
		}
	}
    
    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    // 시간 UI 업데이트
    void UpdateTimeUI()
    {
        if (timeText != null)
        {
            // 시간을 분:초 형식으로 변환
            int minutes = Mathf.FloorToInt(playTime / 60f);
            int seconds = Mathf.FloorToInt(playTime % 60f);

            // 00:00 형식으로 표시
            timeText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
        }
    }
    // Health UI 업데이트 함수 - 새로 추가!
	void UpdateHealthUI()
	{
		if (healthText != null)
		{
		healthText.text = "Health: " + health;
		}
	}

    // Game Over 함수 - 새로 추가!
    void GameOver()
    {
        Debug.Log("💀 Game Over!");
        isPlaying = false;
        Time.timeScale = 0f;
        // Game Over 화면 표시
        hudPanel.SetActive(false);  // HUD 숨기기
        gameOverPanel.SetActive(true);  // Game Over 패널 표시
                                        // 최종 점수 표시
        if (finalScoreText != null)
        {
            finalScoreText.text = "Final Score: " + score;
        }
    }
    
    // Retry 버튼 함수 - 새로 추가!
	public void RetryGame()
	{
		Time.timeScale = 1f;  // 시간 재개
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // 현재 씬 재시작
	}

    // Quit 버튼 함수 - 새로 추가!
    public void QuitGame()
    {
        Debug.Log("게임 종료");
        Application.Quit();  // 빌드된 게임 종료
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // 에디터에서 종료
#endif
    }

    // Game Clear 함수 - 새로 추가!
    public void GameClear()
    {
        Debug.Log("🎉🎉🎉 Game Clear! 🎉🎉🎉");
        isPlaying = false;
        Time.timeScale = 0f;
        // Game Clear 화면 표시
        hudPanel.SetActive(false);
        gameClearPanel.SetActive(true);
        // 최종 점수 및 시간 표시
        if (clearScoreText != null)
        {
            clearScoreText.text = "Score: " + score;
        }
        if (clearTimeText != null)
        {
            int minutes = Mathf.FloorToInt(playTime / 60f);
            int seconds = Mathf.FloorToInt(playTime % 60f);
            clearTimeText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
        }
    }
}