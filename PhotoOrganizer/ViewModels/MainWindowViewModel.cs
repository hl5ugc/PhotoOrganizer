using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Windows.Storage;

namespace PhotoOrganizer.ViewModels;

 
public partial class MainWindowViewModel  : ObservableObject
{
    [ObservableProperty]
    private StorageFolder? _inputFolder;
    [ObservableProperty]
    private StorageFolder? _outputFolder;

    public MainWindowViewModel()
    {

    }

    [RelayCommand]
    private async Task UpdateInputFolderPath(string? folderPath)
    {
        if (folderPath is not null)
        {
            StorageFolder? folder = await StorageFolder.GetFolderFromPathAsync(folderPath);
            if (folder is not null)
            {
                InputFolder = folder;
            }
        }
    }
    [RelayCommand]
    private async Task UpdateOutputFolderPath(string? folderPath)
    {
        if (folderPath is not null)
        {
            StorageFolder? folder = await StorageFolder.GetFolderFromPathAsync(folderPath);
            if (folder is not null)
            {
                OutputFolder = folder;
            }
        }
    }
}
