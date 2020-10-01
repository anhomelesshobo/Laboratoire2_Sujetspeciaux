using DogFetchApp.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ApiHelper.Processor;
using ApiHelper.Models;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Net.Cache;
using System.Windows.Controls;
using System.Resources;
using DogFetchApp.Properties;

namespace DogFetchApp.ViewModels
{
    class MainViewModel : BaseViewModel
    {

        private ResourceManager rm = new ResourceManager(typeof(Resources));
        
        private ObservableCollection<DogModels> races;
        private DogModels selectedbreedname;
        private DogModels currentbreedname;
        private ObservableCollection<NombrePhotos> numbers;
        private NombrePhotos selectednumber;
        private NombrePhotos currentnumber;
        private ObservableCollection<BitmapImage> dogimages;
        private string urilink;

        public ObservableCollection<DogModels> Races
        {
            get => races;
            set
            {
                races = value;
                OnPropertyChanged();
            }
        }

        public string UriLink
        {
            get => urilink;
            set
            {
                urilink = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<BitmapImage> DogImages
        {
            get => dogimages;
            set
            {
                dogimages = value;
                OnPropertyChanged();
            }
        }


        public NombrePhotos SelectedNumber
        {
            get => selectednumber;
            set
            {
                selectednumber = value;
                OnPropertyChanged();
            }
        }

        public DogModels SelectedBreedName
        {
            get => selectedbreedname;
            set
            {
                selectedbreedname = value;
                OnPropertyChanged();
            }
        }

        public NombrePhotos CurrentNumber
        {
            get => currentnumber;
            set
            {
                currentnumber = value;
                OnPropertyChanged();
            }
        }

        public DogModels CurrentBreedName
        {
            get => currentbreedname;
            set
            {
                currentbreedname = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<NombrePhotos> Numbers
        {
            get => numbers;
            set
            {
                numbers = value;
                OnPropertyChanged();
            }
        }


        public DelegateCommand<string> ChangeLanguages { get; private set; }

        public DelegateCommand<object> FetchButton { get; private set; }
        public DelegateCommand<object> Next { get; private set; }
        public DelegateCommand<object> LoadbreedList { get; private set; }



        public MainViewModel()
        {
            ChangeLanguages = new DelegateCommand<string>(ChangeLang);
            FetchButton = new DelegateCommand<object>(Fetch);
            LoadbreedList = new DelegateCommand<object>(LoadBreedImage);
            Next = new DelegateCommand<object>(NextImages);

            Races = new ObservableCollection<DogModels>();

            SelectedBreedName = new DogModels();
            SelectedNumber = new NombrePhotos();

            

            initvalue();
        }

        private void initvalue()
        {
            Numbers = new ObservableCollection<NombrePhotos>()
            {
                     new NombrePhotos(){Number = 1}
                    ,new NombrePhotos(){Number = 3}
                    ,new NombrePhotos(){Number = 5}
                    ,new NombrePhotos(){Number = 10}
            };
        }

        private async void LoadBreedImage(object sender)
        {
            var res = await DogApiProcessor.LoadBreedList();

            Races = new ObservableCollection<DogModels>(res);
        }

        private async void NextImages(object sender)
        {
            await LoadImage(CurrentNumber.Number);
        }

        private async void Fetch(object sender)
        {
            if (SelectedBreedName.Nom == null || SelectedNumber == null)
            {
                MessageBox.Show(Properties.Resources.MessageErrorBreedandNumber);
            }else
            {
                CurrentBreedName = SelectedBreedName;
                CurrentNumber = SelectedNumber;
               
                await LoadImage(SelectedNumber.Number);
                
            }
                       
        }

        private async Task LoadImage(int nbphotos)
        {
            DogImages = new ObservableCollection<BitmapImage>();

            for (int i = 0; i < nbphotos; i++)
            {
                var dogimage = await DogApiProcessor.GetImageUrl(CurrentBreedName.Nom);

                UriLink = dogimage.source;

                var uriSource = new Uri(dogimage.source, UriKind.Absolute);


                DogImages.Add(new BitmapImage(uriSource,
                    new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable)));
             
            }
           

        }

       

        private void ChangeLang(string sender)
        {

            MessageBoxResult result = MessageBox.Show(Properties.Resources.MessageChangeLanguage, "My App", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    ChangeLanguage(sender);
                    Restart();
                    break;
                case MessageBoxResult.No:
                    ChangeLanguage(sender);
                    break;

            }
        }


        private void ChangeLanguage(string param)
        {
            Properties.Settings.Default.Language = param;
            Properties.Settings.Default.Save();
        }

        private void Restart()
        {
            var filename = Application.ResourceAssembly.Location;
            var newFile = Path.ChangeExtension(filename, ".exe");
            Process.Start(newFile);
            Application.Current.Shutdown();
        }

     
    }
}
