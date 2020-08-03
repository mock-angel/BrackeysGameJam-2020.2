using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FlyInAnimation : MonoBehaviour
{
    #region VARIABLES

    //Start Animation
    [Header("Start Animation")]
    public bool fromTop;
    public bool fromBottom;
    public bool fromRight;
    public bool fromLeft;
    public bool fadeIn;
    public bool zoomIn;

    //Variables
    [Header("Variables")]
    public float animationTime = 0.25f; //How long the Animation lasts in Seconds
    public float waitTime = 0.05f;  //How long to wait till Animation starts in Seconds (AFTER first frame of Animation)
    public bool onEnable;

    //Can be assigned. Don't need to
    [Header("Leave blank if not needed")]
    public TMP_Text text;
    public Image image;
    public Color color;

    private Vector3 startPos;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //seconds = 0.15f;
        startPos = transform.localPosition;


        #region IF SCRIPT IS ON TEXT OR IMAGE

        if (text != null)
        {
            color = text.gameObject.GetComponent<TextMeshProUGUI>().color;
            color.a = 0f;
        }

        if (image != null)
        {
            color = image.gameObject.GetComponent<Image>().color;
            color.a = 0f;
        }

        #endregion

        //Start Animation
        StartCoroutine(StartAnimation());
    }

    //Start Animation
    IEnumerator StartAnimation()
    {
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / animationTime;

            
            #region FLY IN FROM DIFFERENT POSITION

            //Fly in from Top
            if (fromTop)
            {
                transform.localPosition = Vector3.Lerp(new Vector3(0, Screen.height * 2, 0), startPos, Mathf.SmoothStep(0f, 1f, t));
            }

            //Fly in from Bottom
            else if (fromBottom)
            {
                transform.localPosition = Vector3.Lerp(new Vector3(0, -Screen.height * 2, 0), startPos, Mathf.SmoothStep(0f, 1f, t));
            }

            //Fly in from Right
            else if (fromRight)
            {
                transform.localPosition = Vector3.Lerp(new Vector3(Screen.width * 2, startPos.y, 0), startPos, Mathf.SmoothStep(0f, 1f, t));
            }

            //Fly in from Left
            else if (fromLeft)
            {
                transform.localPosition = Vector3.Lerp(new Vector3(-Screen.width * 2, startPos.y, 0), startPos, Mathf.SmoothStep(0f, 1f, t));
            }

            #endregion

            #region FLY IN FROM CURRENT POSITION

            //Fade In (color opacity)
            if (fadeIn)
            {
                if (text != null)
                {
                    text.gameObject.GetComponent<TextMeshProUGUI>().color = color;
                    color.a += t * 1.5f;
                }
                if (image != null)
                {
                    image.gameObject.GetComponent<Image>().color = color;
                    color.a += t * 1.5f;
                }
            }

            //Zoom In (Scale)
            if(zoomIn)
            {
                transform.localScale = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(1, 1, 1), Mathf.SmoothStep(0f, 1f, t));
            }

            #endregion

            #region WAIT FOR ANIMATION TO START

            //Wait until Start of Animation (AFTER first Frame)
            if (waitTime > 0)
            {
                yield return new WaitForSeconds(waitTime);
                waitTime = 0f;
            }

            #endregion

            yield return null;
        }

        transform.localPosition = startPos;
    }
}

