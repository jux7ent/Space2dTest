using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Kuhpik {
    public abstract class UIScreen : MonoBehaviour, IUIScreen {
        [SerializeField] [BoxGroup("Base Settings")] private EGamestate type;

        [SerializeField] [BoxGroup("Base Settings")] private GameObject screen;

        [SerializeField] [BoxGroup("Fade")] private bool useFade = true;

        [SerializeField] [BoxGroup("Fade")] [ShowIf("useFade")] private Vector2 fadeUpDownDurationSec = new Vector2(1f, 0.4f);

        [SerializeField] [BoxGroup("Background")] private bool useBackground;

        [SerializeField] [BoxGroup("Background")] [ShowIf("useBackground")] Color backgroundColor;

        public EGamestate Type => type;
        public bool UseBackground => useBackground;
        public Color BackgroundColor => backgroundColor;

        private CanvasGroup canvasGroup;

        private void Awake() {
            if (useFade) {
                canvasGroup = GetComponent<CanvasGroup>();
                if (canvasGroup == null) {
                    canvasGroup = gameObject.AddComponent<CanvasGroup>();
                }
            }
        }

        public void ForceOpen() {
            screen.SetActive(true);
        }

        public void ForceClose() {
            screen.SetActive(false);
        }

        public virtual void Open() {
            if (closeRoutine != null) {
                StopCoroutine(closeRoutine);
            }
            
            screen.SetActive(true);
            
            if (useFade) {
                FadeCanvasGroups(true);
            }
        }

        private Coroutine closeRoutine;

        public virtual void Close() {
            if (closeRoutine != null) {
                StopCoroutine(closeRoutine);
            }
            
            if (useFade) {
                FadeCanvasGroups(false);
                closeRoutine = StartCoroutine(GameExtensions.Coroutines.WaitAndDo(fadeUpDownDurationSec[1], () => screen.SetActive(false)));
            }
            else {
                screen.SetActive(false);
            }
        }
        
        private void FadeCanvasGroups(bool open) {
            float startFadeValue = open ? 0f : 1f;
            float endFadeValue = open ? 1f : 0f;

            canvasGroup.blocksRaycasts = open;
            canvasGroup.interactable = open;
            canvasGroup.alpha = startFadeValue;
            canvasGroup.DOFade(endFadeValue, fadeUpDownDurationSec[open ? 0 : 1]);
        }

        /// <summary>
        /// Use it for special cases.
        /// </summary>
        public virtual void Refresh() { }

        /// <summary>
        /// Subscribe is called on Awake()
        /// </summary>
        public virtual void Subscribe() { }
    }
}