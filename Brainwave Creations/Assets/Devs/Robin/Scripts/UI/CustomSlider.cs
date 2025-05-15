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
    CatapultBehaviour catapultBehaviourPlayer;
    CatapultBehaviourBomb catapultBehaviourBomb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        catapultBehaviourPlayer = GameObject.Find("Player hinge").GetComponent<CatapultBehaviour>();
        catapultBehaviourBomb = GameObject.Find("Bomb catapult").GetComponentInChildren<CatapultBehaviourBomb>();
    }
    void OnEnable()
    {     
        root = GetComponent<UIDocument>().rootVisualElement;
        slider = root.Q<Slider>("PowerSlider");
        slider.highValue = catapultBehaviourBomb.motorForce;
        slider.highValue = catapultBehaviourPlayer.motorForce;
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
        catapultBehaviourPlayer.motorForce = slider.value;
        catapultBehaviourBomb.motorForce = slider.value;
        if(slider.value > 0 && Input.GetMouseButtonUp(0))
        {
            catapultBehaviourBomb.playerAimInput = true;
            catapultBehaviourPlayer.playerAimInput = true;
        }

    }
}
