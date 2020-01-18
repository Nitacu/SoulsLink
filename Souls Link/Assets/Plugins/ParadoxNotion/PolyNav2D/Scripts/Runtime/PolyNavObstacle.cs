using UnityEngine;
using System.Collections;

namespace PolyNav
{

    [DisallowMultipleComponent]
    [AddComponentMenu("Navigation/PolyNavObstacle")]
    ///Place on a game object to act as an obstacle
    public class PolyNavObstacle : MonoBehaviour
    {

        public enum ShapeType
        {
            Polygon,
            Box,
            Composite
        }

        ///Raised when the state of the obstacle is changed (enabled/disabled).
        public static event System.Action<PolyNavObstacle, bool> OnObstacleStateChange;

        [Tooltip("The Shape used. Changing this will also change the Collider2D component type.")]
        public ShapeType shapeType = ShapeType.Polygon;
        [Tooltip("Added extra offset radius.")]
        public float extraOffset;
        [Tooltip("Inverts the polygon (done automatically if collider already exists due to a sprite).")]
        public bool invertPolygon = false;

        private Collider2D _collider;
        private Collider2D myCollider {
            get { return _collider != null ? _collider : _collider = GetComponent<Collider2D>(); }
        }

        ///The number of paths defining the obstacle
        public int GetPathCount() {
            if ( myCollider is BoxCollider2D ) { return 1; }
            if ( myCollider is PolygonCollider2D ) { return ( myCollider as PolygonCollider2D ).pathCount; }
            if ( myCollider is CompositeCollider2D ) { return ( myCollider as CompositeCollider2D ).pathCount; }
            return 0;
        }

        ///Returns the points defining a path
        public Vector2[] GetPathPoints(int index) {
            Vector2[] points = null;
            if ( myCollider is BoxCollider2D ) {
                var box = (BoxCollider2D)myCollider;
                var tl = box.offset + ( new Vector2(-box.size.x, box.size.y) / 2 );
                var tr = box.offset + ( new Vector2(box.size.x, box.size.y) / 2 );
                var br = box.offset + ( new Vector2(box.size.x, -box.size.y) / 2 );
                var bl = box.offset + ( new Vector2(-box.size.x, -box.size.y) / 2 );
                points = new Vector2[] { tl, tr, br, bl };
            }

            if ( myCollider is PolygonCollider2D ) {
                var poly = (PolygonCollider2D)myCollider;
                points = poly.GetPath(index);
            }

            if ( myCollider is CompositeCollider2D ) {
                var comp = (CompositeCollider2D)myCollider;
                points = new Vector2[comp.GetPathPointCount(index)];
                comp.GetPath(index, points);
            }

            if ( invertPolygon && points != null ) { System.Array.Reverse(points); }
            return points;
        }

        void Reset() {

            if ( myCollider == null ) {
                gameObject.AddComponent<PolygonCollider2D>();
                invertPolygon = true;
            }

            if ( myCollider is PolygonCollider2D ) {
                shapeType = ShapeType.Polygon;
            }

            if ( myCollider is BoxCollider2D ) {
                shapeType = ShapeType.Box;
            }

            if ( myCollider is CompositeCollider2D ) {
                shapeType = ShapeType.Composite;
            }

            myCollider.isTrigger = true;
            if ( GetComponent<SpriteRenderer>() != null ) {
                invertPolygon = true;
            }
        }

        void OnEnable() {
            if ( OnObstacleStateChange != null ) {
                OnObstacleStateChange(this, true);
            }
        }

        void OnDisable() {
            if ( OnObstacleStateChange != null ) {
                OnObstacleStateChange(this, false);
            }
        }

        void Awake() {
            transform.hasChanged = false;
        }
    }
}