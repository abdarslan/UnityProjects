using System;
using UnityEngine;
using UnityEngine.UI;

// Minimal DOTween-compatible stubs to allow project to compile when DOTween isn't installed.
// These do NOT perform real tweening; they only provide the symbols used by MicroBar.
namespace DG.Tweening
{
    public enum Ease { Linear, InOutSine, InOutQuad, OutBounce }

    public class Tween
    {
        public Tween SetDelay(float d) { return this; }
        public Tween SetEase(Ease e) { return this; }
        public Tween OnUpdate(Action callback) { return this; }
        public Tween OnComplete(Action callback) { return this; }
        public Tween OnKill(Action callback) { return this; }
        public Tween OnStart(Action callback) { return this; }
        public bool IsActive() { return false; }
        public void Kill() { }
    }

    public class Sequence : Tween
    {
        public Sequence Append(Tween t) { return this; }
        public Sequence Join(Tween t) { return this; }
        public Sequence AppendInterval(float f) { return this; }
    }

    public static class DOTween
    {
        public static Sequence Sequence() { return new Sequence(); }
    }

    // Extension method stubs for Unity types used by MicroBar.
    public static class DOTweenModuleUnity
    {
        // UI Image
        public static Tween DOColor(this Image img, Color c, float duration) { return new Tween(); }
        public static Tween DOFade(this Image img, float endValue, float duration) { return new Tween(); }
        public static Tween DOFillAmount(this Image img, float endValue, float duration) { return new Tween(); }

        // SpriteRenderer
        public static Tween DOColor(this SpriteRenderer sr, Color c, float duration) { return new Tween(); }
        public static Tween DOFade(this SpriteRenderer sr, float endValue, float duration) { return new Tween(); }

        // SpriteMask (scale X helper)
        public static Tween DOScaleX(this SpriteMask sm, float newX, float duration) { return new Tween(); }

        // RectTransform / Transform
        public static Tween DOLocalMove(this RectTransform rt, Vector3 pos, float duration) { return new Tween(); }
        public static Tween DOLocalMove(this Transform t, Vector3 pos, float duration) { return new Tween(); }
        public static Tween DOLocalMoveX(this RectTransform rt, float x, float duration) { return new Tween(); }
        public static Tween DOLocalMoveX(this Transform t, float x, float duration) { return new Tween(); }
        public static Tween DOLocalMoveY(this RectTransform rt, float y, float duration) { return new Tween(); }
        public static Tween DOLocalMoveY(this Transform t, float y, float duration) { return new Tween(); }
        public static Tween DOLocalRotate(this RectTransform rt, Vector3 rot, float duration) { return new Tween(); }
        public static Tween DOLocalRotate(this Transform t, Vector3 rot, float duration) { return new Tween(); }
        public static Tween DOScale(this RectTransform rt, Vector3 scale, float duration) { return new Tween(); }
        public static Tween DOScale(this Transform t, Vector3 scale, float duration) { return new Tween(); }
        public static Tween DOScaleX(this RectTransform rt, float x, float duration) { return new Tween(); }
        public static Tween DOScaleX(this Transform t, float x, float duration) { return new Tween(); }
        public static Tween DOScaleY(this RectTransform rt, float y, float duration) { return new Tween(); }
        public static Tween DOScaleY(this Transform t, float y, float duration) { return new Tween(); }

        // Punch
        public static Tween DOPunchPosition(this RectTransform rt, Vector2 punch, float duration, int vibrato, float elasticity) { return new Tween(); }
        public static Tween DOPunchPosition(this Transform t, Vector2 punch, float duration, int vibrato, float elasticity) { return new Tween(); }
        public static Tween DOPunchRotation(this RectTransform rt, Vector3 punch, float duration, int vibrato, float elasticity) { return new Tween(); }
        public static Tween DOPunchRotation(this Transform t, Vector3 punch, float duration, int vibrato, float elasticity) { return new Tween(); }
        public static Tween DOPunchScale(this RectTransform rt, Vector2 punch, float duration, int vibrato, float elasticity) { return new Tween(); }
        public static Tween DOPunchScale(this Transform t, Vector2 punch, float duration, int vibrato, float elasticity) { return new Tween(); }
        public static Tween DOPunchAnchorPos(this RectTransform rt, Vector2 punch, float duration, int vibrato, float elasticity) { return new Tween(); }

        // Shake
        public static Tween DOShakePosition(this RectTransform rt, float duration, float strength, int vibrato) { return new Tween(); }
        public static Tween DOShakePosition(this Transform t, float duration, float strength, int vibrato) { return new Tween(); }
        // Overloads with randomness parameter (used by MicroBar calls)
        public static Tween DOShakePosition(this RectTransform rt, float duration, float strength, int vibrato, float randomness) { return new Tween(); }
        public static Tween DOShakePosition(this Transform t, float duration, float strength, int vibrato, float randomness) { return new Tween(); }
        public static Tween DOShakeRotation(this RectTransform rt, float duration, Vector3 strength, int vibrato, float randomness) { return new Tween(); }
        public static Tween DOShakeRotation(this Transform t, float duration, Vector3 strength, int vibrato, float randomness) { return new Tween(); }
        public static Tween DOShakeScale(this RectTransform rt, float duration, float strength, int vibrato, float randomness) { return new Tween(); }
        public static Tween DOShakeScale(this Transform t, float duration, float strength, int vibrato, float randomness) { return new Tween(); }
        public static Tween DOShakeAnchorPos(this RectTransform rt, float duration, float strength, int vibrato, float randomness) { return new Tween(); }

        // Anchor moves
        public static Tween DOAnchorPos(this RectTransform rt, Vector2 pos, float duration) { return new Tween(); }
        public static Tween DOAnchorPos(this RectTransform rt, Vector2 pos, float duration, bool snapping) { return new Tween(); }
        public static Tween DOAnchorPosX(this RectTransform rt, float x, float duration) { return new Tween(); }
        public static Tween DOAnchorPosY(this RectTransform rt, float y, float duration) { return new Tween(); }
    }
}
