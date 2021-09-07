using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

namespace Kuhpik {
    public static class GameExtensions {
        public static RaycastHit cachedRaycastHit;
        public static NavMeshHit cachedNavMeshHit;

        public static Vector3 zeroVector = Vector3.zero;
        public static Vector3 upVector = Vector3.up;
        public static Vector3 forwardVector = Vector3.forward;
        public static Vector3 downVector = Vector3.down;
        public static Vector3 rightVector = Vector3.right;

        public static List<Transform> cachedTransformsList = new List<Transform>();

        private static Dictionary<float, WaitForSeconds> cachedWaitForSeconds = new Dictionary<float, WaitForSeconds>();

        public static class Coroutines {
            public static IEnumerator WaitAndDo(float delaySec, Action action) {
                yield return GetWaitForSeconds(delaySec);
                action?.Invoke();
            }

            public static IEnumerator WaitOneFrame(Action action) {
                yield return null;
                action.Invoke();
            }

            public static IEnumerator WaitWhile(Func<bool> waitCondition, Action action, float checkDealy = 0) {
                while (waitCondition()) {
                    yield return GetWaitForSeconds(checkDealy);
                }

                action?.Invoke();
            }

            public static IEnumerator LoopWithDelay(Action action, float iterationDelaySec) {
                while (true) {
                    action?.Invoke();
                    yield return GetWaitForSeconds(iterationDelaySec);
                }
            }

            public static IEnumerator LoopWithDelayWhile(Action action, float iterationDelaySec,
                Func<bool> stopCondition) {
                while (stopCondition?.Invoke() == false) {
                    action?.Invoke();
                    yield return GetWaitForSeconds(iterationDelaySec);
                }
            }

            public static IEnumerator WaitForCoroutine(Coroutine coroutine, Action action) {
                yield return coroutine;
                action?.Invoke();
            }

            public static IEnumerator ObjectArrivedAtPosition(GameObject obj, Vector3 position,
                Action onComplete = null,
                Action movementAction = null, float checkDelaySec = 0.1f, float eps = 0.1f) {
                var waitFor = new WaitForFixedUpdate();
                while (Vector3.Distance(obj.transform.position, position) > eps) {
                    yield return waitFor;
                    movementAction?.Invoke();
                }

                onComplete?.Invoke();
            }
        }


        public static List<Vector3> RemovePointIfBetweenDistanceLessValue(List<Vector3> points, float distance) {
            List<Vector3> result = new List<Vector3>();

            for (int i = 0; i < points.Count - 1; ++i) {
                result.Add(points[i]);
                while (i < points.Count - 1 && Vector3.Distance(points[i], points[i + 1]) < distance) {
                    ++i;
                }
            }

            return result;
        }

        public static void DrawPoints(List<Vector3> points, Color color, float height = 5,
            float firstAdditionalPointHeight = 0, float duration = 5f) {
            for (int i = 0; i < points.Count - 1; ++i) {
                Debug.DrawLine(points[i] + Vector3.up * (height + (i == 0 ? firstAdditionalPointHeight : 0)),
                    points[i + 1] + Vector3.up * height, color, duration);
            }
        }

        public static List<Transform> GetChildTransformsAsSortedList(Transform container) {
            List<Transform> result = new List<Transform>();
            for (int i = 0; i < container.childCount; ++i) {
                result.Add(container.GetChild(i));
            }

            return result;
        }

        public static List<T> GetChildAsSortedList<T>(Transform container) {
            List<T> result = new List<T>();
            for (int i = 0; i < container.childCount; ++i) {
                result.Add(container.GetChild(i).GetComponent<T>());
            }

            return result;
        }

        public static WaitForSeconds GetWaitForSeconds(float sec) {
            if (!cachedWaitForSeconds.ContainsKey(sec)) {
                cachedWaitForSeconds.Add(sec, new WaitForSeconds(sec));
            }

            return cachedWaitForSeconds[sec];
        }

        public static Transform FindTransformWithTagInChild(Transform container, string tag) {
            int childCount = container.childCount;
            for (int i = 0; i < childCount; ++i) {
                Transform childTransform = container.GetChild(i);
                if (childTransform.CompareTag(tag)) {
                    return childTransform;
                }
            }

            return null;
        }

        public static void DetachChildrenDoAttach(Transform parent, Action action) {
            List<Transform> children = DetachChildren(parent);
            action?.Invoke();
            AttachChildren(parent, children);
        }

        public static List<Transform> DetachChildren(Transform parent) {
            List<Transform> children = new List<Transform>();
            while (parent.childCount != 0) {
                children.Add(parent.GetChild(0));
                children[children.Count - 1].parent = null;
            }

            return children;
        }

        public static void AttachChildren(Transform parent, List<Transform> children) {
            for (int i = 0; i < children.Count; ++i) {
                children[i].parent = parent;
            }
        }

        public static class Random {
            private static System.Random random = new System.Random();

            public static T GetRandomElementFromArray<T>(T[] array) {
                return array[random.Next(0, array.Length)];
            }

            public static T GetRandomElementFromArray<T>(Array array) {
                return (T) array.GetValue(random.Next(0, array.Length));
            }

            public static T GetRandomElementFromList<T>(List<T> list) {
                return list[random.Next(0, list.Count)];
            }

            public static T GetRandomElementFromEnum<T>() where T : Enum {
                return GetRandomElementFromArray<T>(Enum.GetValues(typeof(T)));
            }
        }

        private static Dictionary<string, int> animatorHashCache = new Dictionary<string, int>();

        public static int GetAnimatorHashFromTag(string tag) {
            if (!animatorHashCache.ContainsKey(tag)) {
                animatorHashCache.Add(tag, Animator.StringToHash(tag));
            }

            return animatorHashCache[tag];
        }

        private static Dictionary<int, string> intToStringCache = new Dictionary<int, string>();

        public static string GetStringFromInt(int number) {
            if (!intToStringCache.ContainsKey(number)) {
                intToStringCache.Add(number, $"{number}");
            }

            return intToStringCache[number];
        }

        public static Vector3 GetWorldScale(Transform objTransform) {
            Transform parent = objTransform.parent;
            objTransform.parent = null;
            Vector3 result = objTransform.localScale;
            objTransform.parent = parent;
            return result;
        }

        public static void SetWorldScale(Transform objTransform, Vector3 worldScale) {
            Transform parent = objTransform.parent;
            objTransform.parent = null;
            objTransform.localScale = worldScale;
            objTransform.parent = parent;
        }

        public static T CopyComponentValuesToAnother<T>(T copyFrom, T copyTo) where T : Component {
            System.Reflection.FieldInfo[] fields = copyFrom.GetType().GetFields(BindingFlags.Instance |
                BindingFlags.Static |
                BindingFlags.NonPublic |
                BindingFlags.Public |
                BindingFlags.DeclaredOnly);

            for (int i = 0; i < fields.Length; ++i) {
                fields[i].SetValue(copyTo, fields[i].GetValue(copyFrom));
            }

            return copyTo;
        }

        public static T GetCopyOf<T>(this Component comp, T other) where T : Component {
            Type type = comp.GetType();
            if (type != other.GetType()) return null; // type mis-match
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                                 BindingFlags.Default | BindingFlags.DeclaredOnly;
            PropertyInfo[] pinfos = type.GetProperties(flags);
            foreach (var pinfo in pinfos) {
                if (pinfo.CanWrite) {
                    try {
                        pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                    }
                    catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
                }
            }

            FieldInfo[] finfos = type.GetFields(flags);
            foreach (var finfo in finfos) {
                finfo.SetValue(comp, finfo.GetValue(other));
            }

            return comp as T;
        }

        private static Dictionary<GameObject, Dictionary<Type, Component>> cachedComponents =
            new Dictionary<GameObject, Dictionary<Type, Component>>();

        public static T GetComponent<T>(GameObject gameObj) where T : Component {
            if (!cachedComponents.ContainsKey(gameObj)) {
                cachedComponents.Add(gameObj, new Dictionary<Type, Component>());
            }

            if (!cachedComponents[gameObj].ContainsKey(typeof(T))) {
                cachedComponents[gameObj].Add(typeof(T), gameObj.GetComponent(typeof(T)));
            }

            return (T) cachedComponents[gameObj][typeof(T)];
        }

        public static int GetSubMeshIndexByTriangleIndex(Mesh mesh, int triangleIndex) {
            int limit = triangleIndex * 3;
            int subMeshIndex = -1;
            for (subMeshIndex = 0; subMeshIndex < mesh.subMeshCount; ++subMeshIndex) {
                int numIndices = mesh.GetTriangles(subMeshIndex).Length;
                if (numIndices > limit)
                    break;

                limit -= numIndices;
            }

            return subMeshIndex;
        }

        public static bool RandomWithPercent(int percent100Int) {
            return percent100Int < UnityEngine.Random.Range(0, 100);
        }

        public static IEnumerator CheckInternetConnection(Action<bool> action) {
            using UnityWebRequest webRequest = UnityWebRequest.Get("http://google.com");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success) {
                action(true);
            } else {
                action(false);
            }
        }

        public static class UI {
            public static Vector2 WorldToRectTransformPosition(Vector3 targetWorldPos, RectTransform rectTransform,
                Camera camera) {
                Vector3 viewportPosition = camera.WorldToViewportPoint(targetWorldPos);
                if (viewportPosition.z < 0) {
                    viewportPosition.x = 1 - viewportPosition.x;
                    viewportPosition.y = 1 - viewportPosition.y;
                }

                Vector2 rectPosition = new Vector2(
                    ((viewportPosition.x * rectTransform.sizeDelta.x) - (rectTransform.sizeDelta.x * 0.5f)),
                    ((viewportPosition.y * rectTransform.sizeDelta.y) - (rectTransform.sizeDelta.y * 0.5f))
                );
                return rectPosition;
            }

            public static Vector2 NormalizeResolutionVector2(Vector2 input, Vector2 defaultResolution,
                Vector2 resolution) {
                return new Vector2(input.x * (defaultResolution.x / resolution.x),
                    input.y * (defaultResolution.y / resolution.y));
            }

            public static TweenerCore<float, float, FloatOptions> EnableDisableCanvasGroup(CanvasGroup canvasGroup,
                bool enable, float duration = 0f, float startValue = -1f) {
                if (enable) {
                    canvasGroup.gameObject.SetActive(true);
                }

                canvasGroup.interactable = enable;
                canvasGroup.blocksRaycasts = enable;

                if (startValue > -1) {
                    canvasGroup.alpha = startValue;
                }

                return canvasGroup.DOFade(enable ? 1f : 0f, duration)
                    .OnComplete(() => { canvasGroup.gameObject.SetActive(enable); });
            }

            public static bool CursorOverUI() {
                // Check mouse
                if (EventSystem.current.IsPointerOverGameObject()) {
                    return true;
                }

                // Check touches
                for (int i = 0; i < Input.touchCount; i++) {
                    var touch = Input.GetTouch(i);
                    if (touch.phase == TouchPhase.Began) {
                        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId)) {
                            return true;
                        }
                    }
                }

                return false;
            }
        }
    }
}