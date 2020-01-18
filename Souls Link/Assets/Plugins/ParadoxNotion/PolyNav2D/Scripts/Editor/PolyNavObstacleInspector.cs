using UnityEngine;
using UnityEditor;
using System.Collections;

namespace PolyNav
{

    [CustomEditor(typeof(PolyNavObstacle))]
    public class PolyNavObstacleInspector : Editor
    {

        private PolyNavObstacle obstacle {
            get { return target as PolyNavObstacle; }
        }

        public override void OnInspectorGUI() {

            base.OnInspectorGUI();

            if ( GUI.changed ) {
                EditorApplication.delayCall += CheckChangeType;
            }
        }

        void CheckChangeType() {
            var collider = obstacle.GetComponent<Collider2D>();
            if ( obstacle.shapeType == PolyNavObstacle.ShapeType.Polygon && !( collider is PolygonCollider2D ) ) {
                if ( collider != null ) { UnityEditor.Undo.DestroyObjectImmediate(collider); }
                var col = obstacle.gameObject.AddComponent<PolygonCollider2D>();
                UnityEditor.Undo.RegisterCreatedObjectUndo(col, "Change Shape Type");
                obstacle.invertPolygon = true;
            }

            if ( obstacle.shapeType == PolyNavObstacle.ShapeType.Box && !( collider is BoxCollider2D ) ) {
                if ( collider != null ) { UnityEditor.Undo.DestroyObjectImmediate(collider); }
                var col = obstacle.gameObject.AddComponent<BoxCollider2D>();
                UnityEditor.Undo.RegisterCreatedObjectUndo(col, "Change Shape Type");
                obstacle.invertPolygon = false;
            }

            if ( obstacle.shapeType == PolyNavObstacle.ShapeType.Composite && !( collider is CompositeCollider2D ) ) {
                if ( collider != null ) { UnityEditor.Undo.DestroyObjectImmediate(collider); }
                var col = obstacle.gameObject.AddComponent<CompositeCollider2D>();
                UnityEditor.Undo.RegisterCreatedObjectUndo(col, "Change Shape Type");
                var rb = obstacle.GetComponent<Rigidbody2D>();
                rb.simulated = false;
                obstacle.invertPolygon = true;
            }
        }
    }

}