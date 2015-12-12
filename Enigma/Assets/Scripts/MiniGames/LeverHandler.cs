using UnityEngine;
using System.Collections;
using Enigma.UserInterface;
using Enigma.Sound;

namespace Enigma.MiniGames
{
    public class LeverHandler : MonoBehaviour
    {
        public enum State { Top = 0, Middle, Bottom };
        private float[] stateRotationsZ = { -45f, 0f, 45f };
        private static float transitionDuration = 0.5f;

        [SerializeField]
        private State state;
        [SerializeField]
        private State stateTarget;

        private bool isSolved;

        void Awake()
        {
            isSolved = false;
            state = State.Middle;
        }

        public void Use()
        {
            if (!isSolved)
            {
                Vector3 rotationLocal = Vector3.zero;
                LeanTween.cancel(this.gameObject);
                if (state == State.Top)
                {
                    state = State.Middle;
                    rotationLocal.z = stateRotationsZ[(int)state];
                    LeanTween.rotateLocal(this.gameObject, rotationLocal, transitionDuration);
                }
                else if (state == State.Middle)
                {
                    state = State.Bottom;
                    rotationLocal.z = stateRotationsZ[(int)state];
                    LeanTween.rotateLocal(this.gameObject, rotationLocal, transitionDuration);
                }
                else if (state == State.Bottom)
                {
                    state = State.Top;
                    rotationLocal.z = stateRotationsZ[(int)state];
                    LeanTween.rotateLocal(this.gameObject, rotationLocal, transitionDuration);
                }
                Levers.Singleton.checkIfSolved();
                LeversMoved.Singleton.PlaySound();
            }
            else
            {
                UIHandler.Singleton.ShowPopUp("It's locked now.", null);
            }
        }

        public void MakeSolved()
        {
            isSolved = true;
        }

        public bool IsCorrect()
        {
            return state == stateTarget;
        }
    }
}