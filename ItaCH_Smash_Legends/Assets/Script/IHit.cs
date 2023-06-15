using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHit
{
    void Hit(Collider other);
    void GetHit(float power, int animationHash, Collider other /*int damage*/);
}
