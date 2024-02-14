using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.UI.Controls.TextToolbarSymbols;
using Windows.Storage;
using Windows.Storage.Search;

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
    private async Task LoadPhotosAsync(string? inputFolderPath,CancellationToken cancellationToken)
    {
        if (inputFolderPath is not null && LoadPhotosCommand.IsRunning is false)
        {
            StorageFolder? folder = await StorageFolder.GetFolderFromPathAsync(inputFolderPath);
            if(folder is not null)
            {
                List<string> fileTypeFilter = new() { ".jpg", ".jpeg", ".bmp", };
                QueryOptions queryOptions = new QueryOptions(CommonFileQuery.DefaultQuery,fileTypeFilter);
                queryOptions.FolderDepth = FolderDepth.Deep;
                StorageFileQueryResult? results = folder.CreateFileQueryWithOptions(queryOptions);
                IReadOnlyCollection<StorageFile>? files = await results.GetFilesAsync();

                if(files is not null)
                {

                }
            }
        }
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
