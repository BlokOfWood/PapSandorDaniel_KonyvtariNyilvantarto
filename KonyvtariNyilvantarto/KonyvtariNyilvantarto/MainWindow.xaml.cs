using Microsoft.Win32;
using System;
using System.Windows;

namespace KonyvtariNyilvantarto
{
    public class Könyv
    {
        public uint ID;
        public string Szerző;
        public string Cím;
        public string KiadásÉve; //Stringként egyszerűbb kezelni és nincsen funkció amihez szükséges hogy DateTime vagy int legyen.
        public string Kiadó;
        public bool Kölcsönözhető;
    }

    public class Tag
    {
        public uint ID;
        public string Név;
        public string Lakcím;
    }

    public class Kölcsönzés
    {
        public uint TagID;
        public uint KönyvID;
        public DateTime Dátum;
    }

    public partial class MainWindow : Window
    {
        /*
         * Index 0: Könyv állomány
         * Index 1: Tag Állomány
         * Index 2: Kölcsönzés Állomány
         */
        public string[] PathsToData = new string[3];

        public MainWindow()
        {
            InitializeComponent();

            OpenFileDialog fileDialog = new OpenFileDialog()
            {
                Filter = "txt files (*.txt)|*.txt",
                RestoreDirectory = true
            };

            fileDialog.Title = "Válaszd ki a könyv állomány helyét";
            fileDialog.ShowDialog();
            PathsToData[0] = fileDialog.FileName;

            fileDialog.Title = "Válaszd ki a tag állomány helyét";
            fileDialog.ShowDialog();
            PathsToData[1] = fileDialog.FileName;

            fileDialog.Title = "Válaszd ki a kölcsönzés állomány helyét";
            fileDialog.ShowDialog();
            PathsToData[2] = fileDialog.FileName;
        }
    }
}
