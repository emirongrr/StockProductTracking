﻿using StockProductTracking.Core;
using StockProductTracking.MVVM.Model;
using StockProductTracking.Utils;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace StockProductTracking.MVVM.ViewModel
{
    internal class AddOrderPageViewModel : OrderViewModelBase
    {
        public ICommand AddOrderCommand { get; }

        private ObservableCollection<Customer> customers;
        public ObservableCollection<Customer> Customers
        {
            get => customers;
            set
            {
                customers = value;
                OnPropertyChanged(nameof(Customers));
            }
        }
        private ObservableCollection<Product> products;
        public ObservableCollection<Product> Products
        {
            get => products;
            set
            {
                products = value;
                OnPropertyChanged(nameof(Products));
            }
        }
        public AddOrderPageViewModel(MainViewModel mainViewModel)
        {
            Connect connect = new Connect();
            Customers = connect.GetCustomers();
            Products = connect.GetProducts();

            AddOrderCommand = new RelayCommand(o =>
            {
                  IsEnable = true;
                  Connect db = new Connect();
                  db.AddOrder(CustomerId, OrderProductTitle, OrderProductCount, OrderStatus);
                  mainViewModel.OrderVM.UpdateOrderList();
                  mainViewModel.CurrentView = mainViewModel.OrderVM;
            },
            canExecute => (CustomerId != 0 && IsEnable));
        }
    }
}
