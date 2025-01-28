using Command_Generator.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace Command_Generator.UI.Pages
{
    public class Effect : INotifyPropertyChanged
    {
        private int _lvl;

        public string Id { get; set; }

        public int Lvl
        {
            get => _lvl;
            set
            {
                if (_lvl != value)
                {
                    _lvl = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _duration;
        public int Duration
        {
            get => _duration;
            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _particle;
        public bool Particle
        {
            get => _particle;
            set
            {
                if (_particle != value)
                {
                    _particle = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class EffectData
    {
        public List<Effect> Effects { get; set; }
    }


    public sealed partial class UnitEffectPage : Page
    {

        public ObservableCollection<Effect> Effects { get; set; }

        public UnitEffectPage()
        {
            this.InitializeComponent();
            Effects = new ObservableCollection<Effect>();
            LoadEffects();
        }

        // Handle text change and present suitable items
        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Since selecting an item will also change the text,
            // only listen to changes caused by user entering text.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suitableItems = new List<string>();
                var splitText = sender.Text.ToLower().Split(" ");
                foreach (var target in TargetServices.Targets)
                {
                    var found = splitText.All((key) =>
                    {
                        return target.ToLower().Contains(key);
                    });
                    if (found)
                    {
                        suitableItems.Add(target);
                    }
                }
                if (suitableItems.Count == 0)
                {
                    suitableItems.Add("No results found");
                }
                sender.ItemsSource = suitableItems;
            }
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            Target.Text = args.SelectedItem.ToString();
        }


        private string GenerateEffectsCommand()
        {
            string target = Target.Text;
            if (string.IsNullOrWhiteSpace(target))
            {
                target = "@p"; // 如果目标对象为空，则默认为@p（最近的玩家）
            }

            var selectedEffects = EffectsListView.SelectedItems.Cast<Effect>();
            var commandBuilder = new StringBuilder();

            // 获取用户选择的操作方式
            string commandType = CommandType.SelectedIndex == 0 ? "give" : "clear";

            if (commandType == "give")
            {
                commandBuilder.AppendFormat("/effect give {0} ", target);

                foreach (var Effect in selectedEffects)
                {
                    commandBuilder.AppendFormat("{0} {1} {2} {3} ",
                        Effect.Id,
                        Effect.Duration,
                        Effect.Lvl - 1, // Minecraft的放大器等级从0开始计数
                        Effect.Particle ? "false" : "true");
                }
            }
            else
            {
                commandBuilder.AppendFormat("/effect clear {0} ", target);

                foreach (var Effect in selectedEffects)
                {
                    commandBuilder.AppendFormat("{0} ", Effect.Id);
                }
            }

            // 移除最后一个空格
            if (commandBuilder.Length > 0)
            {
                commandBuilder.Remove(commandBuilder.Length - 1, 1);
            }

            return commandBuilder.ToString();
        }



        private async void GenerateCommand_Click(object sender, RoutedEventArgs e)
        {
            if (EffectsListView.SelectedItems.Count == 0)
            {
                //ToastNotificationsServices toastServices = new ToastNotificationsServices();
                //toastServices.ShowToastNotification("尚未选择效果。", "", "ms-appx:///Assets/Error.png");
                return;
            }

            string EffectsNBT = GenerateEffectsCommand();

            ContentDialog commandDialog = new ContentDialog
            {
                Title = "生成的命令",
                Content = new TextBlock
                {
                    Text = EffectsNBT,
                    TextWrapping = TextWrapping.Wrap
                },
                PrimaryButtonText = "确认并复制",
                PrimaryButtonStyle = (Style)Application.Current.Resources["AccentButtonStyle"],
                CloseButtonText = "取消"
            };

            commandDialog.PrimaryButtonClick += async (s, args) =>
            {
                var dataPackage = new DataPackage();
                dataPackage.SetText(EffectsNBT);
                Clipboard.SetContent(dataPackage);
                //ToastNotificationsServices toastServices = new ToastNotificationsServices();
                //toastServices.ShowToastNotification("已复制命令: ", EffectsNBT, "ms-appx:///Assets/Clipboard.png");
            };

            await commandDialog.ShowAsync();
        }



        private void EffectsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 如果选择了多个效果，禁用生成命令按钮
            GenerateCommandButton.IsEnabled = EffectsListView.SelectedItems.Count <= 1;
        }

        private void GeneratePotionNBT_Click(object sender, RoutedEventArgs e)
        {
            var selectedEffects = EffectsListView.SelectedItems.Cast<Effect>();

            var nbtBuilder = new StringBuilder();
            nbtBuilder.Append("{CustomPotionEffects:[");

            foreach (var Effect in selectedEffects)
            {
                nbtBuilder.AppendFormat("{{Id:{0},Amplifier:{1},Duration:{2},ShowParticles:{3}b}},",
                    Effect.Id,
                    Effect.Lvl - 1,
                    Effect.Duration * 20,
                    Effect.Particle ? "1" : "0");
            }

            if (nbtBuilder.Length > 25) // Check if any Effects were added
            {
                // Remove the last comma
                nbtBuilder.Remove(nbtBuilder.Length - 1, 1);
            }

            nbtBuilder.Append("]}");

            // 显示药水NBT标签
            ShowPotionNBTDialog(nbtBuilder.ToString());
        }

        private async void ShowPotionNBTDialog(string potionNBT)
        {
            ContentDialog potionNBTDialog = new ContentDialog
            {
                Title = "生成的药水NBT",
                Content = new TextBlock
                {
                    Text = potionNBT,
                    TextWrapping = TextWrapping.Wrap
                },
                PrimaryButtonText = "确认并复制",
                PrimaryButtonStyle = (Style)Application.Current.Resources["AccentButtonStyle"],
                CloseButtonText = "取消"
            };

            potionNBTDialog.PrimaryButtonClick += async (s, args) =>
            {
                var dataPackage = new DataPackage();
                dataPackage.SetText(potionNBT);
                Clipboard.SetContent(dataPackage);
                //ToastNotificationsServices toastServices = new ToastNotificationsServices();
                //toastServices.ShowToastNotification("已复制NBT: ", potionNBT, "ms-appx:///Assets/Clipboard.png");
            };

            await potionNBTDialog.ShowAsync();
        }


        private async void LoadEffects()
        {
            Uri uri = new Uri("ms-appx:///Assets/Effects List.json");
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            var json = await FileIO.ReadTextAsync(file);
            var data = JsonConvert.DeserializeObject<EffectData>(json);
            data.Effects.ForEach(Effect => Effects.Add(Effect));
            EffectsListView.ItemsSource = Effects;

            EffectsListLoadingAnimation.Visibility = Visibility.Collapsed;
            EffectsTitleBar.Visibility = Visibility.Visible;
            GenerateCommandButton.IsEnabled = true;
            GeneratePotionNBTButton.IsEnabled = true;
        }

    }
}