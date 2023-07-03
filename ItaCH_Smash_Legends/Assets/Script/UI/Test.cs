using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util.Method;

public class Test : MonoBehaviour
{
    [SerializeField] Image image;
    private void Start()
    {
        image = GetComponent<Image>();
    }
    public void Decline()
    {
        Method.ChangeFillAmountGradually(0, 0.5f, image).Forget();
    }

    public void Incline()
    {
        Method.ChangeFillAmountGradually(1, 0.5f, image).Forget();
    }
}
