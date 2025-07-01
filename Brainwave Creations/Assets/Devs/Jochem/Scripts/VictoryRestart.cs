using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class VictoryRestart : MonoBehaviour
{
    private UIDocument UIDocument;
    private Button restartButton;
    private Button exitButton;


    private void Awake()
    {
        UIDocument = GetComponent<UIDocument>();
        restartButton = UIDocument.rootVisualElement.Q("RestartButton") as Button;
        restartButton.RegisterCallback<ClickEvent>(OnrestartClickEvent);
        exitButton = UIDocument.rootVisualElement.Q("ExitButton") as Button;
        exitButton.RegisterCallback<ClickEvent>(OnExitClickEvent);
    }

    private void OnDisable()
    {
        restartButton.UnregisterCallback<ClickEvent>(OnrestartClickEvent);
        exitButton.UnregisterCallback<ClickEvent>(OnExitClickEvent);
    }

    private void OnrestartClickEvent(ClickEvent clickEvent)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnExitClickEvent(ClickEvent clickEvent) 
    {
        SceneManager.LoadScene("main menu");    
    }

}
