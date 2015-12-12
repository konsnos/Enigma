using UnityEngine;
using System.Collections;

namespace Enigma.MiniGames
{
    public class MiniGameBase : MonoBehaviour
    {
        public delegate void Refresh();

        /// <summary>
        /// When true this mini game is solved.
        /// </summary>
        protected bool isSolved = false;

        public bool IsSolved
        {
            get { return isSolved; }
        }
    }
}