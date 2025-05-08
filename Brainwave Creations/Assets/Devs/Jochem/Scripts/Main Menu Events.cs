using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class MainMenuEvents : MonoBehaviour
{
    private UIDocument UIDocument;
    private Button startButton;

    private void Awake()
    {
        UIDocument = GetComponent<UIDocument>();
        startButton = UIDocument.rootVisualElement.Q("StartGameButton") as Button;
        startButton.RegisterCallback<ClickEvent>(OnPlayGameClickEvent);
    }

    private void OnDisable()
    {
        startButton.UnregisterCallback<ClickEvent>(OnPlayGameClickEvent);
    }

    private void OnPlayGameClickEvent(ClickEvent clickEvent)
    {
        SceneManager.LoadScene(1);
        Debug.Log("pressed the button to start game");
    }
}
