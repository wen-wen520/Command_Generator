using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Command_Generator
{
    public sealed partial class EditEffectsPage : Page
    {
        public ObservableCollection<Effect> EditableEffects { get; set; }

        public EditEffectsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            EditableEffects = e.Parameter as ObservableCollection<Effect>;
            EditableEffectsListView.ItemsSource = EditableEffects;
        }

        private void BackPage_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async void SaveEffects_Click(object sender, RoutedEventArgs e)
        {

            ContentDialog saveDialog = new ContentDialog
            {
                Title = "Save Custom Effects",
                Content = new TextBox { PlaceholderText = "Enter name for the custom effect" },

                PrimaryButtonText = "Save",
                CloseButtonText = "Cancel"
            };

            // Save edited effects to JSON file
            string effectSetName = EffectSetNameTextBox.Text;
            if (string.IsNullOrWhiteSpace(effectSetName))
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Effect set name cannot be empty.",
                    CloseButtonText = "Ok"
                };
                await errorDialog.ShowAsync();
                return;
            }

            var effectList = new
            {
                CustomEffectsList = new[]
                {
                    new
                    {
                        Name = effectSetName,
                        Effects = EditableEffects.Select(a => new
                        {
                            Id = a.Id,
                            Amplifier = a.Lvl - 1,
                            Duration = a.Duration * 20,
                            ShowParticles = a.Particle ? "1b" : "0b"
                        })
                    }
                }
            };

            string json = JsonConvert.SerializeObject(effectList, Formatting.Indented);
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await localFolder.CreateFileAsync("CustomEffectsList.json", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, json);

        }


        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            string effectsCommand = GenerateEffectsCommand();

            var dataPackage = new DataPackage();
            dataPackage.SetText(effectsCommand);
            Clipboard.SetContent(dataPackage);

            var toastService = new ToastNotificationsServices();
            toastService.ShowToastNotification("Command copied and custom effect saved.", effectsCommand, "ms-appx:///Assets/1.png");
        }

        private string GenerateEffectsCommand()
        {
            var nbtBuilder = new StringBuilder();
            nbtBuilder.Append("{CustomPotionEffects:[");

            foreach (var effect in EditableEffects)
            {
                nbtBuilder.AppendFormat("{{Id:{0},Amplifier:{1},Duration:{2},ShowParticles:{3}b}},",
                    effect.Id,
                    effect.Lvl - 1,
                    effect.Duration * 20,
                    effect.Particle ? "1" : "0");
            }

            if (nbtBuilder.Length > 25) // Check if any effects were added
            {
                nbtBuilder.Remove(nbtBuilder.Length - 1, 1); // Remove the last comma
            }

            nbtBuilder.Append("]}");
            return nbtBuilder.ToString();
        }

    }
}