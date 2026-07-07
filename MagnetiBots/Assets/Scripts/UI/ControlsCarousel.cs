using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
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
        List<VisualElement>carouselList = dangit.Children().ToArray().Take(5).ToList();
        carouselList.RemoveAt(0);
        controlsCarousel = carouselList.ToArray();
        Debug.Log(controlsCarousel.Length);
        buttonLeft = container.Q("ControlsButtonLeft") as Button;
        buttonRight = container.Q("ControlsButtonRight") as Button;
        buttonLeft.RegisterCallback<ClickEvent>(OnClickLeftButton);
        buttonRight.RegisterCallback<ClickEvent>(OnClickRightButton);
        controlsCarousel[0].AddToClassList("middle");
        controlsCarousel[1].AddToClassList("right");
        controlsCarousel[controlsCarousel.Count() - 1].AddToClassList("left");
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
        StartCoroutine(OnDoRightButton());
    }
    private void OnClickLeftButton(ClickEvent click)
    {
        StartCoroutine(OnDoLeftButton());
    }
    private IEnumerator OnDoRightButton()
    {
        index++;

        int realindex = index % controlsCarousel.Length;
        if (realindex < 0) { realindex += controlsCarousel.Length; }
        Debug.Log(realindex);

        int previousindex = (index - 1) % controlsCarousel.Length;
        if (previousindex < 0) { previousindex += controlsCarousel.Length; }

        int nextindex = (index + 1) % controlsCarousel.Length;
        if (nextindex < 0) { nextindex += controlsCarousel.Length; }

        int leftedge = (index - 2) % controlsCarousel.Length;
        if (leftedge < 0) { leftedge += controlsCarousel.Length; }

        int rightedge = (index + 2) % controlsCarousel.Length;
        if (rightedge < 0) { rightedge += controlsCarousel.Length; }

        controlsCarousel[leftedge].visible = false;
        controlsCarousel[previousindex].visible = true;
        controlsCarousel[realindex].visible = true;
        controlsCarousel[nextindex].visible = false;
        controlsCarousel[rightedge].visible = false;

        yield return new WaitForSeconds(0.3f);

        controlsCarousel[leftedge].AddToClassList("right");
        controlsCarousel[leftedge].RemoveFromClassList("left");
        controlsCarousel[leftedge].RemoveFromClassList("middle");

        controlsCarousel[previousindex].AddToClassList("left");
        controlsCarousel[previousindex].RemoveFromClassList("middle");
        controlsCarousel[previousindex].RemoveFromClassList("right");

        controlsCarousel[realindex].AddToClassList("middle");
        controlsCarousel[realindex].RemoveFromClassList("right");
        controlsCarousel[realindex].RemoveFromClassList("left");

        controlsCarousel[nextindex].AddToClassList("right");
        controlsCarousel[nextindex].RemoveFromClassList("middle");
        controlsCarousel[nextindex].RemoveFromClassList("left");

        controlsCarousel[rightedge].AddToClassList("left");
        controlsCarousel[rightedge].RemoveFromClassList("right");
        controlsCarousel[rightedge].RemoveFromClassList("middle");

        Globals.Managers.Audio.PlaySFX("UI_Up");
    }
    private IEnumerator OnDoLeftButton()
    {
        index--;
        int realindex = index % controlsCarousel.Length;
        if (realindex < 0) { realindex += controlsCarousel.Length; }
        Debug.Log(realindex);

        int previousindex = (index + 1) % controlsCarousel.Length;
        if (previousindex < 0) { previousindex += controlsCarousel.Length; }

        int nextindex = (index - 1) % controlsCarousel.Length;
        if (nextindex < 0) { nextindex += controlsCarousel.Length; }

        int leftedge = (index - 2) % controlsCarousel.Length;
        if (leftedge < 0) { leftedge += controlsCarousel.Length; }

        int rightedge = (index + 2) % controlsCarousel.Length;
        if (rightedge < 0) { rightedge += controlsCarousel.Length; }

        controlsCarousel[leftedge].visible = false;
        controlsCarousel[previousindex].visible = true;
        controlsCarousel[realindex].visible = true;
        controlsCarousel[nextindex].visible = false;
        controlsCarousel[rightedge].visible = false;

        yield return new WaitForSeconds(0.3f);

        controlsCarousel[leftedge].AddToClassList("right");
        controlsCarousel[leftedge].RemoveFromClassList("left");
        controlsCarousel[leftedge].RemoveFromClassList("middle");

        controlsCarousel[nextindex].AddToClassList("left");
        controlsCarousel[nextindex].RemoveFromClassList("right");
        controlsCarousel[nextindex].RemoveFromClassList("middle");

        controlsCarousel[realindex].AddToClassList("middle");
        controlsCarousel[realindex].RemoveFromClassList("left");
        controlsCarousel[realindex].RemoveFromClassList("right");

        controlsCarousel[previousindex].AddToClassList("right");
        controlsCarousel[previousindex].RemoveFromClassList("middle");
        controlsCarousel[previousindex].RemoveFromClassList("left");

        controlsCarousel[rightedge].AddToClassList("left");
        controlsCarousel[rightedge].RemoveFromClassList("right");
        controlsCarousel[rightedge].RemoveFromClassList("middle");

        Globals.Managers.Audio.PlaySFX("UI_Down");
    }
}
