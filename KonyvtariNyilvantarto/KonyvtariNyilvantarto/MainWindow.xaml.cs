using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace KonyvtariNyilvantarto
{
    public class Book
    {
        uint _ID;
        public uint ID { get => _ID; }

        string _author;
        public string Szerző { get => _author; }

        string _title;
        public string Cím { get => _title; }

        string _releaseYear;
        public string KiadásÉve { get => _releaseYear; }

        string _publisher;
        public string Kiadó { get => _publisher; }

        public bool Kölcsönözhető { get => _isBorrowable; }
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

        public BindingList<Book> Books = new BindingList<Book>();
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

            fileDialog.Title = "Válaszd ki a tag állomány helyét";
            fileDialog.ShowDialog();
            PathsToData[1] = fileDialog.FileName;

            /*fileDialog.Title = "Válaszd ki a kölcsönzés állomány helyét";
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
            BookFillFields(BookDataGrid.SelectedIndex);
        }

        void BookFillFields(int index)
        {
            if (index == -1) return;

            if(!AuthorField.IsEnabled)
            {
                AuthorField.IsEnabled = true;
                TitleField.IsEnabled = true;
                ReleaseYearField.IsEnabled = true;
                PublisherField.IsEnabled = true;
                BorrowableCheck.IsEnabled = true;
            }


            IDField.Text = Books[index].ID.ToString();
            AuthorField.Text = Books[index].Szerző;
            TitleField.Text = Books[index].Cím;
            ReleaseYearField.Text = Books[index].KiadásÉve;
            PublisherField.Text = Books[index].Kiadó;
            BorrowableCheck.IsChecked = Books[index].Kölcsönözhető;
        }

        private void NewBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (!AuthorField.IsEnabled) //Mivel egyszerre tesszük módosíthatóvá a mezőket ezért egyet elég leellenőrizni hogy engedélyezve van-e
            {
                AuthorField.IsEnabled = true;
                TitleField.IsEnabled = true;
                ReleaseYearField.IsEnabled = true;
                PublisherField.IsEnabled = true;
                BorrowableCheck.IsEnabled = true;
            }

            IDField.Text = (Books[Books.Count - 1].ID + 1).ToString();
            AuthorField.Text = "";
            TitleField.Text = "";
            ReleaseYearField.Text = "";
            PublisherField.Text = "";
            BorrowableCheck.IsChecked = false;
        }

        private void BookSaveButton_Click(object sender, RoutedEventArgs e)
        {
            int bookEntryID = Books.ToList().FindIndex(x => x.ID == int.Parse(IDField.Text));

            string borrowableString = BorrowableCheck.IsChecked.ToString().ToUpper()[0] + BorrowableCheck.IsChecked.ToString().Substring(1);
            string newLine = $"\n{IDField.Text};{AuthorField.Text};{TitleField.Text};{ReleaseYearField.Text};{PublisherField.Text};{borrowableString}";
            if (bookEntryID == -1)
            {
                File.AppendAllText(PathsToData[0], newLine);
                Books.Add(new Book(newLine));
            }
            else
            {
                Books[bookEntryID] = new Book(newLine);
            }

        }

        private void BookDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            int bookEntryID = Books.ToList().FindIndex(x => x.ID == int.Parse(IDField.Text));
            if(bookEntryID != -1)
            {
                Books.RemoveAt(bookEntryID);
                List<string> currentFile = File.ReadAllLines(PathsToData[0]).ToList();
                currentFile.RemoveAt(bookEntryID);
                File.WriteAllLines(PathsToData[0], currentFile);
            }
        }
    }
}
