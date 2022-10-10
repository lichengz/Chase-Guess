using System.Collections;
using UnityEngine;
using TMPro;

namespace FusionExamples.Tanknarok
{
    public class CountdownManager : MonoBehaviour
    {
        [SerializeField] private float _levelStartcountdownFrom;
        [SerializeField] private TextMeshProUGUI _levelStartcountdownUI;
        private float _levelstartCountdownTimer;

        [SerializeField] private float _refreshCountdownFrom;
        [SerializeField] private TextMeshProUGUI _refreshCountdownUI;
        private float _RefreshCountdownTimer;

        [SerializeField] private AnimationCurve _countdownCurve;
        [SerializeField] AudioEmitter _audioEmitter;

        public delegate void Callback();

        private void Start()
        {
            Reset();
        }

        public void Reset()
        {
            _levelStartcountdownUI.transform.localScale = Vector3.zero;
        }

        public IEnumerator LevelStartCountdown(Callback callback)
        {
            _levelStartcountdownUI.transform.localScale = Vector3.zero;

            _levelStartcountdownUI.text = _levelStartcountdownFrom.ToString();
            _levelStartcountdownUI.gameObject.SetActive(true);

            int lastCount = Mathf.CeilToInt(_levelStartcountdownFrom + 1);
            _levelstartCountdownTimer = _levelStartcountdownFrom;

            while (_levelstartCountdownTimer > 0)
            {
                int currentCount = Mathf.CeilToInt(_levelstartCountdownTimer);

                if (lastCount != currentCount)
                {
                    lastCount = currentCount;
                    _levelStartcountdownUI.text = currentCount.ToString();
                    _audioEmitter.PlayOneShot();
                }

                float x = _levelstartCountdownTimer - Mathf.Floor(_levelstartCountdownTimer);

                float t = _countdownCurve.Evaluate(x);
                if (t >= 0)
                    _levelStartcountdownUI.transform.localScale = Vector3.one * t;

                _levelstartCountdownTimer -= Time.deltaTime * 1.5f;
                yield return null;
            }

            _levelStartcountdownUI.gameObject.SetActive(false);

            callback?.Invoke();
        }

        public IEnumerator RefreshCountdown(Callback callback)
        {
            _refreshCountdownUI.transform.localScale = Vector3.zero;

            _refreshCountdownUI.text = _refreshCountdownFrom.ToString();
            _refreshCountdownUI.gameObject.SetActive(true);

            int lastCount = Mathf.CeilToInt(_refreshCountdownFrom + 1);
            _RefreshCountdownTimer = _refreshCountdownFrom;

            while (_RefreshCountdownTimer > 0)
            {
                int currentCount = Mathf.CeilToInt(_RefreshCountdownTimer);

                if (lastCount != currentCount)
                {
                    lastCount = currentCount;
                    _refreshCountdownUI.text = currentCount.ToString();
                    _audioEmitter.PlayOneShot();
                }

                float x = _RefreshCountdownTimer - Mathf.Floor(_RefreshCountdownTimer);

                float t = _countdownCurve.Evaluate(x);
                if (t >= 0)
                    _refreshCountdownUI.transform.localScale = Vector3.one * t;

                _RefreshCountdownTimer -= Time.deltaTime * 1.5f;

                if (_RefreshCountdownTimer < 0)
                {
                    lastCount = Mathf.CeilToInt(_refreshCountdownFrom + 1);
                    _RefreshCountdownTimer = _refreshCountdownFrom;
                    callback?.Invoke();
                }

                yield return null;
            }

            _refreshCountdownUI.gameObject.SetActive(false);
        }
    }
}