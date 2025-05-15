using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UIElements;

public class CustomSlider : MonoBehaviour
{
    // slider references
    private VisualElement root;
    private VisualElement dragger;
    private Slider slider;
    // fill bar references
    private VisualElement bar;

    //Catapult reference
    private PlayerController playerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        playerController = FindAnyObjectByType<PlayerController>();
    }
    void OnEnable()
    {     
        root = GetComponent<UIDocument>().rootVisualElement;
        slider = root.Q<Slider>("PowerSlider");
        slider.highValue = playerController.cataPultBehaviour.motorForce;
        dragger = root.Q<VisualElement>("unity-dragger");
             
        AddBarElements();
    }

    private void AddBarElements()
    {
        bar = new VisualElement();
        dragger.Add(bar);
        bar.name = "Fill bar";
        bar.AddToClassList("Bar");
    }

    private void Update()
    {
        playerController.cataPultBehaviour.motorForce = slider.value;
        if (slider.value > 0 && Input.GetMouseButtonUp(0))
        {
            playerController.cataPultBehaviour.playerAimInput = true;
        }
    }
}
