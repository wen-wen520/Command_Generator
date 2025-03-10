using Command_Generator.UI.Pages;
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
using Command_Generator.Core.Unit;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Command_Generator.UI.Pages
{
    public sealed partial class UnitEffectPage : Page
    {
        // Attributes
        private custom_effect editing_unit = null;
        // Constructor
        public UnitEffectPage()
        {
            this.InitializeComponent();
        }

        // Utility Functions
        private void has_editor_open()
        {
            EditorCommandBar.IsEnabled = true;
            EditorContent.Visibility = Visibility.Visible;
        }
        // Selector

        // List
        public async void LoadSelectorList()
        {
            string[] filesNames = await JsonFileIO.GetFilesName("user_costume/units");
            SelectorFileList.ItemsSource = filesNames;
        }
        private void LoadEditorContent(string file_name)
        {
            editing_unit = new custom_effect(file_name);
            EditorContentID.Text = editing_unit.id;
            EditorContentLevel.Text = (editing_unit.amplifier + 1).ToString();
            EditorContentDuration.Text = editing_unit.duration.ToString();
            EditorContentAmbient.IsChecked = editing_unit.ambient;
            EditorContentShowParticles.IsChecked = editing_unit.show_particles;
            EditorContentShowIcon.IsChecked = editing_unit.show_icon;
        }
        // Selector Button Click Events
        private void SelectorAddClick(object sender, RoutedEventArgs e)
        {
            has_editor_open();
            Services.ToastNotificationHelper.SendNotification("Add", "Add Unit Effect Clicked");
        }
        private void SelectorEditClick(object sender, RoutedEventArgs e)
        {
            has_editor_open();
            Services.ToastNotificationHelper.SendNotification("Edit", "Edit Unit Effect Clicked");
        }
        private void SelectorDeleteClick(object sender, RoutedEventArgs e)
        {
            Services.ToastNotificationHelper.SendNotification("Delete", "Delete Unit Effect Clicked");
        }
        private void SelectorImportClick(object sender, RoutedEventArgs e)
        {
            Services.ToastNotificationHelper.SendNotification("Import", "Import Unit Effect Clicked");
        }
        private void SelectorExportClick(object sender, RoutedEventArgs e)
        {
            Services.ToastNotificationHelper.SendNotification("Export", "Export Unit Effect Clicked");
        }

        // Editor Button Click Events
        private void EditorButtonNewClick(object sender, RoutedEventArgs e)
        {
            has_editor_open();
            Services.ToastNotificationHelper.SendNotification("New", "New Unit Effect Clicked");
        }
        private void EditorButtonRenameClick(object sender, RoutedEventArgs e)
        {
            Services.ToastNotificationHelper.SendNotification("Rename", "Rename Unit Effect Clicked");
        }
        private void EditorButtonSaveClick(object sender, RoutedEventArgs e)
        {
            Services.ToastNotificationHelper.SendNotification("Save", "Save Unit Effect Clicked");
        }
        private void EditorButtonSaveasClick(object sender, RoutedEventArgs e)
        {
            Services.ToastNotificationHelper.SendNotification("Save As", "Save As Unit Effect Clicked");
        }


        private void EditorButtonDeleteClick(object sender, RoutedEventArgs e)
        {
            EditorCommandBar.IsEnabled = false;
            EditorContent.Visibility = Visibility.Collapsed;
        }
        private void EditorButtonCloseClick(object sender, RoutedEventArgs e)
        {
            EditorCommandBar.IsEnabled = false;
            EditorContent.Visibility = Visibility.Collapsed;
        }
    }
}