using UnityEngine;
using System.Collections;
using Enigma.UserInterface;

namespace Enigma.MiniGames
{
    public class Levers : MiniGameBase
    {
        public static Levers Singleton;

        [SerializeField]
        private LeverHandler[] levers;
        [SerializeField]
        private AudioSource leversSolvedSnd;
        [SerializeField]
        private DrawerHandler shelfUnlocked;

        void Awake()
        {
            Singleton = this;
        }

        public bool checkIfSolved()
        {
            foreach(LeverHandler lever in levers)
            {
                if(!lever.IsCorrect())
                    return false;
            }

            foreach (LeverHandler lever in levers)
                lever.MakeSolved();

            shelfUnlocked.Unlock();
            leversSolvedSnd.Play();
            UIHandler.Singleton.ShowPopUp("What was that sound?", null);
            return true;
        }
    }
}