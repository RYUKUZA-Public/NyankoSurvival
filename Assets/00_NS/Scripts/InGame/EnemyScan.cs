using System;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyScan : NewMonoBehaviour
{
    [SerializeField] private float scanRange;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] public Transform NearestTarget { get; set; }
    [SerializeField] private RaycastHit2D[] _targets;
    
    public override void NewFixedUpdate()
    {
        _targets = Physics2D.CircleCastAll(
            transform.position,
            scanRange, 
            Vector2.zero, 
            0, 
            targetLayer);

        NearestTarget = GetNearest();

    }

    private Transform GetNearest()
    {
        Transform result = null;

        float diff = 100;

        foreach (RaycastHit2D target in _targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;

            float curDiff = Vector3.Distance(myPos, targetPos);

            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }
        
        return result;
    }
}
