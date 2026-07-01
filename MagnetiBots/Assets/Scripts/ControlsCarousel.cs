using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class ControlsCarousel : MonoBehaviour
{

    public VisualElement container;
    VisualElement[] controlsCarousel;
    private Button buttonLeft;
    private Button buttonRight;
    int index;
    public void Startup()
    {
        VisualElement dangit = container.Children().ToArray()[0];
        Debug.Log(dangit.name);
        controlsCarousel = dangit.Children().ToArray().Take(5).ToArray();
        Debug.Log(controlsCarousel.Length);
        buttonLeft = container.Q("ControlsButtonLeft") as Button;
        buttonRight = container.Q("ControlsButtonRight") as Button;
        buttonLeft.RegisterCallback<ClickEvent>(OnClickLeftButton);
        buttonRight.RegisterCallback<ClickEvent>(OnClickRightButton);
    }
    public void Ready()
    {
        foreach (var control in controlsCarousel)
        {
            control.visible = false;
        }
        controlsCarousel[0].visible = true;
        index = 0;
    }
    public void UnReady()
    {
        foreach (var control in controlsCarousel)
        {
            control.visible = false;
        }
        controlsCarousel[index%controlsCarousel.Length].visible = false;
        index = 0;
    }
    private void OnClickRightButton(ClickEvent click)
    {
        index++;
        int realindex = index % controlsCarousel.Length;
        int previousindex = (index-1) % controlsCarousel.Length;
        controlsCarousel[previousindex].visible = false;
        controlsCarousel[realindex].visible = true;
    }
    private void OnClickLeftButton(ClickEvent click)
    {
        index--;
        int realindex = index % controlsCarousel.Length;
        int previousindex = (index + 1) % controlsCarousel.Length;
        controlsCarousel[previousindex].visible = false;
        controlsCarousel[realindex].visible = true;
    }
}
