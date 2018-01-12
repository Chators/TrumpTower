using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());



            OpenFileDialog openFileDialog = new OpenFileDialog();
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog Browser = new Microsoft.Win32.OpenFileDialog();
            //Titre qui se trouveras en dans la barre de titre de la fenêtre de dialogue 
            Browser.Title = "Fenetre de recherche de fichier";
            //Fichier par defaut (si tu veux en mettre un)
            Browser.FileName = "monfichier.txt";
            //L'extension par defaut des type de fichier recherchés
            Browser.DefaultExt = ".txt";
            //Filtre d'extension (si tu recherches un fichier bien spécifique
            Browser.Filter = "Mon fichier test (monfichier.txt)|monfichier.txt";

            // Ça ouvre la fenêtre de dialogue en renvoyant un booleen
            Nullable<bool> result = Browser.ShowDialog();

            // Vérifies si il n'y a aucun problème à l'ouverture (ou à la sélection du fichier)
            if (result == true)
            {
                // Ouverture du fichier
                string filename = Browser.FileName;
                //Affichage du fichier
                MessageBox.Show(filename);
            }
        }
    }
}
