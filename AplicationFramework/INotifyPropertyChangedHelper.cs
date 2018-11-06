/*
 * The following code is Copyright 2018 Dr Warren Creemers (busyDuckman)
 * See LICENSE.md for more information.
 */

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WDToolbox.AplicationFramework
{
    /// <summary>
    ///  Based on some ideas at http://stackoverflow.com/questions/1315621/implementing-inotifypropertychanged-does-a-better-way-exist
    /// </summary>
    /// <example>
    ///    INotifyPropertyChangedHelper notifyHelper = new INotifyPropertyChangedHelper();
    ///    private string foo;
    ///    public string Foo
    ///    {
    ///      set { notifyHelper.SetField(ref foo, value); }
    ///    }
    ///     
    ///    protected void propertyChanged(bool resultsInNewMaze = false, [CallerMemberName] string propertyName = null)
    ///    {
    ///      if (resultsInNewMaze)
    ///      {
    ///        Generate(Seed);
    ///      }
    ///      if (notifyHelper == null)
    ///      {
    ///        notifyHelper = new INotifyPropertyChangedHelper();
    ///        notifyHelper.PropertyChanged += (S, E) => {if(this.PropertyChanged != null) { this.PropertyChanged(S, E);}};
    ///      }
    ///      notifyHelper.Raise(propertyName);
    ///    } 
    /// 
    public sealed class INotifyPropertyChangedHelper : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            field = value;
            OnPropertyChanged(propertyName);
        }

        public void Raise([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(propertyName);
        }
    }

}
