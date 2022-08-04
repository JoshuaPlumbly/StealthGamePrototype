using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsScreen : MonoBehaviour
{
    [SerializeField] private ProgressTracker _progressTracker;
    [SerializeField] private float _duration = 2f;

    [SerializeField] private GameObject _resultScreenGO;
    [SerializeField] private Text _ammoBoText;
    [SerializeField] private Text _ammoBounceText;
    [SerializeField] private Text _timeText;
    [SerializeField] private Text _timeBounceText;
    [SerializeField] private Text _killsText;
    [SerializeField] private Text _killsBounceText;
    [SerializeField] private Text _totalText;

    private void Awake()
    {
        LevelCompleation.LevelCompleated += InvokeResutls;
    }

    private void OnDestroy()
    {
        LevelCompleation.LevelCompleated -= InvokeResutls;
    }

    private void InvokeResutls()
    {
        StartCoroutine(Results(_progressTracker.CurrentProgress()));
    }

    public IEnumerator Results(PlayResults results)
    {
        int timePoints = Mathf.RoundToInt(results.time * 100);
        int ammoPoints = results.ammo * 25;
        int killPoints = results.kills * 200;

        _timeText.text = System.TimeSpan.FromSeconds(results.time).ToString("mm':'ss':'fff");
        _ammoBounceText.text = results.ammo.ToString();
        _killsBounceText.text = results.kills.ToString();

        int total = timePoints + ammoPoints + killPoints;

        _resultScreenGO.SetActive(true);

        float time = 0;

        while (time < 1)
        {
            time += Time.deltaTime;
            float t = time / _duration;
            _ammoBounceText.text = Mathf.Lerp(0, ammoPoints, t).ToString("0");
            _timeBounceText.text = Mathf.Lerp(0, timePoints, t).ToString("0");
            _killsBounceText.text = Mathf.Lerp(0, killPoints, t).ToString("0");
            _totalText.text = Mathf.Lerp(0, total, t).ToString("0");
            yield return null;
        }

        _ammoBounceText.text = ammoPoints.ToString();
        _timeBounceText.text = timePoints.ToString();
        _killsBounceText.text = killPoints.ToString();

        _totalText.text = total.ToString();
    }
}