using System.ComponentModel;

namespace Mapster.ClientApplication;

public class DataModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private string _data;
    public string Data
    {
        get { return _data; }
        set
        {
            if (_data != value)
            {
                _data = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Data)));
            }
        }
    }

    public DataModel(string initialValue = "Default value")
    {
        _data = initialValue;
    }
}
