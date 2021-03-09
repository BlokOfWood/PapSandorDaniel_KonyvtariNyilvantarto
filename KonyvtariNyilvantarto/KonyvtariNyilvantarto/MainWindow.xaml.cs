using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Data;

namespace KonyvtariNyilvantarto
{
    public class Book
    {
        uint _ID;
        public uint ID { get => _ID; }

        string _author;
        public string Author { get => _author; }

        string _title;
        public string Title { get => _title; }

        string _releaseYear;
        public string ReleaseYear { get => _releaseYear; }

        string _publisher;
        public string Publisher { get => _publisher; }

        public bool IsBorrowable { get => _isBorrowable; }
        bool _isBorrowable;

        public Book(string line)
        {
            string[] separatedLine = line.Split(';');

            _ID = Convert.ToUInt32(separatedLine[0]);
            _author = separatedLine[1];
            _title = separatedLine[2];
            _releaseYear = separatedLine[3];
            _publisher = separatedLine[4];
            _isBorrowable = Convert.ToBoolean(separatedLine[5]);
        }
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

        public List<Book> Books = new List<Book>();
        public List<Tag> Tagok = new List<Tag>();
        public List<Kölcsönzés> Kölcsönzések = new List<Kölcsönzés>();

        public MainWindow()
        {
            InitializeComponent();

            OpenFileDialog fileDialog = new OpenFileDialog()
            {
                Filter = "Könyvtár Állományok (*.txt)|*.txt",
                RestoreDirectory = true
            };

            fileDialog.Title = "Válaszd ki a könyv állomány helyét";
            fileDialog.ShowDialog();
            PathsToData[0] = fileDialog.FileName;
/*
            fileDialog.Title = "Válaszd ki a tag állomány helyét";
            fileDialog.ShowDialog();
            PathsToData[1] = fileDialog.FileName;

            fileDialog.Title = "Válaszd ki a kölcsönzés állomány helyét";
            fileDialog.ShowDialog();
            PathsToData[2] = fileDialog.FileName;*/

            string[] input = File.ReadAllLines(PathsToData[0]);

            foreach (string i in input)
            {
                Books.Add(new Book(i));
            }

            BookDataGrid.ItemsSource = Books;
        }

        private void BookDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            FillFields(BookDataGrid.SelectedIndex);
        }

        void FillFields(int index)
        {
            if(!AuthorField.IsEnabled)
            {
                AuthorField.IsEnabled = true;
                TitleField.IsEnabled = true;
                ReleaseYearField.IsEnabled = true;
                PublisherField.IsEnabled = true;
                BorrowableCheck.IsEnabled = true;
            }


            IDField.Text = Books[index].ID.ToString();
            AuthorField.Text = Books[index].Author;
            TitleField.Text = Books[index].Title;
            ReleaseYearField.Text = Books[index].ReleaseYear;
            PublisherField.Text = Books[index].Publisher;
            BorrowableCheck.IsChecked = Books[index].IsBorrowable;
        }
    }
}
