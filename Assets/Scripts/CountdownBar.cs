using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class CountdownBar : MonoBehaviour
{
    public Gradient Gradient;
    public Image Fill;
    public Image ActionImagePrefab;
    public Sprite StabSprite;
    public Sprite SlashSprite;
    public Sprite DashSprite;

    private Slider _Slider;

    private void Awake()
    {
        _Slider = GetComponent<Slider>();
    }

    public void SetMaxTime(float maxTime)
    {
        _Slider.maxValue = maxTime;
        _Slider.value = 0f;

        Fill.color = Gradient.Evaluate(0f);
    }

    public void AddActionImage(PlayerAction playerAction)
    {
        var pref = Instantiate(ActionImagePrefab, _Slider.fillRect.transform, false);

        switch (playerAction.PlayerActionType)
        {
            case PlayerActionTypeEnum.Slash:
                pref.sprite = SlashSprite;
                break;
            case PlayerActionTypeEnum.Stab:
                pref.sprite = StabSprite;
                break;
            case PlayerActionTypeEnum.Dash:
                pref.sprite = DashSprite;
                break;
        }

        var rectTransform = GetComponent<RectTransform>();
        Vector3[] fourCorners = new Vector3[4];
        rectTransform.GetWorldCorners(fourCorners);
        float realWidth = fourCorners[2].x - fourCorners[0].x;

        pref.transform.SetPositionAndRotation(new Vector3(pref.transform.position.x + (_Slider.normalizedValue * realWidth), pref.transform.position.y, pref.transform.position.z), Quaternion.identity);
    }

    public void SetTime(float timePassed)
    {
        _Slider.value = timePassed;

        Fill.color = Gradient.Evaluate(_Slider.normalizedValue);
    }
}

