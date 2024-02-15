using Humanizer;
using Humanizer.Bytes;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;

using PhotoOrganizer.Interfaces;
using MetadataExtractor.Formats.Iptc;




namespace PhotoOrganizer.Services;

public class MetadataSerice : IMetadataSerice
{
    public async Task<string?> GetHumanizedFileSize(StorageFile file)
    {
        BasicProperties basicProperties = await file.GetBasicPropertiesAsync();
        return new ByteSize(basicProperties.Size).Humanize();
    }

    public DateTime? GetTakendDate(StorageFile file)
    {
        IReadOnlyList<Directory>? directories = ImageMetadataReader.ReadMetadata(file.Path);
        ExifSubIfdDirectory? directory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
        if (directory != null)
        {
            if (directory?.TryGetDateTime(ExifDirectoryBase.TagDateTimeOriginal, out DateTime dateTime) is true)
                return dateTime;
        }

        return null;
    }
}
