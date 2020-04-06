using System.ComponentModel;
using UMDataManagement;
using System.Runtime.CompilerServices;
namespace UMGS
{
    [System.Serializable]
    public class VirtualCurrency : INotifyPropertyChanged
    {
    
        public Currency Name;
        public bool IsAvailable => DataHandler.Contains(Name.ToString());

        public float value
        {
            get => DataHandler.Get<float> (Name.ToString ());
            set
            {
                DataHandler.Save (Name.ToString (), value);
                OnPropertyChanged ();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

       
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            
        }
    }
}