using UnityEditor;
using UnityEngine;

public class AngleTrigger : RadialTrigger
{
    [Range(0, 360)]
    [SerializeField] private float _forwardAngleThreshold;

    protected override bool CheckActive(Enemy enemy)
    {
        return base.CheckActive(enemy) && CheckAngle(enemy);
    }

    private bool CheckAngle(Enemy enemy)
    {
        Vector3 enemyDirection = enemy.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, enemyDirection);
        return angle < _forwardAngleThreshold / 2;
    }

    protected override void DrawTriggerArea()
    {
        Color color = _isActivated ? Color.green : Color.red;
        Handles.color = color;
        Gizmos.color = color;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5);
        DrawArc(transform.position, transform.forward, _forwardAngleThreshold / 2, _radius);
        DrawArc(transform.position, transform.forward, -_forwardAngleThreshold / 2, _radius); 
    }

    private void DrawArc(Vector3 center, Vector3 direction, float angle, float radius)
    {
        Quaternion rotation = Quaternion.AngleAxis(angle, transform.up);
        Vector3 from = center + rotation * direction * radius;
        rotation = Quaternion.AngleAxis(-angle, transform.up);
        Vector3 to = center + rotation * direction * radius;
        Gizmos.DrawLine(center, from);
        Gizmos.DrawLine(center, to);
        Handles.DrawWireArc(center, transform.up, direction, angle, radius);
    }
}
