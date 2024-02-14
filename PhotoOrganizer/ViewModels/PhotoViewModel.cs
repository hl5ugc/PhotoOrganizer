using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage;

namespace PhotoOrganizer.ViewModels;

public partial class PhotoViewModel : ObservableObject
{
    #region Fields
    private readonly StorageFile _file;

    [ObservableProperty]
    private BitmapImage? _thumbNail;
    [ObservableProperty]
    private string? _inputFileName;
    [ObservableProperty]
    private string? _inputFilePath;
    [ObservableProperty]
    private string? _filseSize;
    [ObservableProperty]
    private DateTime? _dateTaken;
    [ObservableProperty]
    private string? _outputFilePath;
    #endregion

    public PhotoViewModel(StorageFile file)
    {
        _file = file;
        InputFileName = _file.Name;
        InputFilePath = _file.Path.ToString();

    }
}
