using System;
using UnityEngine;

namespace Assets.Scripts
{
    static public class CarEvents
    {
        public static Action OnCarCongested;
        public static Action OnCarUncongested;

        public static Action<Transform> OnSpeederBrake;
    }
}