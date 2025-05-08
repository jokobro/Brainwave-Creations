using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class GameOverEvents : MonoBehaviour
{
    private UIDocument UIDocument;
    private Button restartButton;

    private void Awake()
    {
        restartButton = UIDocument.rootVisualElement.Q("RestartButton") as Button;
        restartButton.RegisterCallback<ClickEvent>(OnrestartClickEvent);
    }

    private void OnDisable()
    {
        restartButton.UnregisterCallback<ClickEvent>(OnrestartClickEvent);
    }

    private void OnrestartClickEvent(ClickEvent clickEvent)
    {
        SceneManager.LoadScene(1);
    }
}
