using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    [SerializeField] private Transform _entrance;
    [SerializeField] private Signpost _startSignpost;
    [SerializeField] private Transform _signpostsParent;

    public Transform Entrance { get { return _entrance; } }
    public Signpost StartSignpost { get {  return _startSignpost; } }

    public Transform SignpostsParent { get { return _signpostsParent; } }
}
