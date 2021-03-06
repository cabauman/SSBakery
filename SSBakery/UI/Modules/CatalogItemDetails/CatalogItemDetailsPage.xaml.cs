﻿using System;
using System.Reactive.Linq;
using ReactiveUI;
using SSBakery.UI.Common;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSBakery.UI.Modules
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CatalogItemDetailsPage : ContentPageBase<ICatalogItemDetailsViewModel>
    {
        public CatalogItemDetailsPage()
        {
            InitializeComponent();

            this.WhenAnyValue(x => x.ViewModel)
                .Where(x => x != null)
                .Do(PopulateFromViewModel)
                .Take(1)
                .Subscribe();
        }

        private void PopulateFromViewModel(ICatalogItemDetailsViewModel catalogItem)
        {
            NameLabel.Text = catalogItem.Name;
            DescriptionLabel.Text = catalogItem.Description;
            PriceLabel.Text = catalogItem.Price;
            Image.Source = ImageSource.FromFile("Icon.png"); // catalogItem.ImageUrl;
        }
    }
}
