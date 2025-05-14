using UnityEngine;
using UnityEngine.UIElements;

public class CustomSlider : MonoBehaviour
{
    // slider references
    private VisualElement root;
    private VisualElement dragger;
    // fill bar references
    private VisualElement bar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        root= GetComponent<UIDocument>().rootVisualElement;
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
}
