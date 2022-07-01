using GalaSoft.MvvmLight.Command;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Semaphore.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        private string numbercount;

        public string NumberCount
        {
            get { return numbercount; }
            set { numbercount = value; OnPropertyChanged();}
        }


        private Thread selected_create_theead;

        public Thread Selected_create_theead
        {
            get { return selected_create_theead; }
            set { selected_create_theead = value; OnPropertyChanged(); }
        }
        private Thread selected_wait_theead;

        public Thread Selected_wait_theead
        {
            get { return selected_wait_theead; }
            set { selected_wait_theead = value; OnPropertyChanged(); }
        }
        static System.Threading.Semaphore semaphore = new System.Threading.Semaphore(2, 2);


        public ObservableCollection<Thread> createthreads { get; set; }=new ObservableCollection<Thread>();
        public ObservableCollection<Thread> waitthreads { get; set; }=new ObservableCollection<Thread>();
        public ObservableCollection<Thread> workingthreads { get; set; }=new ObservableCollection<Thread>();
        private  void SomeMethod(object state)
        {
           Thread threadcopy = new Thread(SomeMethod) ;
            bool st = false;
            while (!st)
            {
                if (semaphore.WaitOne(1000))
                {
                    try
                    {
                        Thread.Sleep(2000);
                    }
                    finally
                    {
                        if (waitthreads.Count()!=0)
                        {
                            dispatcher.Invoke(() => workingthreads.Add(waitthreads[0]));
                            threadcopy=waitthreads[0];
                            dispatcher.Invoke(() => waitthreads.Remove(waitthreads[0]));
                           
                            st = true;
                            Thread.Sleep(8000);
                            dispatcher.Invoke(() => workingthreads.Remove(threadcopy));
                            
                            semaphore.Release();

                        }
                       
                    }
                }
                else
                {
                }
            }
           
            
         
            
            //workingthreads.Remove();
        }
        private Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

        public RelayCommand Create
        {
            get => new RelayCommand(() =>
            {
                Thread current = new Thread(SomeMethod);
                //ThreadPool.CurrentThread.Name=$"Thread--{nomre}";
                current.Name=$"Thread--{nomre}";
                createthreads.Add(current);
                nomre++;
               
            });
        }
        private void OnMouseDoubleClick(object obj)
        {
            Selected_create_theead= obj as Thread;
            Selected_create_theead.Start();
            waitthreads.Add(Selected_create_theead);
            createthreads.Remove(Selected_create_theead);


               
            
            


        }

        private void OnMouseDoubleClick_2(object obj)
        {
            Selected_wait_theead= obj as Thread;
            Selected_wait_theead.Abort();
            waitthreads.Remove(Selected_wait_theead);

        }
        public RelayCommand<object> double_button_1 { get; set; }
        public RelayCommand<object> double_button_2 { get; set; }



        public int nomre { get; set; } = 1;
        public MainViewModel() 
        {
            double_button_1=new RelayCommand<object>(OnMouseDoubleClick);
            double_button_2=new RelayCommand<object>(OnMouseDoubleClick_2);
            NumberCount ="8";
            semaphore = new System.Threading.Semaphore(int.Parse(NumberCount), int.Parse(NumberCount));
        }
    }
}
