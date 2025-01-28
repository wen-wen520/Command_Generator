using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Command_Generator
{
    public sealed partial class SelectEffectsPage : Page
    {
        public ObservableCollection<Effect> AllEffects { get; set; }
        public ObservableCollection<Effect> SelectedEffects { get; set; }

        public SelectEffectsPage()
        {
            this.InitializeComponent();
            AllEffects = new ObservableCollection<Effect>();
            SelectedEffects = new ObservableCollection<Effect>();

            LoadAllEffects();
            AllEffectsListView.ItemsSource = AllEffects;
            SelectedEffectsListView.ItemsSource = SelectedEffects;
        }

        private class EffectData
        {
            public List<Effect> Effects { get; set; }
        }

        private async void LoadAllEffects()
        {
            Uri uri = new Uri("ms-appx:///Assets/Effects List.json");
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            var json = await FileIO.ReadTextAsync(file);
            var data = JsonConvert.DeserializeObject<EffectData>(json);
            data.Effects.ForEach(Effect => AllEffects.Add(Effect));
            AllEffectsListView.ItemsSource = AllEffects;

        }
            private void AddSelectedEffects_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var selectedItems = AllEffectsListView.SelectedItems;
            foreach (Effect effect in selectedItems)
            {
                SelectedEffects.Add(effect);
            }
        }

        private void RemoveSelectedEffects_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var selectedItems = SelectedEffectsListView.SelectedItems;
            foreach (Effect effect in selectedItems)
            {
                SelectedEffects.Remove(effect);
            }
        }

        private void NextPage_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // 传递选择的效果到下一个页面
            Frame.Navigate(typeof(UI.Pages.UnitEffectPage), SelectedEffects);
        }
    }
}
