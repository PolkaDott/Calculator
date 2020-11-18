using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace calculator
{
    class MainViewModel : INotifyPropertyChanged
    {
        private string expression;
        public MainViewModel()
        {
            
        }
        private ICommand addChar;
        public ICommand AddChar => addChar ?? new RelayCommand<string>(
            (x) => MessageBox.Show(x),
            (x) => true);
        
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


        
    }
}