namespace Models;

public class CustomFile {
    public int ID { get; set; }
    public string Name {get; set;}
    public long Size { get; set; }
    public byte[] Bytes { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is CustomFile file &&
               ID == file.ID &&
               Name == file.Name &&
               Size == file.Size &&
               Bytes.SequenceEqual(file.Bytes);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ID, Name, Size, Bytes);
    }
}