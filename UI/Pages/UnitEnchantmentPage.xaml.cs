using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Windows.Storage;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using System.Threading.Tasks;
using Command_Generator.UI.Pages;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Command_Generator.UI.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public class Enchantment : INotifyPropertyChanged
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

        public string Name { get; set; }
        public string Version { get; set; }
        public string Item {  get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class EnchantmentData
    {
        public List<Enchantment> enchantments { get; set; }
    }


    public sealed partial class UnitEnchantmentPage : Page
    {

        public ObservableCollection<Enchantment> Enchantments { get; set; }

        public UnitEnchantmentPage()
        {
            this.InitializeComponent();
            Enchantments = new ObservableCollection<Enchantment>();
            LoadEnchantments();
        }


        private string GenerateEnchantmentsNBT()
        {
            var selectedEnchantments = EnchantmentsListView.SelectedItems.Cast<Enchantment>();
            var nbtBuilder = new StringBuilder();
            nbtBuilder.Append("{Enchantments:[");

            foreach (var enchantment in selectedEnchantments)
            {
                nbtBuilder.AppendFormat("{{id:\"{0}\",lvl:{1}b}},", enchantment.Id, enchantment.Lvl);
            }

            if (nbtBuilder.Length > 15) // Check if any enchantments were added
            {
                // Remove the last comma
                nbtBuilder.Remove(nbtBuilder.Length - 1, 1);
            }

            nbtBuilder.Append("]}");
            return nbtBuilder.ToString();
        }
        // 生成附魔


        private async void ShowCommandDialog_Click(object sender, RoutedEventArgs e)
        {
            if (EnchantmentsListView.SelectedItems.Count == 0)
            {
                //ToastNotificationsServices toastServices = new ToastNotificationsServices();
                //toastServices.ShowToastNotification("尚未选择任何附魔。" ,"" , "ms-appx:///Assets/Error.png");
                return;
            }

            string enchantmentsNBT = GenerateEnchantmentsNBT();

            ContentDialog commandDialog = new ContentDialog
            {
                Title = "生成的NBT",
                Content = new TextBlock
                {
                    Text = enchantmentsNBT,
                    TextWrapping = TextWrapping.Wrap
                },
                PrimaryButtonText = "确认并复制",
                PrimaryButtonStyle = (Style)Application.Current.Resources["AccentButtonStyle"],
                CloseButtonText = "取消"
            };

            commandDialog.PrimaryButtonClick += async (s, args) =>
            {
                var dataPackage = new DataPackage();
                dataPackage.SetText(enchantmentsNBT);
                Clipboard.SetContent(dataPackage);
                //ToastNotificationsServices toastServices = new ToastNotificationsServices();
                //toastServices.ShowToastNotification("已复制NBT: ", enchantmentsNBT, "ms-appx:///Assets/Clipboard.png");
            };

            await commandDialog.ShowAsync();
        }

        



        private void SomeMethodToHandleSelection()
        {
            var selectedEnchantments = EnchantmentsListView.SelectedItems;
            // 处理选中的附魔项
        }


        private async void LoadEnchantments()
        {
            Uri uri = new Uri("ms-appx:///Assets/Enchantments List.json");
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            var json = await Windows.Storage.FileIO.ReadTextAsync(file);
            var data = JsonConvert.DeserializeObject<EnchantmentData>(json);
            data.enchantments.ForEach(enchantment => Enchantments.Add(enchantment));
            EnchantmentsListView.ItemsSource = Enchantments;

            var groupedEnchantments = Enchantments.GroupBy(e => e.Item)
                                      .Select(g => new GroupInfoCollection<Enchantment>(g))
                                      .ToList();
            var cvs = (CollectionViewSource)this.Resources["GroupedEnchantments"];
            cvs.Source = groupedEnchantments;
            EnchantmentsListLoadingAnimation.Visibility = Visibility.Collapsed; // 隐藏加载动画
            GenerateNBT.IsEnabled = true;
        }

    }

    public class GroupInfoCollection<T> : List<T>
    {
        public object Key { get; set; }

        public GroupInfoCollection(IEnumerable<T> items)
        {
            Key = (items as IGrouping<object, T>).Key;
            AddRange(items);
        }
    }

}
