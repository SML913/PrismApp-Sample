﻿using System.Windows;
using Microsoft.Practices.Prism.Mvvm;
using Model;
using UI.ViewModels;

namespace UI.Wrappers
{
   public class CompanyWrapper:ViewModelBase
    {
        

        public CompanyWrapper(Company model)
        {
            Model = model;
        }
        public Company Model {get;}

        public int Id
        {
            get
            {
                return Model.Id;
            }
        }
        public string Name {
            get { return Model?.Name; }
            set
            {
               
                Model.Name = value; 
                OnPropertyChanged();
            }
            
            
        }
    }
}