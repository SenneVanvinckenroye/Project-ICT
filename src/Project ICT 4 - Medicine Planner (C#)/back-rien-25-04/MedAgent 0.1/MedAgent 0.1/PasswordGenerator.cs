using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MedAgent_0_1
{
    public class PasswordGenerator
    {
        protected Random rGen;
        protected string[] strCharacters = { "A","B","C","D","E","F","G",
"H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y",
"Z","1","2","3","4","5","6","7","8","9","0","a","b","c","d","e","f","g","h",
"i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z"};
        public PasswordGenerator()
        {
            rGen = new Random();
        }

        public string GenPassWithCap(int i)
        {
            rGen = new Random();
            int p = 0;
            string strPass = "";
            for (int x = 0; x <= i; x++)
            {
                p = rGen.Next(0, 60);
                strPass += strCharacters[p];
            }
            return strPass;
        }
    }
    
}
