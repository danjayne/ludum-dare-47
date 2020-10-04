//using UnityEngine;

//public class PlayerCountdown : MonoBehaviour
//{
//    public static float MaxTime = 10;
//    public CountdownBar CountdownBar;

//    private PlayerAttackLoop _PlayerAttackLoop;
//    private float _CurrentTime = 0;
//    //private Animator _Animator;

//    private void Awake()
//    {
//        _PlayerAttackLoop = GetComponent<PlayerAttackLoop>();
//        //_Animator = GetComponent<Animator>();
//    }

//    private void Start()
//    {
//        _CurrentTime = 0f;
//        CountdownBar.SetMaxTime(MaxTime);
//    }

//    private void Update()
//    {
//        if (_PlayerAttackLoop._actionsActive)
//            return;

//        if (_PlayerAttackLoop.IsInitializingActions)
//        {
//            _CurrentTime = _PlayerAttackLoop.ElapsedMs;
//            CountdownBar.SetTime(_CurrentTime);
//        }
//    }
//}
