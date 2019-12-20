using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TittleFaddeOut : MonoBehaviour
{
    // state
    TextMeshProUGUI _tittleText;
    Timer _timer;

    private void Start()
    {
        _tittleText = GetComponent<TextMeshProUGUI>();
        _timer = GetComponent<Timer>();
        SubscribeTimerEvents();
    }

    void SubscribeTimerEvents()
    {
        _timer.OnFinished += RemoveTitle;
        _timer.OnTick += TimerTickAction;
    }

    void UnsubscribeTimerEvents()
    {
        _timer.OnFinished -= RemoveTitle;
        _timer.OnTick -= TimerTickAction;
    }

    void RemoveTitle()
    {
        _tittleText.alpha = 0f; // set alpha channel (oppacity)
        UnsubscribeTimerEvents();
    }

    void TimerTickAction()
    {
        _tittleText.alpha = _timer.GetTimeLeft() / _timer.GetTotalTime();
    }

}
