using UnityEngine;

public class ButtonBehaviourTutorial : MonoBehaviour
{
   public void Tutorial()
   {
     if (gameObject.activeSelf)
     {
       gameObject.SetActive(false);
     }
     else
     {
       gameObject.SetActive(true);
     }
   }
}
