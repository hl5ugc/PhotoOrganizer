using System;
using System.Globalization;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PhotoOrganizer.ViewModels;
using Windows.Storage;
using Windows.Storage.Pickers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PhotoOrganizer;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    public StorageFolder? SelectedInputFolder
    {
        get;
        set;
    }
    public StorageFolder? SelectedOutputFolder
    {
        get;
        set;
    }
    public MainWindowViewModel? ViewModel
    {
        get;
    }

    public MainWindow()
    {
        this.InitializeComponent();

        #region Window Title Bar Modify
        Title = "Photo Organizer";
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(TitleBar);
        #endregion

        ViewModel = Ioc.Default.GetService<MainWindowViewModel>();

        updateOutputFolderExample();
    }

    private async void StartButton_Click(object sender, RoutedEventArgs e)
    {
        ContentDialogResult result = await StartSettingsDialog.ShowAsync();
        if (result is ContentDialogResult.Primary && ViewModel is not null)
        {
            ViewModel.UpdateInputFolderPathCommand?.Execute(SelectedInputFolder?.Path);
            ViewModel.UpdateOutputFolderPathCommand?.Execute(SelectedOutputFolder?.Path);

            ViewModel.LoadPhotosCommand?.ExecuteAsync(SelectedInputFolder?.Path);
        }
    }

    private async Task<StorageFolder?> selectFolderAsync()
    {
        FolderPicker folderPicker = new FolderPicker();
        folderPicker.FileTypeFilter.Add("*");
        IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
        WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);

        return await folderPicker.PickSingleFolderAsync();
    }
    private async void SelectedInputFolderButton_Click(object sender, RoutedEventArgs e)
    {
        StorageFolder? folder = await selectFolderAsync();
        if (folder is not null)
        {
            SelectedInputFolder = folder;
            SelectedInputFolderTextBox.Text = SelectedInputFolder.Path;
        }
    }

    private async void SelectedOutputFolderButton_Click(object sender, RoutedEventArgs e)
    {
        StorageFolder? folder = await selectFolderAsync();
        if (folder is not null)
        {
            SelectedOutputFolder = folder;
            SelectedOutputFolderTextBox.Text = SelectedOutputFolder.Path;
        }
    }
 
    private void FolderStructureCheckBox_Click(object sender, RoutedEventArgs e)
    {
        updateOutputFolderExample();
    }

    private void updateOutputFolderExample()
    {
        var example = @"[Output]";

        if (SelectedOutputFolder?.Path.Length > 0)
        {
            example = SelectedOutputFolder.Path;
        }

        var dateFormat = createDataFolderFormat();
        if (dateFormat.Length > 0)
        {
            example += DateTime.Now.ToString(dateFormat, CultureInfo.InvariantCulture);
        }
        example += @"\[Filename]";

        ExampleTextBlock.Text = example;
    }
    private string createDataFolderFormat()
    {
        var format = string.Empty;

        if (CreateDateFolderCheckBox.IsChecked is true)
            format += @"\\yy-MM-dd";

        if (CreateYearFolderCheckBox.IsChecked is true)
            format += @"\\yy";
        if (CreateMonthFolderCheckBox.IsChecked is true)
            format += @"\\MM";
        if (CreateDayFolderCheckBox.IsChecked is true)
            format += @"\\dd";

        return format;
    }
}
