using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TurretCanon : MonoBehaviour
{
    [SerializeField] private Transform _muzzleTransform;
    [SerializeField] private ParticleSystem _shootFlashEffect;
    [SerializeField] private List<AudioClip> _shootAudioEffects;
    [Range(0f, 0.2f)]
    [SerializeField] private float _recoilDelay;
    [Range(0f, 0.2f)]
    [SerializeField] private float _recoilOffset;

    private bool _canShoot = true;
    private Vector3 _originalPosition;
    private AudioSource _audioSource;

    private void Start()
    {
        _originalPosition = transform.localPosition;
        _audioSource = GetComponent<AudioSource>();
    }

    public void MakeShot()
    {
        if (!_canShoot) return;

        if (_shootAudioEffects.Count > 0)
        {
            int index = Random.Range(0, _shootAudioEffects.Count);
            var shootAudioEffect = _shootAudioEffects[index];
            _audioSource.PlayOneShot(shootAudioEffect);
        }

        if (_shootFlashEffect != null)
        {
            _shootFlashEffect.Play();
        }

        StartCoroutine(CanonRecoil());
    }

    private IEnumerator CanonRecoil()
    {
        _canShoot = false;
        Vector3 target = _originalPosition - transform.forward * _recoilOffset;

        float elapsedTime = 0f;
        while (elapsedTime < _recoilDelay)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, target, elapsedTime / _recoilDelay);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = target;

        yield return new WaitForSeconds(0.1f);

        elapsedTime = 0f; // Reset elapsed time for the next loop
        while (elapsedTime < _recoilDelay)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _originalPosition, elapsedTime / _recoilDelay);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = _originalPosition;
        _canShoot = true;
    }
}
