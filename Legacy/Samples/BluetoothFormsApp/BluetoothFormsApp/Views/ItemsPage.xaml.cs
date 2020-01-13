using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using BluetoothFormsApp.Models;
using BluetoothFormsApp.Views;
using BluetoothFormsApp.ViewModels;
using InTheHand.Devices.Enumeration;
using InTheHand.Devices.Bluetooth;
using InTheHand.Devices.Bluetooth.GenericAttributeProfile;

namespace BluetoothFormsApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ItemsPage : ContentPage
	{
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

           
            BindingContext = viewModel = new ItemsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void Search_Clicked(object sender, EventArgs e)
        {
            foreach (DeviceInformation di in await DeviceInformation.FindAllAsync(GattDeviceService.GetDeviceSelectorFromUuid(BluetoothUuidHelper.FromShortId(0xfff0))))
            {
                System.Diagnostics.Debug.WriteLine(di);
            }
        }

            async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}