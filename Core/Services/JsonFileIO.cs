using Windows.Storage;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

public static class JsonFileIO
{
    public static async Task WriteTextToFileAsync(string filename, string content)
    {
        try
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFolder userCostumeFolder = await localFolder.CreateFolderAsync("user_costume", CreationCollisionOption.OpenIfExists);
            StorageFolder unitsFolder = await userCostumeFolder.CreateFolderAsync("units", CreationCollisionOption.OpenIfExists);
            StorageFile sampleFile = await unitsFolder.CreateFileAsync(filename, CreationCollisionOption.FailIfExists);

            await Windows.Storage.FileIO.WriteTextAsync(sampleFile, content);
            Console.WriteLine("File written successfully.");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine("Error: Access to the path is denied. " + ex.Message);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine("Error: Invalid file name. " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An unexpected error occurred: " + ex.Message);
        }
    }

    public static async Task<T> ReadJsonFromFileAsync<T>(string filename) where T : class
    {
        try
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile jsonFile = await localFolder.GetFileAsync(filename);
            string jsonString = await FileIO.ReadTextAsync(jsonFile);
            T result = JsonConvert.DeserializeObject<T>(jsonString);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
            return null;
        }
    }
    public static async Task<string[]> GetFilesName(string folderName)
    {
        try
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFolder folder = await localFolder.GetFolderAsync(folderName);
            var files = await folder.GetFilesAsync();
            string[] fileNames = new string[files.Count];
            for (int i = 0; i < files.Count; i++)
            {
                fileNames[i] = files[i].Name;
            }
            return fileNames;
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
            return new string[1] { "No files found." };
        }
    }
}