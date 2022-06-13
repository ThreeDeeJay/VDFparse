namespace VDFparse;

public class AppInfoReader : IVDFFileReader
{
    public uint Magic { get => 0x07_56_44_27; }

    public List<Dataset> Read(BinaryReader reader)
    {
        var Datasets = new List<Dataset>();
        while (true)
        {
            var id = reader.ReadUInt32();
            if (id == 0)
            {
                return Datasets;
            }

            Datasets.Add(new AppDataset
            {
                Id = id,
                Size = reader.ReadUInt32(),
                InfoState = reader.ReadUInt32(),
                LastUpdated = DateTimeFromUnixTimestamp(reader.ReadUInt32()),
                Token = reader.ReadUInt64(),
                Hash = reader.ReadBytes(20),
                ChangeNumber = reader.ReadUInt32(),
                Data = KVParser.Parse(reader),
            });
        }
    }

    private static DateTime UnixBaseTime { get; } = new DateTime(1970, 1, 1);
    private static DateTime DateTimeFromUnixTimestamp(uint unixTime)
    {
        return UnixBaseTime.AddSeconds(unixTime);
    }
}

public class AppDataset : Dataset
{
    public uint Size { get; init; }
    public uint InfoState { get; init; }
    public DateTime LastUpdated { get; init; }

}
