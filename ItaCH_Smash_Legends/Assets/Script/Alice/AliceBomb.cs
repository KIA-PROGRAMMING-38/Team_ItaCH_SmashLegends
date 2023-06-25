using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliceBomb : MonoBehaviour
{
    public Transform[] point;
    public ParticleSystem[] effect;
    private bool _bezier;
    private float time;

    private void Awake()
    {
        _bezier = true;
    }

    private void Update()
    {
        if (_bezier)
        {
            time += Time.deltaTime / 1f;
            ThirdBezierCurve(point, time);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            time = 0;
            _bezier = false;
            transform.rotation = Quaternion.Euler(-90, 0, 0);

            PlayEffect(other).Forget();
        }
        //if (other.CompareTag("Enemy"))
        //{
        //    BombHit(other);
        //}
    }

    private async UniTaskVoid PlayEffect(Collider other)
    {
        for (int index = 0; index < effect.Length; ++index)
        {
            effect[index].transform.SetParent(null);
        }
        effect[0].gameObject.SetActive(true);
        effect[0].Play();

        await UniTask.Delay(4500);
        effect[1].Play();
        effect[2].Play();
        gameObject.SetActive(false);

        _bezier = true;
    }

    // TODO : 피격 시 같이 구현
    //private void BombHit(Collider other)
    //{
    //    Rigidbody rigidbody = other.GetComponent<Rigidbody>();
    //    rigidbody.velocity = Vector3.up * 3f;
    //}

    public void ThirdBezierCurve(Transform[] point, float time)
    {
        Vector3 middleOne = Vector3.Lerp(point[0].position, point[1].position, time);
        Vector3 middleTwo = Vector3.Lerp(point[1].position, point[2].position, time);
        Vector3 middleThree = Vector3.Lerp(point[2].position, point[3].position, time);
        Vector3 middleFour = Vector3.Lerp(middleOne, middleTwo, time);
        Vector3 middleFive = Vector3.Lerp(middleTwo, middleThree, time);

        transform.position = Vector3.Lerp(middleFour, middleFive, time);


        if (time < 0.6f)
        {
            transform.localRotation = Quaternion.Euler(-180, 0, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(-30, 0, 0);
        }

        // TODO : 더 가독성을 높이려고 했으나 추후에 다시 리팩토링
        //for (int index = 0; index < point.Length - 1; ++index)
        //{
        //    point[index].position = Vector3.Lerp(point[index].position, point[index + 1].position, time);
        //}

        //Vector3 middleOne = Vector3.Lerp(point[0].position, point[1].position, time);
        //Vector3 middleTwo = Vector3.Lerp(point[1].position, point[2].position, time);

        //transform.position = Vector3.Lerp(middleOne, middleTwo, time);
    }
}
