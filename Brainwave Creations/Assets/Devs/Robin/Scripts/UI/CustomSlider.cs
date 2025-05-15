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
    CatapultBehaviour catapultBehaviour;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        catapultBehaviour = GameObject.Find("Player hinge").GetComponent<CatapultBehaviour>();
    }
    void OnEnable()
    {     
        root = GetComponent<UIDocument>().rootVisualElement;
        slider = root.Q<Slider>("PowerSlider");
        slider.highValue = catapultBehaviour.motorForce;
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
        catapultBehaviour.motorForce = slider.value;
        if(slider.value > 0 && Input.GetMouseButtonUp(0))
        {
            catapultBehaviour.playerAimInput = true;
        }
    }
}
