﻿using StockProductTracking.Core;
using StockProductTracking.Utils;
using System.Windows.Input;

namespace StockProductTracking.MVVM.ViewModel
{
    internal class AddEmployeePageViewModel : EmployeeViewModelBase
    {
        public ICommand AddEmployeeCommand { get; }

        public AddEmployeePageViewModel(MainViewModel mainViewModel)
        {
            AddEmployeeCommand = new RelayCommand(o =>
            {
                Connect db = new Connect();
                db.AddEmployee(EmployeeFirstName, EmployeeLastName, EmployeeUsername, EmployeePassword, EmployeeEmail, EmployeeIsAdmin,mainViewModel.CurrentUser.Username);
                mainViewModel.EmployeeVM.UpdateEmployeeList();
                mainViewModel.CurrentView = mainViewModel.EmployeeVM;
            },
            canExecute=>IsEnable);
        }
    }
}
