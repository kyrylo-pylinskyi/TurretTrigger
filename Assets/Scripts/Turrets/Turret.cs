using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour
{
    [SerializeField] private RadialTrigger _trigger;
    [SerializeField] private Transform _triggerTransform;
    [SerializeField] private Transform _turretBodyTransform;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private TurretCanon _turretCanon;

    private Coroutine _rotationCoroutine;

    private void OnEnable()
    {
        _trigger.OnActivated.AddListener(StartAttack);
    }

    private void OnDisable()
    {
        _trigger.OnActivated.RemoveAllListeners();
    }

    protected virtual void StartAttack(Enemy enemy)
    {
        Debug.Log($"Attack: {enemy.Name}");
        if (_rotationCoroutine != null)
            StopCoroutine(_rotationCoroutine);
        _rotationCoroutine = StartCoroutine(RotateToTarget(enemy.transform));
        _turretCanon.MakeShot();
    }

    private IEnumerator RotateToTarget(Transform target)
    {
        while (true)
        {
            Vector3 targetDirection = target.position - _triggerTransform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

            Quaternion newRotation = Quaternion.Slerp(_turretBodyTransform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            _turretBodyTransform.rotation = newRotation;
            _triggerTransform.rotation = newRotation;
            yield return null;
        }
    }
}
