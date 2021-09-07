using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kuhpik {
    public class StoreTransform {
        private Vector3 position;
        private Quaternion rotation;
        private Vector3 localScale;

        private StoreTransform() { }

        public StoreTransform(Transform transform) {
            position = transform.position;
            rotation = transform.rotation;
            localScale = transform.localScale;
        }

        public void WriteToTransform(Transform transform) {
            transform.position = position;
            transform.rotation = rotation;
            transform.localScale = localScale;
        }
    }
}