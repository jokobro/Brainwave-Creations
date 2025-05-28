using UnityEngine;
using UnityEngine.UIElements;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private float score = 10000;
    private float decreaseRate = 10f;
    private float timer = 0f;
    private float increaseTime = 1f;
    private void Awake()
    {
        instance = this;
        score = Mathf.Round(score * 10.0f);
    }

    private void Update()
    {
        UpdateUI();
        
        if(score > 0)
        {
            score -= decreaseRate * Time.deltaTime;
        }

        timer += increaseTime * Time.deltaTime;
    }

    public void UpdateUI()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        Label scoreText = root.Q<Label>("ScoreText");
        Label timeText = root.Q<Label>("TimeText");
        scoreText.text = ($"{score}");
        timeText.text = ($"{timer}");
        timeText.text = $"{Mathf.Round(timer * 100f) / 100f}";
    }

    public void AddScore(int pointsAmount)
    {
       score += Mathf.RoundToInt(pointsAmount);
    }
}
