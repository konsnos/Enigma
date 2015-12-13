

namespace Enigma.MiniGames
{
    public class ItemSlotIds
    {
        public enum Item { Flashlight = 0, Enigma_Buttons, Battery_Yellow, Battery_Blue, Battery_Red, Folder, Book_Blue };
        public static string[] Names = new string[7] { "Flashlight", "Enigma buttons", "Yellow battery", "Blue battery", "Red battery", "Folder", "Blue book" };

        public static string GetName(Item id)
        {
            return Names[(int)id];
        }
    }
}