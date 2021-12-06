using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ToolTip : MonoBehaviour
{
    private Text TipText;
    private Text Content;
    private CanvasGroup canvasGroup;
    private float AlphaValue = 0;
    private float Smooth = 8;

    // Start is called before the first frame update
    void Start()
    {
        TipText = GetComponent<Text>();
        canvasGroup = GetComponent<CanvasGroup>();
        Content = transform.Find("Content").GetComponent<Text>();
        Hide();
       // StartCoroutine(CloseTooltip());
    }
    IEnumerator CloseTooltip()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(canvasGroup.alpha != AlphaValue)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, AlphaValue, Smooth * Time.deltaTime);
            if(Mathf.Abs(canvasGroup.alpha - AlphaValue) <= 0.05)
            {
                canvasGroup.alpha = AlphaValue;
            }
        }
    }
    public void Show(string s)
    {
        TipText.text = s;
        Content.text = s;
        AlphaValue = 1;
    }
    public void Hide()
    {
        AlphaValue = 0;
    }
    public void SetToolTipPosition(Vector3 pos)
    {
        transform.localPosition = pos;
    }
}
