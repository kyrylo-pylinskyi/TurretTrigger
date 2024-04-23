using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class RadialTrigger : MonoBehaviour
{
    [SerializeField] protected float _radius;
    [SerializeField] protected bool _isActivated;
    [SerializeField] protected Enemy _targetEenmy;
    public UnityEvent<Enemy> OnActivated;


    private void OnDrawGizmos()
    {
        var enemy = GetEnemyInside();
        _isActivated = CheckActive(enemy);
        DrawTriggerArea();
    }

    private void Update()
    {
        var enemy = GetEnemyInside();
        if (enemy == null) return; 
        _isActivated = CheckActive(enemy);
        OnActivated?.Invoke(enemy);
    }

    protected virtual bool CheckActive(Enemy enemy)
    {
        return enemy != null && CheckRadius(enemy);
    }

    private bool CheckRadius(Enemy enemy)
    {
        float distance = Vector3.Distance(transform.position, enemy.transform.position);
        return distance < _radius;
    }

    private Enemy GetEnemyInside()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius);
        Enemy enemy = null;
        foreach (Collider collider in colliders)
        {
            enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                return enemy;
            }
        }
        return enemy;
    }

    protected virtual void DrawTriggerArea()
    {
        Handles.color = _isActivated ? Color.green : Color.red;
        Handles.DrawWireDisc(transform.position, transform.up, _radius);
    }
}
