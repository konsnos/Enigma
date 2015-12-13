using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Enigma.Menu
{
    public class Menu : MonoBehaviour
    {
        void Awake()
        {
            Cursor.visible = true;
        }

        public void GoToLevel()
        {
            SceneManager.LoadScene("InsideSub_2");
        }

        public void ExitApp()
        {
            Application.Quit();
        }
    }
}