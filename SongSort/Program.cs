#region

using Id3;

#endregion

const string directory = @"C:\Users\kent\Music\jd";
var files = Directory.GetFiles(directory, "*.mp3", SearchOption.AllDirectories);

foreach (var file in files)
{
    Id3Tag[] tags;
    using (var d = new Mp3(file))
    {
        tags = d.GetAllTags().ToArray();
    }

    foreach (var tag in tags)
        if (tag.Genre.IsAssigned && tag.Genre != "ÿ")
        {
            var gen = tag.Genre.Value.Replace('\0', '-'); //replace null chars with random char
            var newPath = Path.Combine(directory, gen, Path.GetFileName(file));


            try
            {
                File.Move(file, newPath);
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory(Path.Combine(directory, gen));
                File.Move(file, newPath);
            }


            Console.WriteLine($"Moved {file} to" + Path.Combine(directory, tag.Genre, Path.GetFileName(file)));

            break;
        }
}
