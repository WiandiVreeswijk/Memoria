using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour {
    public void Open() {
        transform.DOMoveY(-4f, 2);
    }
}
