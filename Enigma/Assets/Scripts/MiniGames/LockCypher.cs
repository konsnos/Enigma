using UnityEngine;
using System.Collections;

namespace Enigma.MiniGames
{
    public class LockCypher : MonoBehaviour
    {
        public delegate void Refresh();
        public event Refresh OnSolved;
        public event Refresh OnExitted;

        public char[] debugSolution;

        [SerializeField]
        private GameObject[] cylinders;
        /// <summary>
        /// Letter index of the cylinder.
        /// </summary>
        private int[] cylindersIndex;
        /// <summary>
        /// Index of cylinder which is active to rotate.
        /// </summary>
        private int indexCylinder;
        /// <summary>
        /// Up and down arrows' game object.
        /// </summary>
        [SerializeField]
        private GameObject UpDownGo;
        private float[] upDownYs = new float[6] { -5.2f, -3.94f, -2.72f, -1.54f, -0.39f, 0.72f };
        /// <summary>
        /// Letter indexes array.
        /// </summary>
        private char[] letterIndexes = new char[26] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private float degreesStep = 360f / 26f;
        [SerializeField]
        private char[] solution = new char[6] {'J', 'I', 'G', 'S', 'A', 'W'};
        /// <summary>
        /// Transition of the cylinder to rotate.
        /// </summary>
        [SerializeField]
        private float transitionDuration = 0.3f;
        /// <summary>
        /// Delay to check if the new set is correct.
        /// </summary>
        [SerializeField]
        private float checkDelay = 0.5f;

        /// <summary>
        /// Position of the camera when this minigame is played.
        /// </summary>
        [SerializeField]
        private GameObject camPlaceHolder;
        /// <summary>
        /// Metal of the lock to unlock when the puzzle is solved.
        /// </summary>
        [SerializeField]
        private GameObject metal;

        /// <summary>
        /// When true the mini game is active.
        /// </summary>
        public bool IsActive;
        /// <summary>
        /// When true this mini game is solved.
        /// </summary>
        private bool isSolved;

        public bool IsSolved
        {
            get { return isSolved; }
        }

        public GameObject CamPlaceHolder
        {
            get { return camPlaceHolder; }
        }

        void Awake()
        {
            IsActive = false;

            cylindersIndex = new int[6];
            for (int i = 0; i < cylindersIndex.Length; i++)
                cylindersIndex[i] = 0;

            indexCylinder = 0;
        }

        void Update()
        {
            if(IsActive && !isSolved)
            {
                checkInput();
            }
        }

        private void checkInput()
        {
            if(Input.GetKeyUp(KeyCode.UpArrow))
            {
                cylindersIndex[indexCylinder]++;
                if (cylindersIndex[indexCylinder] >= letterIndexes.Length)
                    cylindersIndex[indexCylinder] = 0;
                LeanTween.rotateY(cylinders[indexCylinder], cylindersIndex[indexCylinder] * degreesStep, transitionDuration);
                CancelInvoke("checkIfCorrect");
                Invoke("checkIfCorrect", checkDelay);
            }
            else if(Input.GetKeyUp(KeyCode.DownArrow))
            {
                cylindersIndex[indexCylinder]--;
                if (cylindersIndex[indexCylinder] < 0)
                    cylindersIndex[indexCylinder] = letterIndexes.Length - 1;
                LeanTween.rotateY(cylinders[indexCylinder], cylindersIndex[indexCylinder] * degreesStep, transitionDuration);
                CancelInvoke("checkIfCorrect");
                Invoke("checkIfCorrect", checkDelay);
            }

            if(Input.GetKeyUp(KeyCode.LeftArrow))
            {
                indexCylinder--;
                if (indexCylinder < 0)
                    indexCylinder = cylinders.Length - 1;
                moveUpDownGo();
            }
            else if(Input.GetKeyUp(KeyCode.RightArrow))
            {
                indexCylinder++;
                if (indexCylinder >= cylinders.Length)
                    indexCylinder = 0;
                moveUpDownGo();
            }

            if(Input.GetKeyUp(KeyCode.Escape))
            {
                if (OnExitted != null)
                    OnExitted();
            }
        }

        /// <summary>
        /// Checks if the current set is correct.
        /// </summary>
        /// <returns>True if puzzle is solved. False if not.</returns>
        private void checkIfCorrect()
        {
            char[] charsTry = getLetters();
            debugSolution = charsTry;
            for(int i = 0;i<solution.Length;i++)
            {
                if (solution[i] != charsTry[i])
                    return; // break the function
            }

            // If all correct
            solved();
        }

        private void solved()
        {
            isSolved = true;

            Vector3 localPos = metal.transform.localPosition;
            localPos.y += 1.8f;
            metal.transform.localPosition = localPos;

            GetComponent<CapsuleCollider>().isTrigger = false;
            GetComponent<Rigidbody>().useGravity = true;

            if (OnSolved != null)
                OnSolved();
        }

        private void moveUpDownGo()
        {
            Vector3 localPos = UpDownGo.transform.localPosition;
            localPos.y = upDownYs[indexCylinder];
            UpDownGo.transform.localPosition = localPos;
        }

        /// <summary>
        /// Returns the letters selected in the lock.
        /// </summary>
        /// <returns></returns>
        private char[] getLetters()
        {
            char[] charTry = new char[6];
            for(int i = 0;i<cylindersIndex.Length;i++)
                charTry[i] = letterIndexes[cylindersIndex[i]];

            return charTry;
        }
    }
}