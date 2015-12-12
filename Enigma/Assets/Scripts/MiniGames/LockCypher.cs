using UnityEngine;
using System.Collections;

namespace Enigma.MiniGames
{
    public class LockCypher : MiniGameBase
    {
        public event Refresh OnExitted;
        public event Refresh OnSolved;

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
        private float[] upDownYs = new float[6] { 1.76f, 2.8f, 4.05f, 5.18f, 6.34f, 7.51f };
        private int[] cylinderLength = new int[6] { 13, 13, 13, 12, 13, 13 };
        private int[] cylinderStartIndex = new int[6] { 0, 0, 0, 13, 0, 13 };
        private float[] cylinderDegreesStep;
        /// <summary>
        /// Letter indexes array.
        /// </summary>
        private char[] letterIndexes = new char[26] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        //private float degreesStep = 360f / 26f;
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

            cylinderDegreesStep = new float[6] { 360f / cylinderLength[0], 360f / cylinderLength[1], 360f / cylinderLength[2], 360f / cylinderLength[3], 360f / cylinderLength[4], 360f / cylinderLength[5] };
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
                if (cylindersIndex[indexCylinder] >= cylinderLength[indexCylinder])
                    cylindersIndex[indexCylinder] = 0;
                Vector3 localRotate = cylinders[indexCylinder].transform.localRotation.eulerAngles;
                localRotate.y = cylindersIndex[indexCylinder] * cylinderDegreesStep[indexCylinder];
                LeanTween.rotateLocal(cylinders[indexCylinder], localRotate, transitionDuration);
                CancelInvoke("checkIfCorrect");
                Invoke("checkIfCorrect", checkDelay);
            }
            else if(Input.GetKeyUp(KeyCode.DownArrow))
            {
                cylindersIndex[indexCylinder]--;
                if (cylindersIndex[indexCylinder] < 0)
                    cylindersIndex[indexCylinder] = cylinderLength[indexCylinder] - 1;
                Vector3 localRotate = cylinders[indexCylinder].transform.localRotation.eulerAngles;
                localRotate.y = cylindersIndex[indexCylinder] * cylinderDegreesStep[indexCylinder];
                LeanTween.rotateLocal(cylinders[indexCylinder], localRotate, transitionDuration);
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
                charTry[i] = letterIndexes[cylinderStartIndex[i] + cylindersIndex[i]];

            return charTry;
        }
    }
}