using UnityEngine;
using UnityEngine.UIElements;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private float score = 10000;
    private float decreaseRate = 10f;

    [SerializeField] private float timer;
    private float increaseTime = 1f;
    private void Awake()
    {
        instance = this;
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
    }

    public void AddScore(int pointsAmount)
    {
       score += Mathf.RoundToInt(pointsAmount);
    }
}
