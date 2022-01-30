using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour {
    public const float PowerScaling = 2.0f;
    public const float SmoothstepDeadbandScale = 4.0f;
    public const float MinFadeTime = 0.01f;

    public float FadeTime = 2.0f;
    public float EaseInOut = 1.0f;
    public string ToSceneName;

    public enum State {
        FadeOut,
        LoadScene,
        FadeIn,
        End,
    }

    public State CurrentState = State.FadeOut;
    public float CurrentStateTime;

    private AsyncOperation _loadOperation;

    public float OverlayAlpha = 0.0f;

    protected void Update() {
        CurrentStateTime += Time.deltaTime;
        float overlayAlpha = 0.0f;
        switch (CurrentState) {
            case State.FadeOut: {
                float t = ComputeFadeT(CurrentStateTime / Mathf.Max(MinFadeTime, FadeTime));
                overlayAlpha = t;
                if (t >= 1.0f) {
                    GoToState(State.LoadScene);
                }
                break;
            }
            case State.LoadScene:
                if (_loadOperation == null) {
                    _loadOperation = SceneManager.LoadSceneAsync(ToSceneName);
                } else {
                    if (_loadOperation.isDone) {
                        GoToState(State.FadeIn);
                    }
                }
                overlayAlpha = 1.0f;
                break;
            case State.FadeIn: {
                float t = ComputeFadeT((FadeTime - CurrentStateTime) / Mathf.Max(MinFadeTime, FadeTime));
                overlayAlpha = t;
                if (t <= 0.0f) {
                    GoToState(State.End);
                }
                break;
            }
            case State.End:
                Destroy(gameObject);
                break;
        }
        OverlayAlpha = overlayAlpha;
    }

    private void GoToState(State state) {
        CurrentState = state;
        CurrentStateTime = 0.0f;
    }

    private float ComputeFadeT(float t) {
        if (t >= 1.0f) {
            return 1.0f;
        }
        if (t <= 0.0f) {
            return 0.0f;
        }
        float easeInOut = EaseInOut * PowerScaling;
        float power = easeInOut < 0.0f ? (-easeInOut + 1.0f) : (1.0f / (easeInOut + 1.0f));
        t = Mathf.Pow(t, power);
        float smoothT = t * t * (3 - 2 * t);
        float smoothTAlpha = Mathf.Min(1.0f, Mathf.Abs(smoothT * SmoothstepDeadbandScale));
        t = t * (1.0f - smoothTAlpha) + smoothT * smoothTAlpha;
        return t;
    }

    public void OnGUI() {
        if (OverlayAlpha <= 0.0f) {
            return;
        }
        OverlayAlpha = Mathf.Min(1.0f, OverlayAlpha);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture, ScaleMode.ScaleAndCrop, alphaBlend: true, imageAspect: 0, new Color(0, 0, 0, OverlayAlpha), borderWidth: 0, borderRadius: 0);
    }
}
