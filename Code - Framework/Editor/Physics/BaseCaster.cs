using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Utility;

public abstract class BaseCaster : MonoBehaviour
{
    #region Enum

    public enum CastUpdateType
    {
        Update,
        FixedUpdate,
        Manual
    }

    public enum ShowInfo
    {
        All = ~0,                       // 111 111 111 111 - int: groot
        None = 0,                       // 000 000 000 000 - int: 0
        Collider = 1 << 0,              // 000 000 000 001 - int: 1
        Target = 1 << 1,                // 000 000 000 010 - int: 2
        HitNormals = 1 << 2,            // 000 000 000 100 - int: 4
        HitCollidersHighlight =  1 << 3,// 000 000 001 000 - int: 8
        ImpactLocations = 1 << 4,       // 000 000 010 000 - int: 16
        PathToTarget = 1 << 5           // 000 000 100 000 - int: 32
                                        // 000 000 010 011 - user input 
    }
    #endregion

    #region Inspector fields

    [Header("DebugInfo")]
    [EnumFlag]
    public ShowInfo ShowAlways = ShowInfo.None;
    [EnumFlag]
    public ShowInfo ShowOnSelected = ShowInfo.Collider;

    [SerializeField, EditorReadOnly]
    private int hits;
    [SerializeField, EditorReadOnly]
    private GameObject[] hitObjects;
    

    [Header("Caster Settings")]
    public CastUpdateType UpdateType;
    public LayerMask Mask;
    public RaycastHit2D[] Result
    {
        private set
        {
            result = value;
            hits = value.Length;
            hitObjects = value.Where(x => x.collider != null).Select(x => x.collider.gameObject).ToArray();
        }
        get
        {
            return result;
        }
    }
    private RaycastHit2D[] result;

    #endregion

    #region Colors

    protected Color ColliderC = new Color(0.1f, 0.75f, 0.25f, 1f);
    protected Color TargetC = new Color(0.1f, 0.75f, 0.25f, 0.95f);
    protected Color HitNormalsC = new Color(0.75f, 0.1f, 0.25f, 1f);
    protected Color HitCollidersC = new Color(0.75f, 0.1f, 0.25f, 1f);
    protected Color HitCollidersCFill = new Color(0.75f, 0.1f, 0.25f, 0.75f);
    protected Color ImpactLocationsC = new Color(0.75f, 0.1f, 0.1f, 1f);
    protected Color PathToTargetC = new Color(0.1f, 0.75f, 0.25f, 0.85f);

    #endregion

    #region Cast & Update loops

    public abstract RaycastHit2D[] Cast();

    void Update ()
    {
        if (UpdateType != CastUpdateType.Update)
            return;

        Result = Cast();
    }

    void FixedUpdate()
    {
        if (UpdateType != CastUpdateType.FixedUpdate)
            return;

        Result = Cast();
    }

    #endregion

    #region Gizmos

    protected abstract void DrawWireSelf();
    protected abstract void DrawWireSelf(Vector3 pos);
    protected abstract void DrawPathToTarget();
    protected abstract void DrawTarget();

    private void OnDrawGizmos()
    {
        DrawGizmos(ShowAlways);
    }

    private void OnDrawGizmosSelected()
    {
        DrawGizmos(ShowOnSelected);
    }

    protected void DrawGizmos(ShowInfo info)
    {
        
        if ((info & ShowInfo.PathToTarget) != 0)                                // Path
            DrawPathToTarget();
        
        if ((info & ShowInfo.Target) != 0)                                      // Target
            DrawTarget();
        
        if ((info & ShowInfo.ImpactLocations) != 0 && result != null)           // ImpactLocations
        {
            foreach (RaycastHit2D hit in result)
            {
                DrawWireSelf(hit.centroid);
                GizmosHelper.DrawWireCircle(hit.centroid, 0.1f, ImpactLocationsC);
            }
        }
        
        if ((info & ShowInfo.HitCollidersHighlight) != 0 && result != null)      // Hit colliders
        {
            foreach (RaycastHit2D hit in result)
            {
                GizmosHelper.DrawCollider(hit.collider.transform.position, hit.collider, HitCollidersCFill, HitCollidersC);
            }
        }
       
        if ((info & ShowInfo.HitNormals) != 0 && result != null)                // Hit Normals & Pos
        {
            foreach (RaycastHit2D hit in result)
            {
                GizmosHelper.DrawRay(hit.point, hit.normal, HitNormalsC);
                GizmosHelper.DrawWireCircle(hit.point, 0.1f, HitNormalsC);
            }
        }

        if ((info & ShowInfo.Collider) != 0)                                    // Collider
            DrawWireSelf();
    }

    #endregion
}
